using System;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //TPLOverview.Start();

            var task = AsyncAwaitTest.StartAsync();
            var message_tasks = AsyncAwaitTest.ProcessDataTestAsync();

            Console.WriteLine("Тестовая задача запущена и мы ее ждем");

            Task.WaitAll(task, message_tasks);

            Console.WriteLine("Главный поток завершил работу");
            Console.ReadLine();
        }
    }
}
