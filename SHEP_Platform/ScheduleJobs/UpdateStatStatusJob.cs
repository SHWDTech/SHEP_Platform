using System;
using System.Globalization;
using System.Linq;
using ESMonitor.Model;
using Newtonsoft.Json;
using Platform.Cache;
using Quartz;
using SHEP_Platform.Common;
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
                var allDevs = ctx.T_Devs.ToList();
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
                    
                    var tpTotal = 0.0d;
                    var dbTotal = 0.0d;
                    var pm25Total = 0.0d;
                    var pm100Total = 0.0d;
                    var validDev = 0;
                    var lastUpdateTime = DateTime.Now;
                    var devIds = allDevs.Where(dev => dev.StatId == stat.Id.ToString()).Select(devId => devId.Id).ToArray();
                    if (devIds.Length <= 0)
                    {
                        validDev = 1;
                    }
                    else
                    {
                        foreach (var devid in devIds)
                        {
                            var devData = RedisService.GetRedisDatabase().StringGet($"DustLastValue:{stat.Id}-{devid}");

                            if (devData.HasValue)
                            {
                                var esMin = JsonConvert.DeserializeObject<EsMin>(devData);
                                tpTotal += esMin.Tp;
                                dbTotal += esMin.Db;
                                pm25Total += esMin.Pm25;
                                pm100Total += esMin.Pm100;
                                lastUpdateTime = esMin.UpdateTime;
                            }
                            validDev += 1;
                        }
                    }

                    status.AvgTp = (tpTotal / validDev / 1000.0).ToString("f3");
                    status.AvgDb = (dbTotal / validDev).ToString("f3");
                    status.AvgPm25 = (pm25Total / validDev / 1000.0).ToString("f3");
                    status.AvgPm100 = (pm100Total / validDev / 1000.0).ToString("f3");
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