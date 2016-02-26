namespace SHEP_Platform.Models.Home
{
    public class StatStatus
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public string AvgTp { get; set; }

        public string AvgDb { get; set; }

        public string AvgPm25 { get; set; }

        public string AvgPm100 { get; set; }

        public string AvgVoc{ get; set; }

        public string UpdateTime { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public string PolluteType { get; set; }
    }
}
