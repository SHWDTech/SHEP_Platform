using System;
using System.Collections.Generic;

namespace SHEP_Platform.Models.Analysis
{
    public class AlarmDetailsViewModel
    {
        public List<AlarmFullDetail> Details { get; set; } = new List<AlarmFullDetail>();
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