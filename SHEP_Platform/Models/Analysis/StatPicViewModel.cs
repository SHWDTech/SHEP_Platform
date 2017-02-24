using System.Collections.Generic;

namespace SHEP_Platform.Models.Analysis
{
    public class StatPicViewModel
    {
        public int StatId { get; set; }

        public List<string> PicUrls { get; set; } = new List<string>();
    }
}