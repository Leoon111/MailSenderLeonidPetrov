using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using MailSender.lib.Interface;

namespace MailSender.lib.Service
{
    public class SmtpMailService : IMailService
    {
        public IMailSender GetSender(string Server, int Port, bool SSL, string Login, string Password)
        {
            return new SmtpMailSender(Server, Port, SSL, Login, Password);
        }
    }

    internal class SmtpMailSender : IMailSender
    {
        private readonly string _address;
        private readonly int _port;
        private readonly bool _ssl;
        private readonly string _login;
        private readonly string _password;

        public SmtpMailSender(string Address, int Port, bool SSL, string Login, string Password)
        {
            _address = Address;
            _port = Port;
            _ssl = SSL;
            _login = Login;
            _password = Password;
        }

        public void Send(string SenderAddress, string RecipientAddress, string Subject, string Body)
        {
            var from = new MailAddress(SenderAddress);
            var to = new MailAddress(RecipientAddress);
            using (var message = new MailMessage(from, to))
            {
                message.Subject = Subject;
                message.Body = Body;
                using (var client = new SmtpClient(_address, _port))
                {
                    client.EnableSsl = _ssl;
                    client.Credentials = new NetworkCredential
                    {
                        UserName = _login,
                        Password = _password
                    };
                    try
                    {
                        client.Send(message);
                    }
                    catch (SmtpException e)
                    {
                        Trace.TraceError(e.ToString());
                        throw;
                    }
                }
            }
        }
    }
}
