using System.Configuration;

namespace SHEP_Platform.Process
{
    public static class GlobalConfig
    {
        public static readonly string HikPictureUrl;

        static GlobalConfig()
        {
            HikPictureUrl = ConfigurationManager.AppSettings["PicUrl"];
        }
    }
}