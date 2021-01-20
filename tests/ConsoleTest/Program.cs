using System;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TPLOverview.Start();

            Console.WriteLine("Главный поток завершил работу");
            Console.ReadLine();
        }
    }
}
