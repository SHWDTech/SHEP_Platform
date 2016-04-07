using System;
using System.Security.Cryptography;
using System.Text;

namespace SHEP_Platform.Process
{
    public class Utility
    {
        #region 大小端转换

        /// <summary>
        /// UINT64 转 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <param name="isLittleEndian"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static void Uint64ToBytes(ulong value, byte[] buffer, int offset, bool isLittleEndian)
        {
            var bufferIndex = offset;
            
            if (isLittleEndian)
            {
                buffer[bufferIndex] = (byte)(value & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 8) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 16) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 24) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 32) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 40) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 48) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 56) & 0xFF);
            }
            else
            {
                buffer[bufferIndex] = (byte)((value >> 56) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 48) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 40) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 32) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 24) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 16) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 8) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)(value & 0xFF);
            }
        }

        /// <summary>
        /// INT64 转 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <param name="isLittleEndian"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static void Int64ToBytes(long value, byte[] buffer, int offset, bool isLittleEndian)
        {
            var bufferIndex = offset;
            
            if (isLittleEndian)
            {            
                buffer[bufferIndex] = (byte)(value & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 8) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 16) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 24) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 32) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 40) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 48) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 56) & 0xFF);
            }
            else
            {
                buffer[bufferIndex] = (byte)((value >> 56) & 0xFF);
                bufferIndex++;
                
                buffer[bufferIndex] = (byte)((value >> 48) & 0xFF);
                bufferIndex++;
                
                buffer[bufferIndex] = (byte)((value >> 40) & 0xFF);
                bufferIndex++;
                
                buffer[bufferIndex] = (byte)((value >> 32) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 24) & 0xFF);
                bufferIndex++;
                
                buffer[bufferIndex] = (byte)((value >> 16) & 0xFF);
                bufferIndex++;
                
                buffer[bufferIndex] = (byte)((value >> 8) & 0xFF);
                bufferIndex++;
                
                buffer[bufferIndex] = (byte)(value & 0xFF);
            }
        }

        /// <summary>
        /// UINT32 转 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <param name="isLittleEndian"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static void Uint32ToBytes(uint value, byte[] buffer, int offset, bool isLittleEndian)
        {
            var bufferIndex = offset;
            
            if (isLittleEndian)
            {
                buffer[bufferIndex] = (byte)(value & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 8) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 16) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 24) & 0xFF);
            }
            else
            {
                buffer[bufferIndex] = (byte)((value >> 24) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 16) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 8) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)(value & 0xFF);
            }
        }

        /// <summary>
        /// INT32 转 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <param name="isLittleEndian"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static void Int32ToBytes(int value, byte[] buffer, int offset, bool isLittleEndian)
        {
            var bufferIndex = offset;
            
            if (isLittleEndian)
            {
                buffer[bufferIndex] = (byte)(value & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 8) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 16) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 24) & 0xFF);
            }
            else
            {
                buffer[bufferIndex] = (byte)((value >> 24) & 0xFF);
                bufferIndex++;
                
                buffer[bufferIndex] = (byte)((value >> 16) & 0xFF);
                bufferIndex++;
                
                buffer[bufferIndex] = (byte)((value >> 8) & 0xFF);
                bufferIndex++;
                
                buffer[bufferIndex] = (byte)(value & 0xFF);
            }
        }


        /// <summary>
        /// UINT16 转 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <param name="isLittleEndian"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static void Uint16ToBytes(ushort value, byte[] buffer, int offset, bool isLittleEndian)
        {
            var bufferIndex = offset;
            
            if (isLittleEndian)
            {
                buffer[bufferIndex] = (byte)(value & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 8) & 0xFF);
            }
            else
            {
                buffer[bufferIndex] = (byte)((value >> 8) & 0xFF);
                bufferIndex++;
                
                buffer[bufferIndex] = (byte)(value & 0xFF);
            }
        }

        /// <summary>
        /// INT32 转 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <param name="isLittleEndian"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static void Int16ToBytes(short value, byte[] buffer, int offset, bool isLittleEndian)
        {
            var bufferIndex = offset;
            
            if (isLittleEndian)
            {
                buffer[bufferIndex] = (byte)(value & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 8) & 0xFF);
            }
            else
            {
                buffer[bufferIndex] = (byte)((value >> 8) & 0xFF);
                bufferIndex++;
                buffer[bufferIndex] = (byte)(value & 0xFF);
            }
        }
        
        /// <summary>
        /// UINT64 转 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isLittleEndian"></param>
        /// <returns></returns>
        public static byte[] Uint64ToBytes(ulong value, bool isLittleEndian)
        {
            var buffer = new byte[8];

            if (isLittleEndian)
            {
                buffer[7] = (byte)((value >> 56) & 0xFF);
                buffer[6] = (byte)((value >> 48) & 0xFF);
                buffer[5] = (byte)((value >> 40) & 0xFF);
                buffer[4] = (byte)((value >> 32) & 0xFF);

                buffer[3] = (byte)((value >> 24) & 0xFF);
                buffer[2] = (byte)((value >> 16) & 0xFF);
                buffer[1] = (byte)((value >> 8) & 0xFF);
                buffer[0] = (byte)(value & 0xFF);
            }
            else
            {
                buffer[0] = (byte)((value >> 56) & 0xFF);
                buffer[1] = (byte)((value >> 48) & 0xFF);
                buffer[2] = (byte)((value >> 40) & 0xFF);
                buffer[3] = (byte)((value >> 32) & 0xFF);

                buffer[4] = (byte)((value >> 24) & 0xFF);
                buffer[5] = (byte)((value >> 16) & 0xFF);
                buffer[6] = (byte)((value >> 8) & 0xFF);
                buffer[7] = (byte)(value & 0xFF);
            }

            return buffer;
        }

        /// <summary>
        /// INT64 转 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isLittleEndian"></param>
        /// <returns></returns>
        public static byte[] Int64ToBytes(long value, bool isLittleEndian)
        {
            var buffer = new byte[8];

            if (isLittleEndian)
            {
                buffer[7] = (byte)((value >> 56) & 0xFF);
                buffer[6] = (byte)((value >> 48) & 0xFF);
                buffer[5] = (byte)((value >> 40) & 0xFF);
                buffer[4] = (byte)((value >> 32) & 0xFF);

                buffer[3] = (byte)((value >> 24) & 0xFF);
                buffer[2] = (byte)((value >> 16) & 0xFF);
                buffer[1] = (byte)((value >> 8) & 0xFF);
                buffer[0] = (byte)(value & 0xFF);
            }
            else
            {
                buffer[0] = (byte)((value >> 56) & 0xFF);
                buffer[1] = (byte)((value >> 48) & 0xFF);
                buffer[2] = (byte)((value >> 40) & 0xFF);
                buffer[3] = (byte)((value >> 32) & 0xFF);

                buffer[4] = (byte)((value >> 24) & 0xFF);
                buffer[5] = (byte)((value >> 16) & 0xFF);
                buffer[6] = (byte)((value >> 8) & 0xFF);
                buffer[7] = (byte)(value & 0xFF);
            }

            return buffer;
        }

        /// <summary>
        /// UINT32 转 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isLittleEndian"></param>
        /// <returns></returns>
        public static byte[] Uint32ToBytes(uint value, bool isLittleEndian)
        {
            var buffer = new byte[4];

            if (isLittleEndian)
            {
                buffer[3] = (byte)((value >> 24) & 0xFF);
                buffer[2] = (byte)((value >> 16) & 0xFF);
                buffer[1] = (byte)((value >> 8) & 0xFF);
                buffer[0] = (byte)(value & 0xFF);
            }
            else
            {
                buffer[0] = (byte)((value >> 24) & 0xFF);
                buffer[1] = (byte)((value >> 16) & 0xFF);
                buffer[2] = (byte)((value >> 8) & 0xFF);
                buffer[3] = (byte)(value & 0xFF);
            }

            return buffer;
        }

        /// <summary>
        /// INT32 转 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isLittleEndian"></param>
        /// <returns></returns>
        public static byte[] Int32ToBytes(int value, bool isLittleEndian)
        {
            var buffer = new byte[4];

            if (isLittleEndian)
            {
                buffer[3] = (byte)((value >> 24) & 0xFF);
                buffer[2] = (byte)((value >> 16) & 0xFF);
                buffer[1] = (byte)((value >> 8) & 0xFF);
                buffer[0] = (byte)(value & 0xFF);
            }
            else
            {
                buffer[0] = (byte)((value >> 24) & 0xFF);
                buffer[1] = (byte)((value >> 16) & 0xFF);
                buffer[2] = (byte)((value >> 8) & 0xFF);
                buffer[3] = (byte)(value & 0xFF);
            }

            return buffer;
        }


        /// <summary>
        /// UINT16 转 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isLittleEndian"></param>
        /// <returns></returns>
        public static byte[] Uint16ToBytes(ushort value, bool isLittleEndian)
        {
            var buffer = new byte[2];

            if (isLittleEndian)
            {
                buffer[1] = (byte)((value >> 8) & 0xFF);
                buffer[0] = (byte)(value & 0xFF);
            }
            else
            {
                buffer[0] = (byte)((value >> 8) & 0xFF);
                buffer[1] = (byte)(value & 0xFF);
            }

            return buffer;
        }

        /// <summary>
        /// INT32 转 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isLittleEndian"></param>
        /// <returns></returns>
        public static byte[] Int16ToBytes(int value, bool isLittleEndian)
        {
            var buffer = new byte[2];

            if (isLittleEndian)
            {
                buffer[1] = (byte)((value >> 8) & 0xFF);
                buffer[0] = (byte)(value & 0xFF);
            }
            else
            {
                buffer[0] = (byte)((value >> 8) & 0xFF);
                buffer[1] = (byte)(value & 0xFF);
            }

            return buffer;
        }


        /// <summary>
        ///  bytes转UINT64
        /// </summary>
        /// <param name="bufferIndex"></param>
        /// <param name="isLittleEndian"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static ulong BytesToUint64(byte[] buffer, int bufferIndex, bool isLittleEndian)
        {
            ulong val;

            if (isLittleEndian)
            {
                val = ((ulong)((buffer[bufferIndex + 7] << 56) + (buffer[bufferIndex + 6] << 48)
                    + (buffer[bufferIndex + 5] << 40) + (buffer[bufferIndex + 4] << 32)
                    + (buffer[bufferIndex + 3] << 24) + (buffer[bufferIndex + 2] << 16)
                    + (buffer[bufferIndex + 1] << 8) + buffer[bufferIndex])) & 0xFFFFFFFFFFFFFFFF;
            }
            else
            {
                val = ((ulong)((buffer[bufferIndex] << 56) + (buffer[bufferIndex + 1] << 48)
                    + (buffer[bufferIndex + 2] << 40) + (buffer[bufferIndex + 3] << 32)
                    + (buffer[bufferIndex + 4] << 24) + (buffer[bufferIndex + 5] << 16)
                    + (buffer[bufferIndex + 6] << 8) + buffer[bufferIndex + 7])) & 0xFFFFFFFFFFFFFFFF;
            }

            return val;
        }

        /// <summary>
        ///  bytes转INT64
        /// </summary>
        /// <param name="bufferIndex"></param>
        /// <param name="isLittleEndian"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static long BytesToInt64(byte[] buffer, int bufferIndex, bool isLittleEndian)
        {
            long val;

            if (isLittleEndian)
            {
                val = (buffer[bufferIndex + 7] << 56) + (buffer[bufferIndex + 6] << 48)
                      + (buffer[bufferIndex + 5] << 40) + (buffer[bufferIndex + 4] << 32)
                      + (buffer[bufferIndex + 3] << 24) + (buffer[bufferIndex + 2] << 16)
                      + (buffer[bufferIndex + 1] << 8) + buffer[bufferIndex] & 0x7FFFFFFFFFFFFFFF;
            }
            else
            {
                val = (buffer[bufferIndex] << 56) + (buffer[bufferIndex + 1] << 48)
                      + (buffer[bufferIndex + 2] << 40) + (buffer[bufferIndex + 3] << 32)
                      + (buffer[bufferIndex + 4] << 24) + (buffer[bufferIndex + 5] << 16)
                      + (buffer[bufferIndex + 6] << 8) + buffer[bufferIndex + 7] & 0x7FFFFFFFFFFFFFFF;
            }

            return val;
        }

        /// <summary>
        ///  bytes转UINT32
        /// </summary>
        /// <param name="bufferIndex"></param>
        /// <param name="isLittleEndian"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static uint BytesToUint32(byte[] buffer, int bufferIndex, bool isLittleEndian)
        {
            uint val;

            if (isLittleEndian)
            {
                val = ((uint)((buffer[bufferIndex + 3] << 24) + (buffer[bufferIndex + 2] << 16)
                    + (buffer[bufferIndex + 1] << 8) + buffer[bufferIndex])) & 0xFFFFFFFF;
            }
            else
            {
                val = ((uint)((buffer[bufferIndex] << 24) + (buffer[bufferIndex + 1] << 16)
                    + (buffer[bufferIndex + 2] << 8) + buffer[bufferIndex + 3])) & 0xFFFFFFFF;
            }

            return val;
        }

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

        /// <summary>
        ///  bytes转UINT16
        /// </summary>
        /// <param name="bufferIndex"></param>
        /// <param name="isLittleEndian"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static ushort BytesToUint16(byte[] buffer, int bufferIndex, bool isLittleEndian)
        {
            ushort val;

            if (isLittleEndian)
            {
                val = (ushort)(((buffer[bufferIndex + 1] << 8) + buffer[bufferIndex]) & 0xFFFF);
            }
            else
            {
                val = (ushort)(((buffer[bufferIndex] << 8) + buffer[bufferIndex + 1]) & 0xFFFF);
            }

            return val;
        }

        /// <summary>
        ///  bytes转INT16
        /// </summary>
        /// <param name="bufferIndex"></param>
        /// <param name="isLittleEndian"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static short BytesToInt16(byte[] buffer, int bufferIndex, bool isLittleEndian)
        {
            short val;

            if (isLittleEndian)
            {
                val = (short)(((buffer[bufferIndex + 1] << 8) + buffer[bufferIndex]) & 0x7FFF);
            }
            else
            {
                val = (short)(((buffer[bufferIndex] << 8) + buffer[bufferIndex + 1]) & 0x7FFF);
            }

            return val;
        }
        #endregion
        
        #region 转换
        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] StringToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            var returnBytes = new byte[hexString.Length / 2];
            for (var i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            
            return returnBytes;
        }

        /// <summary>
        /// Gets the unix time stamp.
        /// </summary>
        /// <returns></returns>
        public static string GetUnixTimeStamp()
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return $"{Convert.ToInt64((DateTime.Now.AddHours(-8) - start).TotalMilliseconds) / 1000}";
        }

        /// <summary>
        /// Gets the MD5.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string GetMd5(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }

            var md5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(str))).ToLower().Replace("-", "");
        }
        #endregion
    }
}
