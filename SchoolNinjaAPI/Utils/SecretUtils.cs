using System;
using System.Security.Cryptography;
using System.Text;

namespace SchoolNinjaAPI.Utils
{
    public class SecretUtils
    {
        public static string GenerateSalt()
        {
            const int length = 10;
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var randomNumber = new Random();
            var chars = new char[length];
            for (var i = 0; i <= length - 1; i++)
            {
                chars[i] = allowedChars[Convert.ToInt32((allowedChars.Length) * randomNumber.NextDouble())];
            }
            return new string(chars);
        }

        public static string EncodePassword(string password, string salt)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] source = Encoding.Unicode.GetBytes(salt);
            byte[] destination = new byte[source.Length + bytes.Length];
            Buffer.BlockCopy(source, 0, destination, 0, source.Length);
            Buffer.BlockCopy(bytes, 0, destination, source.Length, bytes.Length);
            HashAlgorithm hashAlgorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = hashAlgorithm.ComputeHash(destination);
            return EncodePasswordMd5(Convert.ToBase64String(inArray));
        }

        private static string EncodePasswordMd5(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(password);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);
            return BitConverter.ToString(encodedBytes);
        }
    }
}
