using System;
using System.Threading;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var my_thread = Thread.CurrentThread;
            var my_thread_id = my_thread.ManagedThreadId;
            my_thread.Name = "Главный поток";

            //PrintThreadInfo();
            var timer_thread = new Thread(TimerMethod);
            timer_thread.IsBackground = true;
            timer_thread.Start();

            Console.WriteLine("Главный поток завершил работу");
            Console.ReadLine();
        }

        public static void TimerMethod()
        {
            PrintThreadInfo();
            while (true)
            {
                Console.Title = DateTime.Now.ToString("HH:mm:ss.ffff");
                Thread.Sleep(100);
                //Thread.SpinWait(10); // число, это не доли секунд, это процессорное, не прогнозируемое, время.
            }
        }

        public static void PrintThreadInfo()
        {
            var currentTherad = Thread.CurrentThread;

            Console.WriteLine($"Id: {currentTherad.ManagedThreadId}, name: {currentTherad.Name}, priority: {currentTherad.Priority}");
        }
    }
}
