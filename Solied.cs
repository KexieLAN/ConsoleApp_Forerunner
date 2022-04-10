using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp_Forerunner
{
    public class Filed_Info
    {

    }
    public class Sys_Info
    {
        //public Sys_Info per;
        public ushort waring_count;     //总报警个数
        public ushort bug_count;         //总故障个数
        public ushort detector_count;  //总探测器个数
        //public Sys_Info next;
    }

    public class Bug_Info
    {
        //public Bug_Info per;
        public byte reserved1;          //预留扩展
        public byte reserved2;
        public byte def_zone;           //防护区
        public byte device_type;        //设备类型
        public byte device_number;  //设备编号
        public byte bug_code;           //故障代码
        //public Bug_Info next;
    }

    public class Partition_Info
    {
        //public Partition_Info per;
        public ushort reserved;         //预留扩展
        public ushort def_zone;         //防护区
        public ushort waring_level;     //警报等级
        public ushort bug;                  //故障
        public ushort handla_mode;  //手动模式
        public ushort auto_mode;      //自动模式
        public ushort handla_boot;      //手动启动
        public ushort handla_shut;      //手动急停
        public ushort boot_ctrl;            //启动控制
        public ushort delay;                //延时
        public ushort spray_start;   //启动喷洒
        public ushort spraying;             //喷洒
        //public Partition_Info next;
    }

    public class Detector_Info
    {
        //public Detector_Info per;
        public ushort reserved;         //预留扩展
        public ushort def_zone;         //防护区
        public ushort type;                 //类型
        public ushort ID;                   //ID
        public ushort waring_level;     //警报等级
        public ushort temperaturer;     //温度
        public ushort CO;                   //CO
        public ushort VOC;                  //VOC
        public ushort smoke;                //烟雾
        //public Detector_Info next;
    }

    public class Fire_Extinguisher_Info
    {
        //public Detector_Info per;
        public ushort sharpNumber;
        //public Detector_Info next;
    }

}
