using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SHEP_Platform.Common
{
    public static class Global
    {
        public static string GetMd5(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }

            var md5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(str))).ToLower().Replace("-", "");
        }

        public static string GetUserStatus(byte? statusCode)
        {
            var status = string.Empty;
            if (statusCode == 1)
            {
                status = "锁定";
            }

            if (statusCode == 2)
            {
                status = "审核通过";
            }

            return status;
        }

        public static byte[] StringToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if (hexString.Length % 2 != 0)
                hexString += " ";
            byte[] numArray = new byte[hexString.Length / 2];
            for (int index = 0; index < numArray.Length; ++index)
                numArray[index] = Convert.ToByte(hexString.Substring(index * 2, 2), 16);
            return numArray;
        }

        public static byte[] StringToBytes(string str)
        {
            var hexStr = StringToHexString(str);
            if (hexStr.Length > 8)
            {
                hexStr = hexStr.Substring(hexStr.Length - 8, 8);
            }
            else if (hexStr.Length < 8)
            {
                hexStr = hexStr.PadLeft(8, '0');
            }

            return StringToHexByte(hexStr);
        }

        public static string StringToHexString(string str)
        {
            var num = int.Parse(str);
            var bytes = BitConverter.GetBytes(num);
            Array.Reverse(bytes);
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("X2"));
            }

            return builder.ToString();
        }

        public static bool IsMobileDevice(string userAgent)
        {
            var pattern = "iPhone|iPod|Android|ios|iPad|UCWEB|Windows Phone OS";
            return Regex.IsMatch(userAgent, pattern);
        }
    }
}