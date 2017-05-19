// ReSharper disable InconsistentNaming
namespace HikCameraConfiger.Model
{
    public class AccessTokenResponse
    {
        public AccessTokenData data { get; set; }

        public string code { get; set; }

        public string msg { get; set; }
    }

    public class AccessTokenData
    {
        public string accessToken { get; set; }

        public string expireTime { get; set; }
    }
}
