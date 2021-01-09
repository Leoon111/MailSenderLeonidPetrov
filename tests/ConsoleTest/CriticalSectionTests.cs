using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ConsoleTest
{
    class CriticalSectionTests
    {
        public static void Start()
        {
            #region Мютексы и семафоры

            //LockSynchronizationTest();
            //EventWaitHandleTest();

            //var mutex1 = new Mutex(true, "Тестовый мютекс", out var created1);
            //var mutex2 = new Mutex(true, "Тестовый мютекс", out var created2);

            //// синхронизация между потоками
            //// первый WaitOne пройдет и будет работать
            //mutex1.WaitOne();
            //// второй WaitOne будет ждать освобождения мютекса
            //mutex1.WaitOne();
            //// мютекс освобождается командой ReleaseMutex там, где его заняли
            //mutex1.ReleaseMutex();

            //// Semaphore - это мютекс в который можно войти много рай, указывается ограничение входа
            //// можно считать, что мютекс это new Semaphore(0, 1) на один вход
            //var semaphore = new Semaphore(0, 10);
            //// так же работает WaitOne и Release, только здесь например их может быть 10 потоков.
            //semaphore.WaitOne();
            //semaphore.Release();

            #endregion

        }

        private static void EventWaitHandleTest()
        {
            var manual_reset_event = new ManualResetEvent(false);
            var auto_reset_event = new AutoResetEvent(false);

            EventWaitHandle starter = manual_reset_event;

            for (int i = 0; i < 10; i++)
            {
                var local_i = i;
                new Thread(() =>
                {
                    Console.WriteLine($"Поток {local_i} запущен");
                    starter.WaitOne();
                    // при manual_reset_event
                    starter.Reset();
                    Console.WriteLine($"Поток {local_i} завершил свою работу");
                }).Start();
            }

            Console.WriteLine("Все потоки созданы и готовы к работе.");

            Console.ReadLine();
            starter.Set();

            // при AutoResetEvent
            Console.ReadLine();
            starter.Set();

            Console.ReadLine();
            starter.Set();

            Console.ReadLine();
            starter.Set();
        }

        private static void LockSynchronizationTest()
        {
            var threads = new Thread[10];

            for (int i = 0; i < threads.Length; i++)
            {
                // переменная для того, чтоб не было замыкания
                var local_i = i;
                threads[i] = new Thread(() => PrintData($"Сообщение из потока: {local_i}", 10));
            }

            for (var i = 0; i < threads.Length; i++)
                threads[i].Start();
        }

        private static readonly object __SyncRoot = new object();
        private static void PrintData(string Message, int Count)
        {
            for (int i = 0; i < Count; i++)
            {
                lock (__SyncRoot)
                {
                    Console.Write($"Thread id: {Thread.CurrentThread.ManagedThreadId}");
                    Console.Write("\t");
                    Console.Write(Message);
                    //Thread.Sleep(15);
                    Console.WriteLine();
                }

                //Monitor.Enter(__SyncRoot);
                //try
                //{
                //    Console.Write($"Thread id: {Thread.CurrentThread.ManagedThreadId}");
                //    Console.Write("\t");
                //    Console.Write(Message);
                //    //Thread.Sleep(15);
                //    Console.WriteLine();
                //}
                //finally
                //{
                //    Monitor.Exit(__SyncRoot);
                //}
            }
        }
    }

    public class FileLogger : ContextBoundObject
    {
        private readonly string _LogFileName;

        public FileLogger(string LogFileName)
        {
            _LogFileName = LogFileName;
        }

        public void Log(string Message)
        {
            File.WriteAllText(_LogFileName, Message);
        }
    }
}
