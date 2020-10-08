using System;
using System.Net.Mail;
using System.Windows;
using MailSender.lib;
using MailSenderLeonidPetrov.Data;
using MailSenderLeonidPetrov.Models;

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

        private void OnSendButtonClick(object sender, RoutedEventArgs e)
        {
            var mySender = SendersList.SelectedItem as Sender;
            if(mySender is null) return;

            if(!(RecipientsList.SelectedItem is Recipient recipient)) return;
            if (!(SendersList.SelectedItem is Server server)) return;
            if (!(MessegesList.SelectedItems is Message message)) return;

            var send_service = new MailSenderService()
            {
                ServerAddress = server.Address,
                ServerPort = server.Port,
                UseSSL = server.UseSSL,
                Login = server.Login,
                Passvord = server.Password
            };

            try
            {
                send_service.SendMessage(mySender.Address, recipient.Address, message.Subject, message.Body);
            }
            catch (SmtpException error)
            {
                MessageBox.Show($"Ошибка при отправке почты {error.Message}", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
