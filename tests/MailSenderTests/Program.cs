using MailSender.lib.Interface;
using System;
using System.Net;
using System.Net.Mail;
using MailSender.lib.Service;

namespace MailSenderTests
{
    class Program
    {
        static void Main(string[] args)
        {

                IEncryptorService cryptor = new Rfc2898Encryptor();

                var str = "Hello World!";
                const string password = "MailSender!";

                var crypted_str = cryptor.Encrypt(str, password);

                var decrypted_str = cryptor.Decrypt(crypted_str, password);



            //var to = new MailAddress("Leoon111@gmail.com");
            //var from = new MailAddress("Leoon123@yandex.ru");

            //var message = new MailMessage(from, to);

            //message.Subject = "Заголовок письма + " + DateTime.Now;
            //message.Body = "Тело письма " + DateTime.Now;

            //var client = new SmtpClient("smtp.yandex.ru", 25);

            //client.Credentials = new NetworkCredential
            //{
            //    UserName = "user@name.com",
            //    Password = "password"
            //};
            //client.Send(message);
        }
    }
}
