using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace WpfMailSenderTestLeonidPetrov
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //private void ComputeResultButton_Click(object sender, RoutedEventArgs e)
        //{
        //    // здесь в один поток
        //    //var result = GetResultHard();
        //    //ResultText.Text = result;

        //    new Thread(() =>
        //    {
        //        // так нельзя, потому что Интерфейс - это критическая секция и будет ругаться, что не из потока интерфейса.
        //        //ResultText.Text = GetResultHard();

        //        // либо
        //        //Dispatcher
        //        //ResultText.Dispatcher
        //        //App.Current.Dispatcher

        //        var result = GetResultHard();
        //        UpdateResultValue(result);
        //    }){ IsBackground = true}.Start();
        //}

        //public void UpdateResultValue(string Result)
        //{
        //    if (Dispatcher.CheckAccess())
        //        ResultText.Text = Result;
        //    else
        //        Dispatcher.Invoke(() => UpdateResultValue(Result));
        //}

        //private string GetResultHard()
        //{
        //    for (int i = 0; i < 500; i++)
        //    {
        //        Thread.Sleep(10);
        //    }

        //    return "Hello!";
        //}
        private async void OnOpenFileClick(object sender, RoutedEventArgs e)
        {
            // Мы находились в TheradId == 1
            await Task.Yield(); // Даем время на обработку сообщений пользовательского интерфейса
            // Мы снова в ThreadID == 1

            string filePath =
                @"E:\MyApps\C#\C# Уровень 3\MailSenderLeonidPetrov\WpfMailSenderTestLeonidPetrov\WpfMailSenderTestLeonidPetrov";

            var dialog = new OpenFileDialog
            {
                Title = "Выбор файла для чтения",
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                RestoreDirectory = true,
                InitialDirectory = Directory.Exists(filePath)
                    ? filePath
                    : string.Empty
            };


            if (dialog.ShowDialog() != true) return;

            //var dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            //using (var reader = new StreamReader(dialog.FileName))
            //{
            //    while (!reader.EndOfStream)
            //    {
            //        var line = await reader.ReadLineAsync().ConfigureAwait(false);
            //        var words = line.Split(' ');
            //        //Thread.Sleep(100);
            //        //await Task.Delay(1);
            //        foreach (var word in words)
            //            if (dict.ContainsKey(word))
            //                dict[word]++;
            //            else
            //                dict.Add(word, 1);
            //        //ProgressInfo.Value = reader.BaseStream.Position / (double)reader.BaseStream.Length;
            //        ProgressInfo.Dispatcher.Invoke(() =>
            //            ProgressInfo.Value = reader.BaseStream.Position / (double) reader.BaseStream.Length);
            //    }
            //}
            //var count = dict.Count;
            //Result.Text = $"Число слов {count}";
            //Result.Dispatcher.Invoke(() => Result.Text = $"Число слов {count}");

            StartAction.IsEnabled = false;
            CancelAction.IsEnabled = true;

            _ReadingFileCancellation = new CancellationTokenSource();

            var cancel = _ReadingFileCancellation.Token;
            IProgress<double> progress = new Progress<double>(p => ProgressInfo.Value = p);

            try
            {
                var count = await GetWordsCountAsync(dialog.FileName, progress, cancel);
                Result.Text = $"Число слов {count}";
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Операция чтения файла была отменена");
                Result.Text = $"---";
                progress.Report(0);
            }

            CancelAction.IsEnabled = false;
            StartAction.IsEnabled = true;
        }

        private static async Task<int> GetWordsCountAsync(string FileName, IProgress<double> progress = null, CancellationToken cansel = default)
        {
            // Мы находимся в ThreadId == 7 (к примеру)
            await Task.Yield();
            // Теперь мы в ThreadId == 12 (например) (а возможно и обратно в 7)

            var dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            cansel.ThrowIfCancellationRequested();
            using (var reader = new StreamReader(FileName))
            {
                while (!reader.EndOfStream)
                {
                    cansel.ThrowIfCancellationRequested();
                    var line = await reader.ReadLineAsync().ConfigureAwait(false);
                    // ConfigureAwait(false); - требование "вернутся" в произвольны поток из пула потоков.
                    var words = line.Split(' ');

                    //Thread.Sleep(100);
                    await Task.Delay(1);

                    foreach (var word in words)
                        if (dict.ContainsKey(word))
                            dict[word]++;
                        else
                            dict.Add(word, 1);

                    progress?.Report(reader.BaseStream.Position / (double)reader.BaseStream.Length);
                }
            }

            return dict.Count;
        }

        private CancellationTokenSource _ReadingFileCancellation;

        private void OnCancelReadingClick(object sender, RoutedEventArgs e)
        {
            _ReadingFileCancellation?.Cancel();
        }
    }
}
