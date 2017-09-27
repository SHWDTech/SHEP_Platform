using System;
using System.Linq;
using Quartz;

namespace SHEP_Platform.ScheduleJobs
{
    public class CalcMinuteAvgJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var now = DateTime.Now;
            var checkTime = DateTime.Now.AddMinutes(-1);
            using (var ctx = new ESMonitorEntities())
            {
                var cityDatas = ctx.T_ESMin.Where(d => d.UpdateTime > checkTime).ToList();
                var cityStatis = new T_Statistics
                {
                    TP = cityDatas.Average(t => t.TP),
                    DB = cityDatas.Average(t => t.DB),
                    PM25 = cityDatas.Average(t => t.PM25),
                    PM100 = cityDatas.Average(t => t.PM100),
                    country = 1,
                    type = 0,
                    UpdateTime = now
                };
                ctx.T_Statistics.Add(cityStatis);
                foreach (var country in ctx.T_Country)
                {
                    var allDatas = cityDatas.Where(d => d.Country == country.Id.ToString()).ToList();
                    if (!allDatas.Any()) continue;
                    var statis = new T_Statistics
                    {
                        TP = allDatas.Average(t => t.TP),
                        DB = allDatas.Average(t => t.DB),
                        PM25 = allDatas.Average(t => t.PM25),
                        PM100 = allDatas.Average(t => t.PM100),
                        country = country.Id,
                        type = 1,
                        UpdateTime = now
                    };
                    ctx.T_Statistics.Add(statis);
                }
                ctx.SaveChanges();
            }
        }
    }
}