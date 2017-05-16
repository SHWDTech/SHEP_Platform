using System.Configuration;

namespace SHEP_Platform.Common
{
    public class AppConfig
    {
        public static string CompanyName { get; } = ConfigurationManager.AppSettings["company"];
    }
}