using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleTest
{
    public class ThreadTests
    {
        public static void Start()
        {
            var my_thread = Thread.CurrentThread;
            var my_thread_id = my_thread.ManagedThreadId;
            my_thread.Name = "Главный поток";

            PrintThreadInfo();
            var timer_thread = new Thread(TimerMethod);
            timer_thread.IsBackground = true;
            timer_thread.Start();

            var printer_thread = new Thread(PrintMessage)
            {
                IsBackground = true,
                Name = "PrinterMessage",
            };
            printer_thread.Start("Поток через передачу object");

            new Thread(() =>
                PrintMessage("Метод с двумя параметрами", 5))
            {
                IsBackground = true,
                Name = "Метод с двумя параметрами"
            }.Start();

            var print_task = new PrintMessageStart("Запуск потока из отдельного класса", 6);
            print_task.Start();

            Console.WriteLine("Останавливаю время...");

            //var current_process = System.Diagnostics.Process.GetCurrentProcess();
            //Process.Start("calc.exe");

            timer_thread.Priority = ThreadPriority.BelowNormal;
            //timer_thread.Interrupt();
            //timer_thread.Abort();

            // Если мы не дождались через .Join() остановки потока, то вызываем .Interrupt();
            _TimerWork = false;
            if (!timer_thread.Join(100))
                timer_thread.Interrupt();

            Console.ReadLine();
        }

        private static void PrintMessage(object param)
        {
            PrintThreadInfo();
            var msg = (string)param;
            var therad_id = Thread.CurrentThread.ManagedThreadId;
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"id: {therad_id}, message: {msg}");
            }
        }

        private static void PrintMessage(string message, int count)
        {
            PrintThreadInfo();

            var therad_id = Thread.CurrentThread.ManagedThreadId;
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"id: {therad_id}, message: {message}");
            }
        }

        private static volatile bool _TimerWork = true;

        public static void TimerMethod()
        {
            PrintThreadInfo();
            while (_TimerWork)
            {
                Console.Title = DateTime.Now.ToString("HH:mm:ss.ffff");
                Thread.Sleep(100);
                //Thread.SpinWait(10); // число, это не доли секунд, это процессорное, не прогнозируемое, время.
            }
        }

        public static void PrintThreadInfo()
        {
            var currentTherad = Thread.CurrentThread;

            Console.WriteLine($"\nId: {currentTherad.ManagedThreadId}, name: {currentTherad.Name}, priority: {currentTherad.Priority}\n");
        }
    }

    class PrintMessageStart
    {
        private readonly string _Message;
        private readonly int _Count;
        private Thread _Thread;

        public PrintMessageStart(string Message, int Count)
        {
            _Message = Message;
            _Count = Count;
            _Thread = new Thread(ThreadMethod) { IsBackground = true };
        }

        public void Start()
        {
            if (_Thread?.IsAlive == false)
                _Thread.Start();
        }

        private void ThreadMethod()
        {
            var therad_id = Thread.CurrentThread.ManagedThreadId;
            for (int i = 0; i < _Count; i++)
            {
                Console.WriteLine($"id: {therad_id}, message: {_Message}");
                Thread.Sleep(10);
            }

            _Thread = null;
        }
    }
}
