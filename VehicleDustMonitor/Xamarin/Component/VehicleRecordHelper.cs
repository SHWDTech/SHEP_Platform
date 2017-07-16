using System;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.Runtime;

namespace VehicleDustMonitor.Xamarin.Component
{
    public class VehicleRecordHelper : SQLiteOpenHelper
    {
        public static readonly int DatabaseVersion = 2;

        public static readonly string DatabaseFileName = "VehicleRecord.sqlite";

        public VehicleRecordHelper(Context context) : this(context, DatabaseFileName, null, DatabaseVersion)
        {
            
        }

        public VehicleRecordHelper(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public VehicleRecordHelper(Context context, string name, SQLiteDatabase.ICursorFactory factory, int version) : base(context, name, factory, version)
        {
        }

        public VehicleRecordHelper(Context context, string name, SQLiteDatabase.ICursorFactory factory, int version, IDatabaseErrorHandler errorHandler) : base(context, name, factory, version, errorHandler)
        {
        }

        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL(VehicleRecordEntity.SqlCreateEntitis);
            db.ExecSQL(VehicleRecordValuesEntity.SqlDeleteEntitis);
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            //db.ExecSQL(VehicleRecordEntity.SqlDeleteEntitis);
            //OnCreate(db);
        }
    }
}