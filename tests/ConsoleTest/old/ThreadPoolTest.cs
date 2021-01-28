using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ConsoleTest
{
    public class ThreadPoolTest
    {
        public static void Start()
        {
            var messages = Enumerable.Range(1, 500)
                .Select(i => $"Message {i}")
                .ToArray();

            var timer = Stopwatch.StartNew();

            ThreadPool.GetAvailableThreads(out var available_worker_threads, out var available_completion_threads);
            ThreadPool.GetMinThreads(out var min_worker_threads, out var min_completion_threads);
            ThreadPool.GetMaxThreads(out var max_worker_threads, out var max_completion_threads);

            //ThreadPool.SetMinThreads(2, 2);
            //ThreadPool.SetMaxThreads(50, 50);

            for (int i = 0; i < messages.Length; i++)
            {
                //// это без использования пула потоков
                //var local_i = i;
                //new Thread(() => ProcessMessage(messages[local_i])) { IsBackground = true }.Start();

                ThreadPool.QueueUserWorkItem(o => ProcessMessage((string) o), messages[i]);
            }

            timer.Stop();
            Console.Title = $"Обработка заняла {timer.Elapsed.TotalSeconds}";
        }

        private static void ProcessMessage(string message)
        {
            Console.WriteLine($"Обработка сообщения {message}");
            //Thread.Sleep(1000);
            Console.WriteLine($"Обработка сообщения {message} закончена");
        }
    }
}
