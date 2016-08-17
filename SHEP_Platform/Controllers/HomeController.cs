using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using SHEP_Platform.Enum;
using SHEP_Platform.Models.Home;

namespace SHEP_Platform.Controllers
{
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// 数据库Context
        /// </summary>
        public ActionResult Index()
        {
            var stats = WdContext.StatList;

            var list = new List<StatStatus>();

            var homestatList = new HomeStatList();

            foreach (var stat in stats)
            {
                var statmodel = new StatList
                {
                    Id = stat.Id,
                    Name = stat.StatName,
                    Address = stat.Address
                };

                var statId = stat.Id.ToString();
                var devIds = DbContext.T_Devs.Where(dev => dev.StatId == statId).Select(devId => devId.Id).ToArray();

                var tpTotal = 0.0d;
                var dbTotal = 0.0d;
                var pm25Total = 0.0d;
                var pm100Total = 0.0d;
                var vocTotal = 0.0d;
                var validDev = 0;
                var lastUpdateTime = DateTime.Now;
                foreach (var id in devIds)
                {
                    var esMin = DbContext.T_ESMin.OrderByDescending(obj => obj.Id).FirstOrDefault(es => es.DevId == id);
                    if (esMin != null)
                    {
                        tpTotal += esMin.TP;
                        dbTotal += esMin.DB;
                        pm25Total += esMin.PM25.GetValueOrDefault();
                        pm100Total += esMin.PM100.GetValueOrDefault();
                        vocTotal += esMin.VOCs.GetValueOrDefault();
                        validDev += 1;
                        if (esMin.UpdateTime != null) lastUpdateTime = esMin.UpdateTime.Value;
                    }
                }

                list.Add(new StatStatus()
                {
                    Name = stat.StatName,
                    Id = stat.Id,
                    AvgTp = (tpTotal / validDev / 1000.0).ToString("f2"),
                    AvgDb = (dbTotal / validDev).ToString("f2"),
                    AvgPm25 = (pm25Total / validDev).ToString("f2"),
                    AvgPm100 = (pm100Total /validDev).ToString("f2"),
                    AvgVoc = (vocTotal / validDev).ToString("f2"),
                    UpdateTime = lastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    Longitude = stat.Longitude,
                    Latitude = stat.Latitude,
                    PolluteType = PolluteType.NotOverRange
                });

                statmodel.AvgTp = (tpTotal/validDev/1000.0).ToString("f2");
                statmodel.AvgDb = (dbTotal/validDev).ToString("f2");
                homestatList.StatLists.Add(statmodel);
            }

            ViewBag.Status = JsonConvert.SerializeObject(list);

            return DynamicView("Index", homestatList);
        }
    }
}