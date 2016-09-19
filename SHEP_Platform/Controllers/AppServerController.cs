using System.Linq;
using System.Web.Http;
using SHEP_Platform.Models.Api;

namespace SHEP_Platform.Controllers
{
    public class AppServerController : ApiController
    {
        private readonly ESMonitorEntities _dbContext = new ESMonitorEntities();

        public AppServer Get([FromUri]string appCode)
        {
            var server = new AppServer
            {
                address = _dbContext.T_SysConfig.First(obj => obj.ConfigType == "AppServer" && obj.ConfigName == appCode).ConfigValue.Trim(),
                port = int.Parse(_dbContext.T_SysConfig.First(obj => obj.ConfigType == "AppServerPort" && obj.ConfigName == appCode).ConfigValue)
            };
            return server;
        }
    }
}