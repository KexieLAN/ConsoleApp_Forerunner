using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp_Forerunner
{
    public struct Transform_Info_From_T
    {
        public char START1, START2;    //同步字符
        public UInt16 Info_Length;          //长度
        public byte ID;                           //ID
        public byte Function_Code;      //功能码
        /*60H   系统信息
         * 70H  故障信息
         * 80H  分区信息
         * 90H  探测器信息
         * A0H  灭火器信息
         */
        public byte Function_Tag;         //功能标识
        /*0     读取成功
         * 1    读取失败
         */
        public byte Data_Form;             //数据格式
        public string Data;                    //数据内容
        public byte CRC;                       //CRC校验
        public byte END;                       //结束码
    };

    public struct Transform_Info_To_T
    {
        public char START1, START2;        //同步字符
        public UInt16 Info_Length;              //长度
        public byte ID;                              //ID
        public byte Function_Code;         //功能码
        public byte Recall_Tag;                //回复标识
        /*上传数据功能码   肯定回复标识
         * 7FH  否定回复标识
         */
        public byte Recall_Code;           //回复码
        /*00H   默认（肯定）
         *11H   服务不支持
         *E0H   CRC校验失败
         */
        public byte CRC;                       //CRC校验码
        public byte END;                      //结束码
    }

    public class Info_Adepter   //用来转换socket接收到的报文，转化成对应的结构体
    {
        public Transform_Info_From_T String_2_Struct(string src_Info)      //解析socket报文，将其转化为 结构体
        {
            Transform_Info_From_T carry=new Transform_Info_From_T { };
            carry.START1 = src_Info[0];
            carry.START2 = src_Info[1];
            byte tmp1 = Convert.ToByte(src_Info[2]);
            byte tmp2 = Convert.ToByte(src_Info[3]);
            int uni = tmp1;
            uni <<= 8;
            uni += tmp2;
            carry.Info_Length = Convert.ToUInt16(uni);
            //carry.Info_Length = Convert.ToInt16(src_Info[2..4]);
            carry.ID  = Convert.ToByte(src_Info[4]);
            carry.Function_Code  = Convert.ToByte(src_Info[5]);
            carry.Function_Tag = Convert.ToByte(src_Info[6]);
            carry.Data_Form = Convert.ToByte(src_Info[7]);
            int poi = src_Info.LastIndexOf('E');
            carry.CRC = Convert.ToByte(src_Info[poi - 1]);
            carry.END = Convert.ToByte(src_Info[poi]);
            carry.Data = src_Info[8..(poi - 1)];
            return carry;
        }

        private string Struct_2_String(Transform_Info_To_T stt)      //将回复报文转化为String类型，便于socket发送报文
        {
            StringBuilder send_info=new StringBuilder();
            send_info.Append(stt.START1);
            send_info.Append(stt.START2);
            send_info.Append(stt.Info_Length.ToString());
            send_info.Append(stt.ID.ToString());
            send_info.Append(stt.Function_Code.ToString());
            send_info.Append(stt.Recall_Tag.ToString());
            send_info.Append(stt.Recall_Code.ToString());
            send_info.Append(stt.CRC.ToString());
            send_info.Append(stt.END.ToString());
            return send_info.ToString();
        }

        public void Show_Info(Transform_Info_From_T TFT)
        {
            Console.WriteLine("同步字符1：{0}", TFT.START1);
            Console.WriteLine("同步字符2：{0}", TFT.START2);
            Console.WriteLine("信息长度：{0}", TFT.Info_Length);
            Console.WriteLine("ID ：{0}", TFT.ID);
            Console.WriteLine("功能代码：{0}", Convert.ToChar(TFT.Function_Code));
            Console.WriteLine("功能标识：{0}", Convert.ToChar(TFT.Function_Tag));
            Console.WriteLine("数据格式：{0}", Convert.ToChar(TFT.Data_Form));
            Console.WriteLine("数据：{0}", TFT.Data);
            Console.WriteLine("CRC：{0}", Convert.ToChar(TFT.CRC));
            Console.WriteLine("结束：{0}",  Convert.ToChar(TFT.END));
        }

        public void Show_Info(Transform_Info_To_T TTT)
        {

        }
    }

    public class Data_Parsing //用来实现对结构体的解析
    {
        //解析上传的报文的功能代码
        public List<Sys_Info> sysInfos = new List<Sys_Info>();
        public byte sysInfos_num;

        public List<Bug_Info> bugInfos = new List<Bug_Info>();
        public byte bugInfos_num;

        public List<Partition_Info> partitionInfos = new List<Partition_Info>();
        public byte partitionInfos_num;

        public List<Detector_Info> detectorInfos = new List<Detector_Info>();
        public byte detectorInfos_num;

        public List<Fire_Extinguisher_Info> fireExtinguisherInfos = new List<Fire_Extinguisher_Info>();
        public byte fireExtinguisherInfos_num;

        //判定报文需要如何解析。
        public void Function_Code_Judge(Transform_Info_From_T src)
        {
            switch (src.Function_Code)
            {
                case 0x60:
                    //if (src.Function_Tag == 0x00)
                        System_Info_alz(src);
                   //
                        //break;
                    break;
                case 0x70:
                    //if (src.Function_Tag == 0x00)
                        Bug_Info_alz(src);
                    break;
                case 0x80:
                    //if (src.Function_Tag == 0x00)
                        Partition_Info_alz(src);
                    break;
                case 0x90:
                    //if (src.Function_Tag == 0x00)
                        Detector_Info_alz(src);
                    break;
                case 0xA0:
                    //if (src.Function_Tag == 0x00)
                        Fire_Extinguisher_Info_alz(src);
                    break;
                default:
                    break;
            }
        }
        //系统信息解析
        private void System_Info_alz(Transform_Info_From_T src)
        {
            //信息无误解析
            if(src.Function_Tag==0x00)
            {
                Sys_Info ss=new Sys_Info();
                sysInfos_num = Convert.ToByte(src.Data[0]);
                //ushort waring_count,bug_count,tt_count;
                int tmp;
                tmp = Convert.ToInt32(src.Data[1]);
                tmp <<= 8;
                ss.waring_count = Convert.ToUInt16(tmp + Convert.ToInt32(src.Data[2]));
                tmp = Convert.ToInt32(src.Data[3]);
                tmp <<= 8;
                ss.bug_count= Convert.ToUInt16(tmp + Convert.ToInt32(src.Data[4]));
                tmp = Convert.ToInt32(src.Data[5]);
                tmp <<= 8;
                ss.detector_count = Convert.ToUInt16(tmp + Convert.ToInt32(src.Data[6]));
                sysInfos.Add(ss);
            }
            else if(src.Function_Tag==0x01)
            {
                sysInfos_num = 0x00;
            }
            else
            {
                
            }
            return;
        }
        //故障信息解析
        private void Bug_Info_alz(Transform_Info_From_T src)
        {
            if (src.Function_Tag == 0x00)
            {
                string str = src.Data[..src.Data.Length];
                Bug_Info bug = new Bug_Info();
                bugInfos_num = Convert.ToByte(str[0]);
                int i = 1;
                while (i < str.Length)
                {
                    bug.reserved1 = Convert.ToByte(str[i]);
                    bug.reserved2 = Convert.ToByte(str[++i]);
                    bug.def_zone = Convert.ToByte(str[++i]);
                    bug.device_type = Convert.ToByte(str[++i]);
                    bug.device_number = Convert.ToByte(str[++i]);
                    bug.bug_code = Convert.ToByte(str[++i]);
                    bugInfos.Add(bug);
                    i++;
                }
                
            }
            else if (src.Function_Tag == 0x01)
            {
                bugInfos_num = 0x00;
            }
        }
        //分区信息解析
        private void Partition_Info_alz(Transform_Info_From_T src)
        {
            if (src.Function_Tag == 0x00)
            {
                Partition_Info partitionInfo = new Partition_Info();
                string str = src.Data.Substring(0, src.Data.Length);
                int i = 0;
                partitionInfos_num = Convert.ToByte(str[i++]);
                while (i<str.Length)
                {
                    //预留
                    partitionInfo.reserved = DecodeUshort(str, i);
                    //防护区
                    partitionInfo.def_zone = DecodeUshort(str,i+=2);
                    //报警等级
                    partitionInfo.waring_level = DecodeUshort(str, i+=2);
                    //故障
                    partitionInfo.bug = DecodeUshort(str, i+=2);
                    //手动模式
                    partitionInfo.handla_mode = DecodeUshort(str, i+=2);
                    //自动模式
                    partitionInfo.auto_mode = DecodeUshort(str, i+=2);
                    //手动启动
                    partitionInfo.handla_boot = DecodeUshort(str, i+=2);
                    //手动急停
                    partitionInfo.handla_shut = DecodeUshort(str, i += 2);
                    //启动控制
                    partitionInfo.boot_ctrl = DecodeUshort(str, i += 2);
                    //延时
                    partitionInfo.delay = DecodeUshort(str, i += 2);
                    //启动喷洒
                    partitionInfo.spray_start = DecodeUshort(str, i += 2);
                    //喷洒
                    partitionInfo.spraying = DecodeUshort(str, i += 2);
                    i += 2;
                    partitionInfos.Add(partitionInfo);
                }
            }
            else if (src.Function_Tag == 0x01)
            {
                partitionInfos_num = 0x00;
            }
        }
        //探测器信息解析
        private void Detector_Info_alz(Transform_Info_From_T src)
        {
            if (src.Function_Tag == 0x00)
            {
                Detector_Info detectorInfo = new Detector_Info();
                string str = src.Data.Substring(0, src.Data.Length);
                int i=0;
                detectorInfos_num = Convert.ToByte(src.Data[i++]);
                while (i < str.Length)
                {
                    detectorInfo.reserved = DecodeUshort(str, i += 2);
                    detectorInfo.def_zone = DecodeUshort(str, i += 2);
                    detectorInfo.type = DecodeUshort(str, i += 2);
                    detectorInfo.ID = DecodeUshort(str, i += 2);
                    detectorInfo.waring_level = DecodeUshort(str, i += 2);
                    detectorInfo.temperaturer = DecodeUshort(str, i += 2);
                    detectorInfo.CO = DecodeUshort(str, i += 2);
                    detectorInfo.VOC = DecodeUshort(str, i += 2);
                    detectorInfo.smoke = DecodeUshort(str, i += 2);
                    i += 2;
                    detectorInfos.Add(detectorInfo);
                }
            }
            else if (src.Function_Tag == 0x01)
            {
                detectorInfos_num = 0x00;
            }
        }
        //灭火器信息解析
        private void Fire_Extinguisher_Info_alz(Transform_Info_From_T src)
        {
            if (src.Function_Tag == 0x00)
            {

            }
            else if (src.Function_Tag == 0x01)
            {

            }
        }

        //解析双字节数据Ushort/Int16
        private ushort DecodeUshort(string ss, int i)
        {
            int tmp;
            tmp = Convert.ToInt32(ss[i]);
            tmp <<= 8;
            return Convert.ToUInt16(tmp + Convert.ToInt32(ss[++i]));
        }
    }
}
