namespace VehicleDustMonitor.Xamarin.Component
{
    public static class VehicleRecordEntity
    {
        public const string TableName = "VehicleRecord";

        public const string ColumnNameId = "Id";

        public const string ColumnNameDevId = "DevId";

        public const string ColumnNameRecordName = "RecordName";

        public const string ColumnNameComment = "Comment";

        public const string ColumnNameStartDateTime = "StartDateTime";

        public const string ColumnNameEndDateTIme = "EndDateTime";

        public const string ColumnNameAverage = "Average";

        public static readonly string SqlCreateEntitis =
            $"CREATE TABLE {TableName} ({ColumnNameId} INTEGER PRIMARY KEY AUTOINCREMENT, {ColumnNameDevId} INTEGER, {ColumnNameRecordName} TEXT, {ColumnNameComment} TEXT, {ColumnNameStartDateTime} TEXT, {ColumnNameEndDateTIme} TEXT, {ColumnNameAverage} FLOAT)";

        public static readonly string SqlDeleteEntitis = $"DROP TABLE IF EXISTS {TableName}";
    }
}