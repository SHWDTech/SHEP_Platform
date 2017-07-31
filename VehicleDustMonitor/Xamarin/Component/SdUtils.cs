using System;
using Android.Content;

namespace VehicleDustMonitor.Xamarin.Component
{
    public class SdUtils
    {
        public static Context Context { get; private set; }

        public static void Init(Context context)
        {
            if (Context == null)
            {
                Context = context.ApplicationContext;
            }
        }


        public static Java.IO.File GetExternalDownloadDir()
        {
            return GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads);
        }

        private static Java.IO.File GetExternalFilesDir(String path)
        {
            var f = Context.GetExternalFilesDir(path) ?? GetExternalDir("files", path);
            return f;
        }

        private static Java.IO.File GetExternalDir(String parentPath, String subPath)
        {
            var f = Android.OS.Environment.ExternalStorageDirectory;
            f = new Java.IO.File(f, "Android");
            f = new Java.IO.File(f, "data");
            f = new Java.IO.File(f, Context.PackageName);
            f = new Java.IO.File(f, parentPath);
            if (subPath != null)
            {
                f = new Java.IO.File(f, subPath);
            }
            if (!f.Exists()) f.Mkdirs();
            return f;
        }
    }
}