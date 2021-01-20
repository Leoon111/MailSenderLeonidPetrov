using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest
{
    static class TPLOverview
    {
        public static void Start()
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

            //Parallel.Invoke(
            //    ParallelInvokeMethod,
            //    ParallelInvokeMethod,
            //    ParallelInvokeMethod,
            //    () => Console.WriteLine("Еще один метод ..."));

            //Parallel.Invoke(
            //    new ParallelOptions { MaxDegreeOfParallelism = 2},
            //    ParallelInvokeMethod,
            //    ParallelInvokeMethod,
            //    ParallelInvokeMethod,
            //    ParallelInvokeMethod,
            //    ParallelInvokeMethod,
            //    () => Console.WriteLine("Еще один метод ..."));

            Action<string> printer = str =>
            {
                Console.WriteLine($"Сообщение [th id: {Thread.CurrentThread.ManagedThreadId}]: {str}");
                Thread.Sleep(100);
            };

            //Parallel.For(0, 100, i => printer(i.ToString()));
            //Parallel.For(0, 100, new ParallelOptions {MaxDegreeOfParallelism = 2}, i => printer(i.ToString()));
            //var result = Parallel.For(
            //    0,
            //    100, 
            //    new ParallelOptions { MaxDegreeOfParallelism = 2 }, 
            //    (i, state) =>
            //    {
            //        printer(i.ToString());
            //        if(i > 10)
            //            state.Break();
            //    });
            //Console.WriteLine($"Выполнено {result.LowestBreakIteration} итераций.");

            var messages = Enumerable.Range(1, 500).Select(i => $"Message {i}");//.ToArray();

            //Parallel.ForEach(messages, message => printer(message));
            //Parallel.ForEach(messages, new ParallelOptions { MaxDegreeOfParallelism = 2 }, message => printer(message));

            //foreach (var message in messages.Where(msg => msg.EndsWith("0")))
            //    printer(message);
            // так же через LINQ можно
            //var messages_coutn = messages
            //    .Where(msg => msg.EndsWith("0"))
            //    .Count();

            // с выходом TPL у LINQ появился параллельный режим.
            //var messages_coutn = messages
            //    .AsParallel()
            //    .Where(msg =>
            //    {
            //        printer(msg);
            //        return msg.EndsWith("0");
            //    })
            //    .Count();

            //// всегда можно перейти обратно к параллельному режиму
            //var cancellation = new CancellationTokenSource(); // объект, источник токенов отмены
            ////cancellation.Token.ThrowIfCancellationRequested(); // внутри потока вызывается этот метод, если флаг токена поднят, то будет аварийная остановка потока.
            //var messages_coutn1 = messages
            //    .AsParallel() // все после этого выполняется параллельно (распараллеливание)
            //    .WithDegreeOfParallelism(2) // максимальное количество потоков, участвующие в процессе
            //    .WithCancellation(cancellation.Token) // токен отмены, когда он поднят, аварийно останавливается
            //    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
            //    .Where(msg =>
            //    {
            //        printer(msg);
            //        return msg.EndsWith("0");
            //    })
            //    .AsSequential() // дальше выполняется в один поток (объединение обратно в последовательность)
            //    .Count();

            // быстрый оптимизированный с точки зрения производительности способ запуска задач
            //var printer_task = Task.Run(() => printer("Hello World"));

            //// Метод Factory имеет расширенный список запуска задач, но работает чуть медленнее
            //var printer_task1 = Task.Factory.StartNew(() => printer("Hello World"));
            //// сложнее, потому что передаются дополнительные параметры
            //var printer_task2 = Task.Factory.StartNew(obj => printer((string)obj), "Hello World");

            //printer_task.Wait(); // не рекомендуется к использованию

            // костыльный способ запуска задачи с возвращением результата
            //var result_task = new Task<int>(() =>
            //{
            //    Thread.Sleep(100);
            //    return 45;
            //});

            var result_task = Task.Run(() =>
            {
                Thread.Sleep(100);
                return 41;
            });
            //var result = result_task.Result; // не рекомендуется, блокировка, по сути это костыли
            ////var result1 = result_task.Result;

            var result_task1 = Task.Run(() =>
            {
                Thread.Sleep(100);
                return 41;
            });

            Task.WaitAll(result_task1, result_task); // костыли!
            var index = Task.WaitAny(result_task1, result_task);
        }

        public static void ParallelInvokeMethod()
        {
            Console.WriteLine($"ThrID: {Thread.CurrentThread.ManagedThreadId} - started");
            Thread.Sleep(250);
            Console.WriteLine($"ThrID: {Thread.CurrentThread.ManagedThreadId} - finished");
        }

        public static void ParallelInvokeMethod(string msg)
        {
            Console.WriteLine($"ThrID: {Thread.CurrentThread.ManagedThreadId} - started: {msg}");
            Thread.Sleep(250);
            Console.WriteLine($"ThrID: {Thread.CurrentThread.ManagedThreadId} - finished: {msg}");
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
}
