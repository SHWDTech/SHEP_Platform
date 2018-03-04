using System;
using System.Linq;
using System.Web;
using SHEP_Platform.Common;
using SHEP_Platform.Enums;

namespace SHEP_Platform.Process
{
    public class AuthenticationProcess
    {
        private ESMonitorEntities DbContext { get; }

        public HttpRequestBase Request { get; set; }

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
                var investigate = new Investigate
                {
                    IpAddr = Request.UserHostAddress,
                    Message = $"用户尝试登陆失败，错误原因：密码错误，用户名：{userName}",
                    MessageTime = DateTime.Now
                };
                DbContext.Investigate.Add(investigate);
                DbContext.SaveChanges();
                return result;
            }

            if (loginUser.Status == 1)
            {
                result.ResultType = LoginResultType.AccountLocked;
                result.ErrorMessage = "账户已锁定";
                var investigate = new Investigate
                {
                    IpAddr = Request.UserHostAddress,
                    Message = $"用户尝试登陆失败，错误原因：账户已锁定，用户名：{userName}",
                    MessageTime = DateTime.Now,
                    UserId = loginUser.UserId
                };
                DbContext.Investigate.Add(investigate);
                DbContext.SaveChanges();
                return result;
            }

            if (loginUser.Status == 0)
            {
                result.ResultType = LoginResultType.AccountLocked;
                result.ErrorMessage = "用户未审核";
                var investigate = new Investigate
                {
                    IpAddr = Request.UserHostAddress,
                    Message = $"用户尝试登陆失败，错误原因：用户未审核，用户名：{userName}",
                    MessageTime = DateTime.Now,
                    UserId = loginUser.UserId
                };
                DbContext.Investigate.Add(investigate);
                DbContext.SaveChanges();
                return result;
            }

            loginUser.LastTime = DateTime.Now;

            try
            {
                var investigate = new Investigate
                {
                    IpAddr = Request.UserHostAddress,
                    Message = $"用户尝试登陆成功，用户名：{userName}",
                    MessageTime = DateTime.Now,
                    UserId = loginUser.UserId
                };
                DbContext.Investigate.Add(investigate);
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