using System;
using System.Collections.Generic;
using System.Web;

namespace Web.Common
{
    public class Utility
    {
        #region MD5加密
        /// <summary>
        /// MD5加密算法
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="code">加密方式,16或32</param>
        /// <returns></returns>
        public static string MD5(string str, int code)
        {
            if (code == 16) //16位MD5加密（取32位加密的9~25字符） 
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower().Substring(8, 16);
            }
            else//32位加密 
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
            }
        }
        #endregion
        
        #region 大小端转换
        /// <summary>
        /// UINT64 转 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="IsLittleEndian"></param>
        /// <returns></returns>
        public static void UINT64ToBytes(UInt64 value, byte[] buffer, int offset, bool IsLittleEndian)
        {
            int bufferIndex = offset;
            
            if (IsLittleEndian)
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
                bufferIndex++;
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
                bufferIndex++;
            }

            return;
        }

        /// <summary>
        /// INT64 转 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="IsLittleEndian"></param>
        /// <returns></returns>
        public static void INT64ToBytes(Int64 value, byte[] buffer, int offset, bool IsLittleEndian)
        {
            int bufferIndex = offset;
            
            if (IsLittleEndian)
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
                bufferIndex++;
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
                bufferIndex++;
            }

            return;
        }

        /// <summary>
        /// UINT32 转 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="IsLittleEndian"></param>
        /// <returns></returns>
        public static void UINT32ToBytes(UInt32 value, byte[] buffer, int offset, bool IsLittleEndian)
        {
            int bufferIndex = offset;
            
            if (IsLittleEndian)
            {
                buffer[bufferIndex] = (byte)(value & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 8) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 16) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 24) & 0xFF);
                bufferIndex++;
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
                bufferIndex++;
            }

            return;
        }

        /// <summary>
        /// INT32 转 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="IsLittleEndian"></param>
        /// <returns></returns>
        public static void INT32ToBytes(Int32 value, byte[] buffer, int offset, bool IsLittleEndian)
        {
            int bufferIndex = offset;
            
            if (IsLittleEndian)
            {
                buffer[bufferIndex] = (byte)(value & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 8) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 16) & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 24) & 0xFF);
                bufferIndex++;
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
                bufferIndex++;
            }

            return;
        }


        /// <summary>
        /// UINT16 转 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="IsLittleEndian"></param>
        /// <returns></returns>
        public static void UINT16ToBytes(UInt16 value, byte[] buffer, int offset, bool IsLittleEndian)
        {
            int bufferIndex = offset;
            
            if (IsLittleEndian)
            {
                buffer[bufferIndex] = (byte)(value & 0xFF);
                bufferIndex++;

                buffer[bufferIndex] = (byte)((value >> 8) & 0xFF);
                bufferIndex++;
            }
            else
            {
                buffer[bufferIndex] = (byte)((value >> 8) & 0xFF);
                bufferIndex++;
                
                buffer[bufferIndex] = (byte)(value & 0xFF);
                bufferIndex++;
            }

            return;
        }

        /// <summary>
        /// INT32 转 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="IsLittleEndian"></param>
        /// <returns></returns>
        public static void INT16ToBytes(Int16 value, byte[] buffer, int offset, bool IsLittleEndian)
        {
            int bufferIndex = offset;
            
            if (IsLittleEndian)
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

            return;
        }
        
        /// <summary>
        /// UINT64 转 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="IsLittleEndian"></param>
        /// <returns></returns>
        public static byte[] UINT64ToBytes(UInt64 value, bool IsLittleEndian)
        {
            byte[] buffer = new byte[8];

            if (IsLittleEndian)
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
        /// <param name="IsLittleEndian"></param>
        /// <returns></returns>
        public static byte[] INT64ToBytes(Int64 value, bool IsLittleEndian)
        {
            byte[] buffer = new byte[8];

            if (IsLittleEndian)
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
        /// <param name="IsLittleEndian"></param>
        /// <returns></returns>
        public static byte[] UINT32ToBytes(UInt32 value, bool IsLittleEndian)
        {
            byte[] buffer = new byte[4];

            if (IsLittleEndian)
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
        /// <param name="IsLittleEndian"></param>
        /// <returns></returns>
        public static byte[] INT32ToBytes(Int32 value, bool IsLittleEndian)
        {
            byte[] buffer = new byte[4];

            if (IsLittleEndian)
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
        /// <param name="IsLittleEndian"></param>
        /// <returns></returns>
        public static byte[] UINT16ToBytes(UInt16 value, bool IsLittleEndian)
        {
            byte[] buffer = new byte[2];

            if (IsLittleEndian)
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
        /// <param name="IsLittleEndian"></param>
        /// <returns></returns>
        public static byte[] INT16ToBytes(Int32 value, bool IsLittleEndian)
        {
            byte[] buffer = new byte[2];

            if (IsLittleEndian)
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
        /// <param name="value"></param>
        /// <param name="IsLittleEndian"></param>
        /// <returns></returns>
        public static UInt64 BytesToUINT64(byte[] buffer, int bufferIndex, bool IsLittleEndian)
        {
            UInt64 val;

            if (IsLittleEndian)
            {
                val = ((UInt64)((buffer[bufferIndex + 7] << 56) + (buffer[bufferIndex + 6] << 48)
                    + (buffer[bufferIndex + 5] << 40) + (buffer[bufferIndex + 4] << 32)
                    + (buffer[bufferIndex + 3] << 24) + (buffer[bufferIndex + 2] << 16)
                    + (buffer[bufferIndex + 1] << 8) + buffer[bufferIndex])) & 0xFFFFFFFFFFFFFFFF;
            }
            else
            {
                val = ((UInt64)((buffer[bufferIndex] << 56) + (buffer[bufferIndex + 1] << 48)
                    + (buffer[bufferIndex + 2] << 40) + (buffer[bufferIndex + 3] << 32)
                    + (buffer[bufferIndex + 4] << 24) + (buffer[bufferIndex + 5] << 16)
                    + (buffer[bufferIndex + 6] << 8) + buffer[bufferIndex + 7])) & 0xFFFFFFFFFFFFFFFF;
            }

            return val;
        }

        /// <summary>
        ///  bytes转INT64
        /// </summary>
        /// <param name="value"></param>
        /// <param name="IsLittleEndian"></param>
        /// <returns></returns>
        public static Int64 BytesToINT64(byte[] buffer, int bufferIndex, bool IsLittleEndian)
        {
            Int64 val;

            if (IsLittleEndian)
            {
                val = ((Int64)((buffer[bufferIndex + 7] << 56) + (buffer[bufferIndex + 6] << 48)
                    + (buffer[bufferIndex + 5] << 40) + (buffer[bufferIndex + 4] << 32)
                    + (buffer[bufferIndex + 3] << 24) + (buffer[bufferIndex + 2] << 16)
                    + (buffer[bufferIndex + 1] << 8) + buffer[bufferIndex])) & 0x7FFFFFFFFFFFFFFF;
            }
            else
            {
                val = ((Int64)((buffer[bufferIndex] << 56) + (buffer[bufferIndex + 1] << 48)
                    + (buffer[bufferIndex + 2] << 40) + (buffer[bufferIndex + 3] << 32)
                    + (buffer[bufferIndex + 4] << 24) + (buffer[bufferIndex + 5] << 16)
                    + (buffer[bufferIndex + 6] << 8) + buffer[bufferIndex + 7])) & 0x7FFFFFFFFFFFFFFF;
            }

            return val;
        }

        /// <summary>
        ///  bytes转UINT32
        /// </summary>
        /// <param name="value"></param>
        /// <param name="IsLittleEndian"></param>
        /// <returns></returns>
        public static UInt32 BytesToUINT32(byte[] buffer, int bufferIndex, bool IsLittleEndian)
        {
            UInt32 val;

            if (IsLittleEndian)
            {
                val = ((UInt32)((buffer[bufferIndex + 3] << 24) + (buffer[bufferIndex + 2] << 16)
                    + (buffer[bufferIndex + 1] << 8) + buffer[bufferIndex])) & 0xFFFFFFFF;
            }
            else
            {
                val = ((UInt32)((buffer[bufferIndex] << 24) + (buffer[bufferIndex + 1] << 16)
                    + (buffer[bufferIndex + 2] << 8) + buffer[bufferIndex + 3])) & 0xFFFFFFFF;
            }

            return val;
        }

        /// <summary>
        ///  bytes转INT32
        /// </summary>
        /// <param name="value"></param>
        /// <param name="IsLittleEndian"></param>
        /// <returns></returns>
        public static Int32 BytesToINT32(byte[] buffer, int bufferIndex, bool IsLittleEndian)
        {
            Int32 val;

            if (IsLittleEndian)
            {
                val = ((Int32)((buffer[bufferIndex + 3] << 24) + (buffer[bufferIndex + 2] << 16)
                    + (buffer[bufferIndex + 1] << 8) + buffer[bufferIndex])) & 0x7FFFFFFF;
            }
            else
            {
                val = ((Int32)((buffer[bufferIndex] << 24) + (buffer[bufferIndex + 1] << 16)
                    + (buffer[bufferIndex + 2] << 8) + buffer[bufferIndex + 3])) & 0x7FFFFFFF;
            }

            return val;
        }

        /// <summary>
        ///  bytes转UINT16
        /// </summary>
        /// <param name="value"></param>
        /// <param name="IsLittleEndian"></param>
        /// <returns></returns>
        public static UInt16 BytesToUINT16(byte[] buffer, int bufferIndex, bool IsLittleEndian)
        {
            UInt16 val;

            if (IsLittleEndian)
            {
                val = (UInt16)(((buffer[bufferIndex + 1] << 8) + buffer[bufferIndex]) & 0xFFFF);
            }
            else
            {
                val = (UInt16)(((buffer[bufferIndex] << 8) + buffer[bufferIndex + 1]) & 0xFFFF);
            }

            return val;
        }

        /// <summary>
        ///  bytes转INT16
        /// </summary>
        /// <param name="value"></param>
        /// <param name="IsLittleEndian"></param>
        /// <returns></returns>
        public static Int16 BytesToINT16(byte[] buffer, int bufferIndex, bool IsLittleEndian)
        {
            Int16 val;

            if (IsLittleEndian)
            {
                val = (Int16)(((buffer[bufferIndex + 1] << 8) + buffer[bufferIndex]) & 0x7FFF);
            }
            else
            {
                val = (Int16)(((buffer[bufferIndex] << 8) + buffer[bufferIndex + 1]) & 0x7FFF);
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
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            
            return returnBytes;
        }
        #endregion
    }
}
