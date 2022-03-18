using System;
using System.Net;
using System.Threading;

namespace ConsoleApp_Forerunner
{
    class Program
    {
        static void Main(string[] args)
        {
            string getInfo;
            getInfo=Console.ReadLine();
            Info_Adepter info_Adepter = new Info_Adepter();
            info_Adepter.Show_Info(info_Adepter.String_2_Struct(getInfo));
            Console.ReadKey();
        }
    }
}
