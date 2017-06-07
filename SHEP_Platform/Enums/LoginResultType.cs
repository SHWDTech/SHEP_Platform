namespace SHEP_Platform.Enums
{
    /// <summary>
    /// 登陆结果
    /// </summary>
    public enum LoginResultType
    {
        /// <summary>
        /// 用户名不存在
        /// </summary>
        ValidUserName,

        /// <summary>
        /// 密码错误
        /// </summary>
        ValidPassword,

        /// <summary>
        /// 账户已锁定
        /// </summary>
        AccountLocked,

        /// <summary>
        /// 未知登陆错误
        /// </summary>
        InvalidError
    }
}