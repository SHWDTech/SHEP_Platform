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
            return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(str))).ToLower().Replace("-","");
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

        public static byte[] StringTohexStringByte(string str)
        {
            var num = int.Parse(str);
            var hexStr = num.ToString("x8");

            return StringToHexByte(hexStr);
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


        /// <summary>
        /// 将输入的Byte数组转换为十六进制显示的字符串
        /// </summary>
        /// <param name="data">需要转换的Byte数组</param>
        /// <param name="addPad">是否要添加空字符</param>
        /// <returns>data的字符串表示形式</returns>
        public static string ByteArrayToHexString(byte[] data, bool addPad = true)
        {
            var sb = new StringBuilder(data.Length * 3);
            foreach (var b in data)
            {
                var Char = Convert.ToString(b, 16).PadLeft(2, '0');
                if (addPad)
                {
                    Char = Char.PadRight(3, ' ');
                }
                sb.Append(Char);
            }
            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// 将输入的Byte数组转换为UTF8字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ByteArrayToUtf8String(byte[] data) => Encoding.UTF8.GetString(data);

        /// <summary>
        ///  bytes转INT32
        /// </summary>
        /// <param name="bufferIndex"></param>
        /// <param name="isLittleEndian"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static int BytesToInt32(byte[] buffer, int bufferIndex, bool isLittleEndian)
        {
            int val;

            if (isLittleEndian)
            {
                val = (buffer[bufferIndex + 3] << 24) + (buffer[bufferIndex + 2] << 16)
                      + (buffer[bufferIndex + 1] << 8) + buffer[bufferIndex] & 0x7FFFFFFF;
            }
            else
            {
                val = (buffer[bufferIndex] << 24) + (buffer[bufferIndex + 1] << 16)
                      + (buffer[bufferIndex + 2] << 8) + buffer[bufferIndex + 3] & 0x7FFFFFFF;
            }

            return val;
        }

        public static bool IsMobileDevice(string userAgent)
        {
            var pattern = "iPhone|iPod|Android|ios|iPad|UCWEB|Windows Phone OS";
            return Regex.IsMatch(userAgent, pattern);
        }
    }
}