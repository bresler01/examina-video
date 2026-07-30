using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace proj
{
    public static class Encriptar
    {
        private static readonly string Key = "sua-chave-secreta-32-caracteres!";

        public static string Encrypt(string plainText)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.GenerateIV();
                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    byte[] combined = new byte[aes.IV.Length + cipherBytes.Length];
                    Array.Copy(aes.IV, 0, combined, 0, aes.IV.Length);
                    Array.Copy(cipherBytes, 0, combined, aes.IV.Length, cipherBytes.Length);
                    return Convert.ToBase64String(combined);
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
            byte[] combined = Convert.FromBase64String(cipherText);
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                byte[] iv = new byte[aes.BlockSize / 8];
                Array.Copy(combined, 0, iv, 0, iv.Length);
                aes.IV = iv;
                using (var decryptor = aes.CreateDecryptor())
                {
                    byte[] cipherBytes = new byte[combined.Length - iv.Length];
                    Array.Copy(combined, iv.Length, cipherBytes, 0, cipherBytes.Length);
                    byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                    return Encoding.UTF8.GetString(plainBytes);
                }
            }
        }

    }

}
