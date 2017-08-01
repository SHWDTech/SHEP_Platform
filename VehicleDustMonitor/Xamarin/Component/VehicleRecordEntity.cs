using Android.Content;
using Android.Database.Sqlite;

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

        public const string ColumnNameLat = "Lat";

        public const string ColumnNameLng = "Lng";

        public const string ColumnNameUploaded = "Uploaded";

        public static readonly string SqlCreateEntitis =
            $"CREATE TABLE {TableName} ({ColumnNameId} INTEGER PRIMARY KEY AUTOINCREMENT, {ColumnNameDevId} INTEGER, {ColumnNameRecordName} TEXT, {ColumnNameComment} TEXT, {ColumnNameStartDateTime} TEXT, {ColumnNameEndDateTIme} TEXT, {ColumnNameAverage} FLOAT, {ColumnNameLat} FLOAT, {ColumnNameLng} FLOAT, {ColumnNameUploaded} INTEGER)";

        public static readonly string SqlDeleteEntitis = $"DROP TABLE IF EXISTS {TableName}";

        public static long DoInsert(SQLiteDatabase db, string nullColumnHack, ContentValues values)
        {
            return db.Insert(TableName, nullColumnHack, values);
        }

        public static int DoUpdate(SQLiteDatabase db, long id, ContentValues values)
        {
            return db.Update(TableName, values, "Id=?", new[] {id.ToString()});
        }
    }
}