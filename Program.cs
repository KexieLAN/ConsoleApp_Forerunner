using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleApp_Forerunner
{
    class Program
    {
        static void Main(string[] args)
        {
            //byte[] bufferBytes = new byte[] { 0 };
            /*//系统信息Test
            byte[] bufferBytes = new byte[]
            {0x51,0x4E,0x00,0x10,
                0x23,
                0x60,
                0x00,
                0x01,
                0x01,0x00,0x01,0x00,0x02,0x00,0x03,
                0x12,0x45 };
            */

            //故障信息Test
            /*
            byte[] bufferBytes = new byte[]
            {0x51,0x4E,0x00,0x10,
                0x23,
                0x70,
                0x00,
                0x01,

                0x01,
                0x00,0x00,
                0x01,
                0x10,
                0xFF,
                0x00,

                0x12,0x45 };
            */
            //分区信息Test
            byte[] bufferBytes = new byte[]
            {0x51,0x4E,0x00,0x10,
                0x23,
                0xA0,
                0x00,
                0x01,

                0x01,
                0x00,0x00,
                0x01,0x02,
                0x00,0x01,
                0xFF,0xFF,
                0x00,0x01,
                0x00,0x01,
                0x00,0x01,
                0x00,0x01,
                0x00,0x01,
                0x00,0x01,
                0x00,0x01,
                0x00,0x01,

                0x12,0x45 };
            
            //探测器信息Test

            //灭火器信息Test

            /*//网络相关代码
            //SocketAddress add = new SocketAddress(AddressFamily.InterNetwork);
            IPAddress ipAddress = new IPAddress(new byte[] {127, 0, 0, 1});
            int pot=Convert.ToInt32(Console.ReadLine());
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, pot);

            //创建Socket
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //绑定地址
            socket.Bind(ipEndPoint);
            socket.Listen(100);//监听
            Socket temp = socket.Accept();//接受链接
            temp.Receive(bufferBytes);

            */
            //无关代码
            //bufferBytes = Convert.FromBase64String(Console.ReadLine());
            Info_Adepter info_Adepter = new Info_Adepter();

            Transform_Info_From_T tmp = info_Adepter.String_2_Struct(bufferBytes);

            info_Adepter.Show_Info(tmp);

            Data_Parsing dataParsing = new Data_Parsing();

            switch (dataParsing.Function_Code_Judge(tmp))
            {
                case 0x60:
                    info_Adepter.Show_Info(dataParsing.sysInfos, dataParsing.sysInfos.Count, dataParsing.sysInfos_num);
                    break;
                case 0x70:
                    info_Adepter.Show_Info(dataParsing.bugInfos, dataParsing.bugInfos.Count, dataParsing.bugInfos_num);
                    break;
                case 0x80:
                    info_Adepter.Show_Info(dataParsing.partitionInfos, dataParsing.partitionInfos.Count, dataParsing.partitionInfos_num);
                    break;
                case 0x90:
                    info_Adepter.Show_Info(dataParsing.detectorInfos, dataParsing.detectorInfos.Count, dataParsing.detectorInfos_num);
                    break;
                case 0xA0:
                    info_Adepter.Show_Info(dataParsing.fireExtinguisherInfos, dataParsing.fireExtinguisherInfos.Count, dataParsing.fireExtinguisherInfos_num);
                    break;
                    ;
            }

            Console.ReadKey();
        }
    }
}
