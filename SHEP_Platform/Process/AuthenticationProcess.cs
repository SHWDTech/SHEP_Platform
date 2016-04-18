using System;
using System.Linq;
using SHEP_Platform.Common;
using SHEP_Platform.Enum;

namespace SHEP_Platform.Process
{
    public class AuthenticationProcess
    {
        private ESMonitorEntities DbContext { get; }
        public AuthenticationProcess()
        {
            DbContext = new ESMonitorEntities();
        }

        public LoginResult Login(string userName, string passWord)
        {
            var result = new LoginResult();

            var loginUser = DbContext.T_Users.FirstOrDefault(user => user.UserName == userName);
            if (loginUser  == null)
            {
                result.ResultType = LoginResultType.ValidUserName;
                result.ErrorElement = "UserName";
                result.ErrorMessage = "用户名不存在";
                return result;
            }

            var pwdMd5 = Global.GetMd5(passWord);

            loginUser = DbContext.T_Users.FirstOrDefault(obj => obj.UserName == userName && obj.Pwd == pwdMd5);
            if (loginUser == null)
            {
                result.ResultType = LoginResultType.ValidUserName;
                result.ErrorElement = "PassWord";
                result.ErrorMessage = "密码错误";
                return result;
            }

            if (loginUser.Status == 1)
            {
                result.ResultType = LoginResultType.AccountLocked;
                result.ErrorMessage = "账户已锁定";
                return result;
            }

            loginUser.LastTime = DateTime.Now;

            try
            {
                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            result.User = loginUser;

            return result;
        }
    }
}