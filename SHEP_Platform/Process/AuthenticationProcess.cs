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

        public T_Users Login(string userName, string passWord)
        {
            var pwdMd5 = Global.GetMd5(passWord);
            return DbContext.T_Users.FirstOrDefault(obj => obj.UserName == userName && obj.Pwd == pwdMd5);
        }
    }
}