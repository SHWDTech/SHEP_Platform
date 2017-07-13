using System.Security.Cryptography;
using System.Text;
using Android.Content;

namespace VehicleDustMonitor.Xamarin.Component
{
    public class BaseUtils
    {
        public static string GetHashSha256(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            using (var hashstring = new SHA256Managed())
            {
                var hash = hashstring.ComputeHash(bytes);
                var hashString = string.Empty;
                var builder = new StringBuilder();
                builder.Append(hashString);
                foreach (var x in hash)
                {
                    builder.Append($"{x:x2}");
                }
                hashString = builder.ToString();
                return hashString;
            }
        }
    }
}