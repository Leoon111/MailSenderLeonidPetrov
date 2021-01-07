using System;
using System.Diagnostics;
using System.Threading;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //ThreadTests.Start();

            CriticalSectionTests.Start();

            Console.WriteLine("Главный поток завершил работу");
            Console.ReadLine();
        }
    }

    
}
