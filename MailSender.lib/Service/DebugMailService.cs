using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MailSender.lib.Interface;

namespace MailSender.lib.Service
{
    public class DebugMailService : IMailService
    {
        private readonly IEncryptorService _encryptorService;

        public DebugMailService(IEncryptorService encryptorService)
        {
            _encryptorService = encryptorService;
        }
        public IMailSender GetSender(string Server, int Port, bool SSL, string Login, string Password)
        {
            return new DebugMailSender(Server, Port, SSL, Login, Password);
        }
        private class DebugMailSender : IMailSender
        {
            private readonly string _address;
            private readonly int _port;
            private readonly bool _ssl;
            private readonly string _login;
            private readonly string _password;

            public DebugMailSender(string address, int port, bool ssl, string login, string password)
            {
                _address = address;
                _port = port;
                _ssl = ssl;
                _login = login;
                _password = password;
            }

            public void Send(string SenderAddress, string RecipientAddress, string Subject, string Body)
            {
                Debug.WriteLine($"Отправка почты через сервер " +
                                $"{_address}:{_port} SSL:{_ssl} (Login:{_login}; Pass:{_password})");
                Debug.WriteLine($"Сообщение от {SenderAddress} к {RecipientAddress}:\r\n{Subject}\r\n{Body}");
            }

            public void Send(string SenderAddress, IEnumerable<string> RecipientsAddresses, string Subject, string Body)
            {
                foreach (var recipient_address in RecipientsAddresses)
                {
                    Send(SenderAddress, recipient_address, Subject, Body);
                }
            }

            public void SendParallel(string SenderAddress, IEnumerable<string> RecipientsAddresses, string Subject, string Body)
            {
                Send(SenderAddress, RecipientsAddresses, Subject, Body);
            }
        }
    }
}
