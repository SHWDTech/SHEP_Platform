// ReSharper disable InconsistentNaming
namespace SHEP_Platform.Common
{
    /// <summary>
    /// 海康SDK方法
    /// </summary>
    public static class HikMethod
    {
        /// <summary>
        /// 获取AccessToken
        /// </summary>
        public const string GetAccessToken = "token/getAccessToken";
    }

    public enum PTZCommand
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        UPLEFT,
        DOWNLEFT,
        UPRIGHT,
        DOWNRIGHT,
        ZOOMIN,
        ZOOMOUT,
        FOCUSNEAR,
        FOCUSFAR,
        IRISSTARTUP,
        IRISSTOPDOWN,
        LIGHT,
        WIPER,
        AUTO,
        UNKNOW
    }

    public enum PTZACtion
    {
        START,
        STOP
    }
}
