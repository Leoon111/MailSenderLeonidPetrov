using System;
using System.Collections.Generic;
using System.Text;

namespace MailSender.lib.Interface
{
    public interface IEncryptorService
    {
        string Encrypt(string str, string Password);

        string Decrypt(string str, string Password);

        byte[] Encrypt(byte[] data, string Password);

        byte[] Decrypt(byte[] data, string Password);

    }
}
