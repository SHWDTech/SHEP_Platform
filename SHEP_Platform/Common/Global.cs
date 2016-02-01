using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

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
    }
}