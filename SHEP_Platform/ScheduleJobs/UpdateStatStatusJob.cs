using System;
using System.Globalization;
using System.Linq;
using Platform.Cache;
using Quartz;
using SHEP_Platform.Enums;
using SHEP_Platform.Models.Home;

namespace SHEP_Platform.ScheduleJobs
{
    public class UpdateStatStatusJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            using (var ctx = new ESMonitorEntities())
            {
                var statIds = ctx.T_Stats;
                foreach (var stat in statIds)
                {
                    var cacheName = $"StatStatus:id={stat.Id}";
                    var status = new StatStatus
                    {
                        Id = stat.Id,
                        Name = stat.StatName,
                        Longitude = stat.Longitude,
                        Latitude = stat.Latitude,
                        PolluteType = PolluteType.NotOverRange
                    };

                    var devIds = ctx.T_Devs.Where(dev => dev.StatId == stat.Id.ToString()).Select(devId => devId.Id).ToArray();

                    var tpTotal = 0.0d;
                    var dbTotal = 0.0d;
                    var pm25Total = 0.0d;
                    var pm100Total = 0.0d;
                    var vocTotal = 0.0d;
                    var validDev = 0;
                    var lastUpdateTime = DateTime.Now;
                    foreach (var devid in devIds)
                    {
                        var esMin = ctx.T_ESMin.OrderByDescending(obj => obj.UpdateTime).FirstOrDefault(es => es.DevId == devid);
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

                    status.AvgTp = (tpTotal / validDev / 1000.0).ToString("f3");
                    status.AvgDb = (dbTotal / validDev).ToString("f3");
                    status.AvgPm25 = (pm25Total / validDev / 1000.0).ToString("f3");
                    status.AvgPm100 = (pm100Total / validDev / 1000.0).ToString("f3");
                    status.AvgVoc = (vocTotal / validDev).ToString("f3");
                    status.UpdateTime = lastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    status.PolluteType = PolluteType.NotOverRange;
                    PlatformCaches.Add(cacheName, status, cacheType:"statStatus");
                    var modelCacheName = $"StatList:id={stat.Id}";
                    var model = new StatList
                    {
                        Id = stat.Id,
                        Name = stat.StatName,
                        Address = stat.Address,
                        Latitude = stat.Latitude.ToString(CultureInfo.InvariantCulture),
                        Longitude = stat.Longitude.ToString(CultureInfo.InvariantCulture)
                    };
                    PlatformCaches.Add(modelCacheName, model, cacheType: "statList");
                }
            }
        }
    }
}