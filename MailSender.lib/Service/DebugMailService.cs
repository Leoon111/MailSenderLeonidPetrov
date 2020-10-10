using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MailSender.lib.Interface;

namespace MailSender.lib.Service
{
    public class DebugMailService : IMailService
    {
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
        }
    }
}
