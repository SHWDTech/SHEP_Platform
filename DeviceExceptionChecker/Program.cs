using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseModel;
using DeviceExceptionChecker.AlarmSender;
using DeviceExceptionChecker.Database;
using DeviceExceptionChecker.MessageWarpper;

namespace DeviceExceptionChecker
{
    class Program
    {
        private static Dictionary<string, List<string>> _textMobileNumber;

        static void Main()
        {

            using (var ctx = new ESMonitor2())
            {
                LoadTextMobileNumber(ctx);
                try
                {
                    CheckForDeviceNoData(ctx);
                    CheckForZeroData(ctx);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        static void LoadTextMobileNumber(ESMonitor2 ctx)
        {
            _textMobileNumber = ctx.T_SysConfig.Where(cfg => cfg.ConfigType == "DeviceExceptionTextNumber")
                .GroupBy(c => c.ConfigName)
                .ToDictionary(config => config.Key.Trim(), config => config.Select(g => g.ConfigValue).ToList());
        }

        static void CheckForDeviceNoData(ESMonitor2 ctx)
        {
            var result = ctx.CheckForDeviceNoData();
            foreach (var dataResult in result)
            {
                var devName = ctx.T_Devs.First(d => d.Id == dataResult.DevId).DevCode;
                var statName = ctx.T_Stats.First(s => s.Id == dataResult.StatId).StatName;
                foreach (var number in _textMobileNumber["OfflineException"])
                {
                    TextMessageSender.Send(number, DustMessageWarpper.WarpperMessage((DeviceExceptionType)dataResult.ExceptionType, devName, statName));
                }
            }
        }

        static void CheckForZeroData(ESMonitor2 ctx)
        {
            var result = ctx.CheckForDeviceZeroData();
            foreach (var dataResult in result)
            {
                var devName = ctx.T_Devs.First(d => d.Id == dataResult.DevId).DevCode;
                var statName = ctx.T_Stats.First(s => s.Id == dataResult.StatId).StatName;
                foreach (var number in _textMobileNumber["OfflineException"])
                {
                    TextMessageSender.Send(number, DustMessageWarpper.WarpperMessage((DeviceExceptionType)dataResult.ExceptionType, devName, statName, 0));
                }
            }
        }
    }
}
