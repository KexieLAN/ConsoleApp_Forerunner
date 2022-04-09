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
            byte[] bufferBytes = Array.Empty<byte>(); 

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

            
            //无关代码
            getInfo = Console.ReadLine();
            Info_Adepter info_Adepter = new Info_Adepter();

            info_Adepter.Show_Info(info_Adepter.String_2_Struct(getInfo));
            Console.ReadKey();
        }
    }
}
