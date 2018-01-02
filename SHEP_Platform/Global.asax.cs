using System;
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

            StartSchedules();
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
