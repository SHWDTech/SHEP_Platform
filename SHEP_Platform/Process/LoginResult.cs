using SHEP_Platform.Enums;

namespace SHEP_Platform.Process
{
    /// <summary>
    /// 登陆尝试结果
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// 登陆用户
        /// </summary>
        public T_Users User { get; set; }

        /// <summary>
        /// 登陆结果类型
        /// </summary>
        public LoginResultType ResultType { get; set; }

        public string ErrorElement { get; set; } = string.Empty;


        public string ErrorMessage { get; set; } = string.Empty;
    }
}