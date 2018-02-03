using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Quartz;
using Quartz.Impl;
using SHEP_Platform.ScheduleJobs;

namespace SHEP_Platform
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            WireUp();
            StartSchedules();
        }

        void WireUp()
        {
            using (var context = new ESMonitorEntities())
            {
                var schedules = context.T_UnicomSchedule.ToList();
                var datas = context.T_UnicomScheduleConfig.ToList();
                var scheduleDevices = context.T_UnicomScheduleDevice.ToList();
                foreach (var schedule in schedules)
                {
                    var sch = new UnicomDataGenerateSchedule
                    {
                        ScheduleName = schedule.ScheduleName,
                        SchedulePriority = schedule.SchedulePriority,
                        DataRanges = new Dictionary<string, DataRange>()
                    };
                    foreach (var data in datas)
                    {
                        if (data.ScheduleId != schedule.Id) continue;
                        sch.DataRanges.Add(data.DataName, new DataRange
                        {
                            DataName = data.DataName,
                            MaxValue = data.MaxValue,
                            MinValue = data.MinValue
                        });
                    }

                    foreach (var device in scheduleDevices)
                    {
                        if (device.ScheduleId != schedule.Id) continue;
                        sch.DeviceList.Add(device.DeviceId);
                    }
                    UnicomDataGenerateSchedule.CachedSchedules.Add(schedule.Id.ToString(), sch);
                }
            }
        }

        void StartSchedules()
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler();

            scheduler.Start();

            var job = JobBuilder.Create<UpdateStatStatusJob>()
                .Build();

            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);

            var job1 = JobBuilder.Create<CalcMinuteAvgJob>()
                .Build();

            var trigger1 = TriggerBuilder.Create()
                .StartAt(new DateTimeOffset(DateTime.Now.AddSeconds(60 - DateTime.Now.Second)))
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
                .Build();
            scheduler.ScheduleJob(job1, trigger1);

            var jobUnicom = JobBuilder.Create<UnicomPlatformDataTransactionJob>()
                .Build();

            var triggerUnicom = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
                .Build();

            scheduler.ScheduleJob(jobUnicom, triggerUnicom);
        }
    }
}
