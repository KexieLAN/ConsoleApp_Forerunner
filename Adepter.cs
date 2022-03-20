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
            ushort tmp1 = Convert.ToByte(src_Info[2]);
            ushort tmp2 = Convert.ToByte(src_Info[3]);
            carry.Info_Length = (ushort)(tmp1 + tmp2);
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

    class Data_Parsing //用来实现对结构体的解析
    {
        //解析上传的报文的功能代码
        public void Function_Code_Judge(Transform_Info_From_T src)
        {
            switch (src.Function_Code)
            {
                case 0x60:
                    if (src.Function_Tag == 0x00)
                        System_Info();
                    else
                        break;
                    break;
                case 0x70:
                    if (src.Function_Tag == 0x00)
                        Bug_Info();
                    break;
                case 0x80:
                    if (src.Function_Tag == 0x00)
                        Partition_Info();
                    break;
                case 0x90:
                    if (src.Function_Tag == 0x00)
                        Detector_Info();
                    break;
                case 0xA0:
                    if (src.Function_Tag == 0x00)
                        Fire_Extinguisher_Info();
                    break;
                default:
                    break;
            }
        }
        //系统信息解析
        private void System_Info()
        {

        }
        //故障信息解析
        private void Bug_Info()
        {

        }
        //分区信息解析
        private void Partition_Info()
        {

        }
        //探测器信息解析
        private void Detector_Info()
        {

        }
        //灭火器信息解析
        private void Fire_Extinguisher_Info()
        {

        }
    }
}
