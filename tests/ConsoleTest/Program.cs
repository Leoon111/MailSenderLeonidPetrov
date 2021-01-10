using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Channels;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            #region До TPL

            //ThreadTests.Start();
            //CriticalSectionTests.Start();
            //ThreadPoolTest.Start();

            //// напрямую, без отдельного класса
            //var factorial = 0;
            //var sum = 0;
            //var factorial_thread = new Thread(() => factorial = Factorial(10));
            //var sum_thread = new Thread(() => sum = Sum(10));
            //factorial_thread.Start();
            //sum_thread.Start();
            //factorial_thread.Join();
            //sum_thread.Join();

            //// сильно упрощенный вариант работы класса Task
            //var factorial = new MathTask(() => Factorial(10));
            //var sum = new MathTask(() => Sum(10));
            //factorial.Start();
            //sum.Start();
            //Console.WriteLine($"Факториал {factorial.Result}; Сумма {sum.Result}");

            // можно запустить поток асинхронно вот так
            //Action<string> printer = str =>
            //    Console.WriteLine($"Сообщение [th id: {Thread.CurrentThread.ManagedThreadId}]: {str}");
            //printer("Hello");
            //printer.Invoke("Hello World");
            // в Framework можно было так вот
            //var process_control = printer.BeginInvoke(
            //    "QWE", result => { Console.WriteLine("Операция завершена"); }, 123);
            //
            //var worker = new BackgroundWorker();
            //worker.DoWork += (sender, args) =>
            //{
            //    var w = (BackgroundWorker) sender;
            //    w.ReportProgress(100);
            //    w.CancelAsync();
            //    // Асинхронная операция
            //};
            //worker.ProgressChanged += (s, e) => Console.WriteLine($"Прогресс {e.ProgressPercentage}");
            //worker.RunWorkerCompleted += (s, e) => Console.WriteLine("Завершено");
            //worker.RunWorkerAsync();
            // Вот так вот можно было запустить что то асинхронно до эпохи TPL

            #endregion



            Console.WriteLine("Главный поток завершил работу");
            Console.ReadLine();
        }

        private static int Factorial(int n)
        {
            var factorial = 1;
            for (var i = 1; i <= n; i++)
                factorial *= n;
            return factorial;
        }

        private static int Sum(int n)
        {
            var sum = 1;
            for (var i = 1; i <= n; i++)
                sum += n;
            return sum;
        }
    }

    class MathTask
    {
        private Thread _CalculationThread;
        private int _Result;
        private bool _IsComplited;

        public bool IsComplited => _IsComplited;

        public int Result
        {
            get
            {
                if (!_IsComplited)
                    _CalculationThread.Join();
                return _Result;
            }
        }

        public MathTask(Func<int> Calculation)
        {
            _CalculationThread = new Thread(
                () =>
                {
                    _Result = Calculation();
                    _IsComplited = true;
                })
            { IsBackground = true };
        }

        public void Start() => _CalculationThread.Start();
    }
}
