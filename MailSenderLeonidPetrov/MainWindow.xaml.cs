using System.Windows;
using MailSenderLeonidPetrov.Data;

namespace MailSenderLeonidPetrov
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //ServersList.ItemsSource = TestData.Senders;
        }
    }
}
