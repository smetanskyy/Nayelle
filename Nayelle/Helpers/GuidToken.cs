using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Nayelle.Helpers
{
    public class GuidToken
    {
        public static string CreateTokenBasedOnGuid(string guid, int expiredHoursAfterCreating = 24)
        {
            if (string.IsNullOrWhiteSpace(guid))
            {
                return null;
            }
            var guidParse = Guid.Parse(guid);
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.AddHours(expiredHoursAfterCreating).ToBinary());
            byte[] key = guidParse.ToByteArray();
            var data = time.Concat(key).ToArray();
            var tokenInBytes = Encrypt(data);
            return WebEncoders.Base64UrlEncode(tokenInBytes);
        }

        public static bool IsValidTokenBasedOnTime(string token)
        {
            try
            {
                byte[] data = WebEncoders.Base64UrlDecode(token);
                data = Decrypt(data);
                DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
                if (when < DateTime.UtcNow)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GetGuidFromToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            try
            {
                byte[] data = WebEncoders.Base64UrlDecode(token);
                data = Decrypt(data);
                byte[] time = data.Take(8).ToArray();
                byte[] key = data.Skip(8).Take(16).ToArray();
                return new Guid(key).ToString("D");
            }
            catch (Exception)
            {
                return null;
            }
        }

        // password length should be 16 characters
        private const string passKey = "campussupportkey";
        private const string passIv = "campussupport_iv";

        private static byte[] Encrypt(byte[] data)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.BlockSize = 128;
                aes.Key = Encoding.ASCII.GetBytes(passKey);
                aes.IV = Encoding.ASCII.GetBytes(passIv);

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    return PerformCryptography(data, encryptor);
                }
            }
        }

        private static byte[] Decrypt(byte[] data)
        {
            byte[] time = data.Take(8).ToArray();
            byte[] key = data.Skip(8).ToArray();

            using (var aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.Zeros;

                aes.Key = Encoding.ASCII.GetBytes(passKey);
                aes.IV = Encoding.ASCII.GetBytes(passIv);

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    return PerformCryptography(data, decryptor);
                }
            }
        }

        private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (var ms = new MemoryStream())
            using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                return ms.ToArray();
            }
        }
    }
}