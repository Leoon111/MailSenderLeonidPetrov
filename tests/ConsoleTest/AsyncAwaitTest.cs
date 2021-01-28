using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class AsyncAwaitTest
    {
        private static void PrintThreadInfo(string message = "")
        {
            var current_thread = Thread.CurrentThread;
            Console.WriteLine("Thread id: {0}, Task id: {1} {2}", current_thread.ManagedThreadId, Task.CurrentId, message);
        }

        public static async Task StartAsync()
        {
            Console.WriteLine("Запуск асинхронного метода выполняется синхронно!!! ");
            PrintThreadInfo("При входе в метод StartAsync");

            //var result_task = GetStringResultAsync();
            //var result = await result_task;
            //var result = await GetStringResultAsync();
            var result = await GetStringResultRealAsync();

            Console.WriteLine($"Был получен результат {result}");
            PrintThreadInfo("При печати результата");

            var result1 = await GetStringResultRealAsync();

            Console.WriteLine($"Был получен результат {result}");
            PrintThreadInfo("При печати результата");

            for (var i = 0; i < 10; i++)
            {
                var result3 = await GetStringResultRealAsync();

                Console.WriteLine($"Был получен результат {result}");
                PrintThreadInfo("При печати результата");
            }
        }

        private static async Task<string> GetStringResultAsync()
        {
            PrintThreadInfo("В псевдоасинхронном методе");
            return DateTime.Now.ToString();
            //return Task.FromResult(DateTime.Now.ToString());
        }

        private static Task<string> GetStringResultRealAsync()
        {
            PrintThreadInfo("В начале асинхронного метода");
            return Task.Run(() =>
            {
                PrintThreadInfo("Внутри асинхронного метода");
                return DateTime.Now.ToString();
            });
        }

        public static async Task ProcessDataTestAsync()
        {
            var message = Enumerable.Range(1, 50).Select(i => $"Message {i}");

            var tasks = message.Select(msg => Task.Run(() => LowSpeedPrinter(msg)));

            Console.WriteLine(">>> Подготовка к запуску обработки сообщений ...");
            var runing_tasks = tasks.ToArray();
            Console.WriteLine(">>> Задачи созданы");
            await Task.WhenAll(runing_tasks);
            Console.WriteLine(">>> Обработка всех сообщений завершена");
        }

        private static void LowSpeedPrinter(string msg)
        {
            Console.WriteLine(">>> [Thread id: {1}] Начинаю обработку {0}", msg, Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(100);
            Console.WriteLine(">>> [Thread id: {1}] Сообщение {0} обработано", msg, Thread.CurrentThread.ManagedThreadId);
        }
    }
}
