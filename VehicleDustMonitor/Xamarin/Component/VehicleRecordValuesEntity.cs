namespace VehicleDustMonitor.Xamarin.Component
{
    public class VehicleRecordValuesEntity
    {
        public const string TableName = "VehicleRecordValue";

        public const string ColumnNameId = "Id";

        public const string ColumnNameRecordId = "RecordId";

        public const string ColumnNameValue = "Value";

        public static readonly string SqlCreateEntitis =
            $"CREATE TABLE {TableName} ({ColumnNameId} INTEGER PRIMARY KEY AUTOINCREMENT, {ColumnNameRecordId} INTEGER, {ColumnNameValue} FLOAT)";

        public static readonly string SqlDeleteEntitis = $"DROP TABLE IF EXISTS {TableName}";
    }
}