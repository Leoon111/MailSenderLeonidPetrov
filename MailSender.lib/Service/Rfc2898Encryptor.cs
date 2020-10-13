using System;
using System.IO;
using MailSender.lib.Interface;
using System.Security.Cryptography;
using System.Text;

namespace MailSender.lib.Service
{
    public class Rfc2898Encryptor : IEncryptorService
    {
        #region Implementation of IEncryptorService

        /// <summary> 
        /// Массив сайт . - "соль" ' алгоритмы . шифрования ГГГс2898 
        /// </summary>    
        private static readonly byte[] SALT =
        {
            0x26, 0xdc, 0xff, 0x00,
            0xad, 0xed, 0x7a, 0xee,
            0xc5, 0xfe, 0x07, 0xaf, 
            0x4d, 0x08, 0x22, 0x3c
        };

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>Получить алгоритм шифрования с указанным паролем</summary>
        /// <param name="password">Пароль шифрования</param>
        /// <returns>Алгоритм шифрования</returns>
        private static ICryptoTransform GetAlgorithm(string password)
        {
            var pdb = new Rfc2898DeriveBytes(password, SALT);
            var algorithm = Rijndael.Create();
            algorithm.Key = pdb.GetBytes(32);
            algorithm.IV = pdb.GetBytes(16);
            return algorithm.CreateEncryptor();
        }
        /// <summary>Получить алгоритм для расшифровки</summary>
        /// <param name="password">Пароль</param>
        /// <returns>Алгоритм расшифровки</returns>
        private static ICryptoTransform GetInverseAlgorithm(string password)
        {
            var pdb = new Rfc2898DeriveBytes(password, SALT);
            var algorithm = Rijndael.Create();
            algorithm.Key = pdb.GetBytes(32);
            algorithm.IV = pdb.GetBytes(16);
            return algorithm.CreateDecryptor();
        }


        public string Encrypt(string str, string Password)
        {
            var encoding = Encoding ?? Encoding.UTF8;
            var bytes = encoding.GetBytes(str);
            var crypted_bytes = Encrypt(bytes, Password);
            return Convert.ToBase64String(crypted_bytes);
        }

        public byte[] Encrypt(byte[] data, string Password)
        {
            var algoritm = GetAlgorithm(Password);
            using (var stream = new MemoryStream())
            using (var crypted_stream = new CryptoStream(stream, algoritm, CryptoStreamMode.Write))
            {
                crypted_stream.Write(data, 0, data.Length);
                crypted_stream.FlushFinalBlock();
                return stream.ToArray();
            }
        }

        public string Decrypt(string str, string Password)
        {
            var crypted_bytes = Convert.FromBase64String(str);
            var bytes = Decrypt(crypted_bytes, Password);
            var encoding = Encoding ?? Encoding.UTF8;
            return encoding.GetString(bytes);
        }

        public byte[] Decrypt(byte[] data, string Password)
        {
            var algoritm = GetInverseAlgorithm(Password);
            using (var stream = new MemoryStream())
            using (var crypted_stream = new CryptoStream(stream, algoritm, CryptoStreamMode.Write))
            {
                crypted_stream.Write(data, 0, data.Length);
                crypted_stream.FlushFinalBlock();
                return stream.ToArray();
            }
        }

        #endregion
    }
}
