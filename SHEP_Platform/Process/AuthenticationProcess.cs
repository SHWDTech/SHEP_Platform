using System.Linq;
using SHEP_Platform.Common;

namespace SHEP_Platform.Process
{
    public class AuthenticationProcess
    {
        private ESMonitorEntities DbContext { get; }
        public AuthenticationProcess()
        {
            DbContext = new ESMonitorEntities();
        }

        public bool Login(string userName, string passWord)
        {
            var pwdMd5 = Global.GetMd5(passWord);
            return DbContext.T_Users.Any(user => user.UserName == userName && user.Pwd == pwdMd5);
        }
    }
}