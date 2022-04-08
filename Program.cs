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
            string getInfo;

            SocketAddress add = new SocketAddress(AddressFamily.InterNetwork);
            IPAddress ipAddress = new IPAddress(new byte[] {127, 0, 0, 1});
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 1234);

            getInfo=Console.ReadLine();
            Info_Adepter info_Adepter = new Info_Adepter();

            //接受地址。
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipEndPoint);
            socket.Listen(100);
            Socket temp = socket.Accept();

            info_Adepter.Show_Info(info_Adepter.String_2_Struct(getInfo));
            Console.ReadKey();
        }
    }
}
