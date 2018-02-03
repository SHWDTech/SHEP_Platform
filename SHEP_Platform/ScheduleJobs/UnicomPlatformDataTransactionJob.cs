using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Quartz;
using SHEP_Platform.Common;
using SHEP_Platform.UnicomPlatform;
using SHWDTech.Platform.Utility;

namespace SHEP_Platform.ScheduleJobs
{
    public class UnicomPlatformDataTransactionJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var service = new UnicomService();
            using (var ctx = new ESMonitorEntities())
            {
                var unicomProjects = ctx.T_UnicomProject.ToList();
                var unicomDevices = ctx.T_UnicomDevice.ToList();
                var checkTime = DateTime.Now.AddMinutes(-1);
                foreach (var project in unicomProjects)
                {
                    var stat = ctx.T_Stats.First(s => s.Id == project.StatId);
                    foreach (var device in unicomDevices)
                    {
                        var status = EmsdataStatus.Normal;
                        try
                        {
                            var emsDatas = FetchRecentData(ctx, device.DevId, device.StatId, checkTime);
                            var schedule = TryGetSchedule(device.Id);
                            if (schedule == null) continue;
                            if (emsDatas.Count <= 0)
                            {
                                LoadFromHistoryData(ctx, emsDatas);
                                status = EmsdataStatus.NotFound;
                            }

                            AddDeviceInfo(emsDatas, project, device);
                            if (FixErrorData(emsDatas, schedule) && status != EmsdataStatus.NotFound)
                            {
                                status = EmsdataStatus.Exceeded;
                            }
                            var result = service.PushRealTimeData(emsDatas.ToArray());
                            if (result.result.Length > 0)
                            {
                                foreach (var entry in result.result)
                                {
                                    LogService.Instance.Warn($"发送联通数据失败,错误原因key:{entry.key},value:{entry.value}");
                                }
                            }
                            else
                            {
                                AfterUpdateProcess(status, ctx, emsDatas.First(), project.StatId, device.Id, stat.Country, checkTime);
                            }

                        }
                        catch (Exception ex)
                        {
                            LogService.Instance.Error("联通数据上传处理失败。", ex);
                        }
                    }
                }
            }

        }

        private static UnicomDataGenerateSchedule TryGetSchedule(int deviceId)
        {
            var schedules = UnicomDataGenerateSchedule.CachedSchedules.Where(s => s.Value.DeviceList.Contains(deviceId))
                .ToList();
            if (schedules.Count == 0) return null;
            schedules = schedules.OrderBy(s => s.Value.SchedulePriority).ToList();
            return schedules.First().Value;
        }

        private static List<emsData> FetchRecentData(ESMonitorEntities ctx, int devId, int statId, DateTime checkTime)
        {
            var recent = ctx.T_ESMin.Where(m => m.StatId == statId && m.DevId == devId && m.UpdateTime > checkTime)
                .Take(1).ToList();
            return EsMinToEmsDatas(recent);
        }

        private static List<emsData> EsMinToEmsDatas(IEnumerable<T_ESMin> esMins) => esMins.Select(esMin => new emsData
        {
            dust = (float)esMin.TP / 1000,
            temperature = (float)esMin.Temperature,
            humidity = (float)esMin.Humidity,
            noise = (int)esMin.DB,
            windSpeed = (float)esMin.WindSpeed,
            windDirection = (int)esMin.WindDirection,
            dateTime = ConvertToUnixTime(esMin.UpdateTime.Value),
            dustFlag = "N",
            humiFlag = "N",
            noiseFlag = "N"
        }).ToList();

        private static bool FixErrorData(List<emsData> datas, UnicomDataGenerateSchedule schedule)
        {
            var random = new Random();
            var exceed = false;
            foreach (var data in datas)
            {
                if (schedule.DataRanges.ContainsKey("dust"))
                {
                    var dustRange = schedule.DataRanges["dust"];
                    if (data.dust > dustRange.MaxValue || data.dust < dustRange.MinValue)
                    {
                        data.dust = (float)random.GetNextDouble(dustRange.MinValue, dustRange.MaxValue);
                        exceed = true;
                    }
                }

                if (schedule.DataRanges.ContainsKey("noise"))
                {
                    var dbRange = schedule.DataRanges["noise"];
                    if (data.noise > dbRange.MaxValue || data.noise < dbRange.MinValue)
                    {
                        data.noise = (int)random.GetNextDouble(dbRange.MinValue, dbRange.MaxValue);
                        exceed = true;
                    }
                }

                if (schedule.DataRanges.ContainsKey("temperature"))
                {
                    var tempRange = schedule.DataRanges["temperature"];
                    if (data.temperature > tempRange.MaxValue || data.temperature < tempRange.MinValue)
                    {
                        data.temperature = (int)random.GetNextDouble(tempRange.MinValue, tempRange.MaxValue);
                        exceed = true;
                    }
                }

                if (schedule.DataRanges.ContainsKey("humidity"))
                {
                    var humRange = schedule.DataRanges["humidity"];
                    if (data.humidity > humRange.MaxValue || data.humidity < humRange.MinValue)
                    {
                        data.humidity = (int)random.GetNextDouble(humRange.MinValue, humRange.MaxValue);
                        exceed = true;
                    }
                }

                if (schedule.DataRanges.ContainsKey("windSpeed"))
                {
                    var wsRange = schedule.DataRanges["windSpeed"];
                    if (data.windSpeed > wsRange.MaxValue || data.windSpeed < wsRange.MinValue)
                    {
                        data.windSpeed = (int)random.GetNextDouble(wsRange.MinValue, wsRange.MaxValue);
                        exceed = true;
                    }
                }

                if (schedule.DataRanges.ContainsKey("windDirection"))
                {
                    var wdRange = schedule.DataRanges["windDirection"];
                    if (data.windDirection > wdRange.MaxValue || data.windDirection < wdRange.MinValue)
                    {
                        data.windDirection = (int)random.GetNextDouble(wdRange.MinValue, wdRange.MaxValue);
                        exceed = true;
                    }
                }
            }

            return exceed;
        }

        private static void AddDeviceInfo(List<emsData> emsDatas, T_UnicomProject prj, T_UnicomDevice dev)
        {
            foreach (var emsData in emsDatas)
            {
                emsData.prjCode = prj.UnicomCode;
                emsData.devCode = dev.UnicomCode;
            }
        }

        private static void LoadFromHistoryData(ESMonitorEntities ctx, List<emsData> emsDatas)
        {
            var value = ctx.T_ESMin.OrderBy(m => m.UpdateTime).Take(1).ToList();
            emsDatas.AddRange(EsMinToEmsDatas(value));
        }

        private static long ConvertToUnixTime(DateTime dateTime)
        {
            var sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long)(dateTime.ToUniversalTime() - sTime).TotalMilliseconds;
        }

        private static void AfterUpdateProcess(EmsdataStatus status, ESMonitorEntities ctx, emsData data, int statId, int devId, int country, DateTime checkTime)
        {
            if (status == EmsdataStatus.NotFound)
            {
                ctx.T_ESMin.Add(new T_ESMin
                {
                    TP = data.dust,
                    PM25 = data.dust,
                    PM100 = data.dust,
                    DB = data.noise,
                    Temperature = data.temperature,
                    Humidity = data.maxHumidity,
                    WindSpeed = data.windSpeed,
                    WindDirection = data.windDirection,
                    StatId = statId,
                    DevId = devId,
                    Country = country.ToString()
                });
            }
            else if (status == EmsdataStatus.Exceeded)
            {
                var last = ctx.T_ESMin
                    .Where(d => d.StatId == statId && d.DevId == devId && d.Country == country.ToString() &&
                                d.UpdateTime > checkTime).OrderByDescending(d => d.UpdateTime).First();
                last.TP = data.dust;
                last.PM25 = data.dust;
                last.PM100 = data.dust;
                last.DB = data.noise;
                last.Temperature = data.temperature;
                last.Humidity = data.maxHumidity;
                last.WindSpeed = data.windSpeed;
                last.WindDirection = data.windDirection;
                last.StatId = statId;
                last.DevId = devId;
                last.Country = country.ToString();
                ctx.T_ESMin.AddOrUpdate(last);
            }
            ctx.SaveChanges();
        }
    }

    public enum EmsdataStatus
    {
        Normal,

        NotFound,

        Exceeded
    }
}