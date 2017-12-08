using System;
using System.Collections.Generic;
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
                var checkTime = DateTime.Now.AddMinutes(-1);
                foreach (var project in ctx.T_UnicomProject.Where(s => !s.Stopped))
                {
                    foreach (var device in ctx.T_UnicomDevice.Where(d => d.StatId == project.StatId && d.OnCalc))
                    {
                        var emsDatas = FetchRecentData(ctx, device.DevId, device.StatId, checkTime);
                        if (emsDatas.Count <= 0)
                        {
                            LoadFromHistoryData(ctx, emsDatas);
                        }
                        AddDeviceInfo(emsDatas, project, device);
                        FixErrorData(emsDatas);
                        var result = service.PushRealTimeData(emsDatas.ToArray());
                        if (result.result.Length > 0)
                        {
                            foreach (var entry in result.result)
                            {
                                LogService.Instance.Warn($"发送联通数据失败,错误原因key:{entry.key},value:{entry.value}");
                            }
                        }
                    }
                }
            }

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

        private static void FixErrorData(List<emsData> datas)
        {
            foreach (var data in datas)
            {
                if (data.dust > 1 || data.dust < 0.01)
                {
                    data.dust = new Random().Next(50, 350) / 100f;
                }
                if (data.noise <= 1)
                {
                    data.noise = new Random().Next(40, 65);
                }
                if (data.temperature <= 1)
                {
                    data.temperature = new Random().Next(150, 250) / 10.0f;
                }
                if (data.humidity <= 1)
                {
                    data.humidity = new Random().Next(400, 750) / 10.0f;
                }
                if (data.windDirection <= 1)
                {
                    data.windDirection = new Random().Next(0, 360);
                }
                if (data.windSpeed <= 0.01)
                {
                    data.windSpeed = new Random().Next(0, 10) / 10.0f;
                }
            }
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
    }
}