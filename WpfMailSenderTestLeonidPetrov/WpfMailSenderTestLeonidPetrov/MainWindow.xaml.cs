using System.Threading;
using System.Windows;

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

        private void ComputeResultButton_Click(object sender, RoutedEventArgs e)
        {
            // здесь в один поток
            //var result = GetResultHard();
            //ResultText.Text = result;

            new Thread(() =>
            {
                // так нельзя, потому что Интерфейс - это критическая секция и будет ругаться, что не из потока интерфейса.
                //ResultText.Text = GetResultHard();

                // либо
                //Dispatcher
                //ResultText.Dispatcher
                //App.Current.Dispatcher

                var result = GetResultHard();
                UpdateResultValue(result);
            }){ IsBackground = true}.Start();
        }

        public void UpdateResultValue(string Result)
        {
            if (Dispatcher.CheckAccess())
                ResultText.Text = Result;
            else
                Dispatcher.Invoke(() => UpdateResultValue(Result));
        }

        private string GetResultHard()
        {
            for (int i = 0; i < 500; i++)
            {
                Thread.Sleep(10);
            }

            return "Hello!";
        }
    }
}
