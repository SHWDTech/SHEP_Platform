// ReSharper disable InconsistentNaming
namespace SHEP_Platform.Models.Api
{
    public class TablePost
    {
        public int offset { get; set; }

        public int limit { get; set; }

        public string sort { get; set; }

        public string order { get; set; }

        public string act { get; set; }

        public string title { get; set; }
    }

    public class NameQueryTablePost : TablePost
    {
        public string QueryName { get; set; }
    }
}