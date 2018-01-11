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
        public string StatName { get; set; }
        public string DevCode { get; set; }
        public string NodeId { get; set; }
        public string QueryName { get; set; }
    }

    public class StatQueryTablePost : TablePost
    {
        public int Country { get; set; }

        public string Chargeman { get; set; }

        public string Department { get; set; }

        public string Address { get; set; }
    }

    public class ExceptionHistoryTablePost : TablePost
    {
        public int? DevId { get; set; }

        public int? StatId { get; set; }
    }
}