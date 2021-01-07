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
            //LockSynchronizationTest();
            //EventWaitHandleTest();


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
