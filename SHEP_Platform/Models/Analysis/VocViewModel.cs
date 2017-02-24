using System;
using System.Collections.Generic;

namespace SHEP_Platform.Models.Analysis
{
    public class VocViewModel
    {
       public List<VocValue> VocValueList = new List<VocValue>();
    }

    public struct VocValue
    {
        public string UpdateTime { get; set; }

        public double Value { get; set; }
    }
}