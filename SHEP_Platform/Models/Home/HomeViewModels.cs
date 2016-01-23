using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHEP_Platform.Models.Home
{
    public class StatStatus
    {
        public string Name { get; set; }

        public string AvgTp { get; set; }

        public string AvgDb { get; set; }

        public string UpdateTime { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }
    }
}
