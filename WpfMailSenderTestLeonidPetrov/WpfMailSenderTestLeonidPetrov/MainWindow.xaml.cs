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
                Application.Current.Dispatcher.Invoke(() => ResultText.Text = result);


            }){ IsBackground = true}.Start();
        }

        private string GetResultHard()
        {
            for (int i = 0; i < 1000; i++)
            {
                Thread.Sleep(10);
            }

            return "Hello!";
        }
    }
}
