using System;
using System.Net;
using System.Net.Mail;

namespace MailSenderTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var to = new MailAddress("Leoon111@gmail.com");
            var from = new MailAddress("Leoon123@yandex.ru");

            var message = new MailMessage(from, to);

            message.Subject = "Заголовок письма + " + DateTime.Now;
            message.Body = "Тело письма " + DateTime.Now;

            var client = new SmtpClient("smtp.yandex.ru", 25);

            client.Credentials = new NetworkCredential
            {
                UserName = "user@name.com",
                Password = "password"
            };
            client.Send(message);
        }
    }
}
