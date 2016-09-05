using System;
using System.Collections.Generic;
using PagedList;

namespace SHEP_Platform.Models.Analysis
{
    public class AlarmDetailsViewModel
    {
        public IPagedList<AlarmFullDetail> Details { get; set; }

        public int page { get; set; } = 1;
    }

    public class AlarmFullDetail
    {
        public int AlarmId { get; set; }

        public string StatName { get; set; }

        public string ChargeMan { get; set; }

        public string Telephone { get; set; }

        public string DevCode { get; set; }

        public DateTime AlarmDateTime { get; set; }

        public string FaultValue { get; set; }

        public bool IsReaded { get; set; }
    }
}