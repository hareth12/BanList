﻿namespace TwoFactorAuthentication.API.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Cryptography;

    public static class TimeSensitivePassCode
    {
        public static string GeneratePresharedKey()
        {
            var key = new byte[10]; // 80 bits
            using (var rngProvider = new RNGCryptoServiceProvider())
            {
                rngProvider.GetBytes(key);
            }

            return key.ToBase32String();
        }

        public static IEnumerable<string> GetListOfOtp(string base32EncodedSecret)
        {
            var epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);

            var counter = (long) Math.Floor((DateTime.UtcNow - epochStart).TotalSeconds/30);
            var otps = new List<string>
            {
                GetHotp(base32EncodedSecret, counter - 1),
                GetHotp(base32EncodedSecret, counter),
                GetHotp(base32EncodedSecret, counter + 1)
            };

            return otps;
        }

        private static string GetHotp(string base32EncodedSecret, long counter)
        {
            byte[] message = BitConverter.GetBytes(counter).Reverse().ToArray(); //Intel machine (little endian)
            byte[] secret = base32EncodedSecret.ToByteArray();

            var hmac = new HMACSHA1(secret, true);

            byte[] hash = hmac.ComputeHash(message);
            int offset = hash[hash.Length - 1] & 0xf;
            int truncatedHash = ((hash[offset] & 0x7f) << 24) | ((hash[offset + 1] & 0xff) << 16)
                                | ((hash[offset + 2] & 0xff) << 8) | (hash[offset + 3] & 0xff);

            int hotp = truncatedHash%1000000;
            return hotp.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0');
        }
    }

    public static class StringHelper
    {
        private const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        public static string ToBase32String(this IEnumerable<byte> secret)
        {
            string bits = secret.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')).Aggregate((a, b) => a + b);

            return
                Enumerable.Range(0, bits.Length/5)
                    .Select(i => alphabet.Substring(Convert.ToInt32(bits.Substring(i*5, 5), 2), 1))
                    .Aggregate((a, b) => a + b);
        }

        public static byte[] ToByteArray(this string secret)
        {
            string bits =
                secret.ToUpper()
                    .ToCharArray()
                    .Select(c => Convert.ToString(alphabet.IndexOf(c), 2).PadLeft(5, '0'))
                    .Aggregate((a, b) => a + b);

            return
                Enumerable.Range(0, bits.Length/8).Select(i => Convert.ToByte(bits.Substring(i*8, 8), 2)).ToArray();
        }
    }
}