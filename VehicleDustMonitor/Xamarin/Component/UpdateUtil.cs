using Android.App;
using Android.Content;
using Android.OS;
using Java.IO;
using Java.Lang;
using VehicleDustMonitor.Xamarin.Model;
using VehicleDustMonitor.Xamarin.receiver;
using Console = System.Console;
using Exception = System.Exception;
using Uri = Android.Net.Uri;

namespace VehicleDustMonitor.Xamarin.Component
{
    public class UpdateUtil
    {
        public static UpdateUtil Instance { get; private set; }

        public static void Init(Context context)
        {
            if (Instance == null)
            {
                Instance = new UpdateUtil(context);
            }
        }

        private readonly Context _context;

        public VehicleAndroidVersionInfo VersionInfo { get; set; }

        private long _downloadId;

        private readonly DownloadManager _systemDownloadManager;

        private File _directory;

        public int CurrentVersionCode { get; }

        private UpdateUtil(Context context)
        {
            _context = context;
            CurrentVersionCode = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionCode;
            _directory = SdUtils.GetExternalDownloadDir();
            _systemDownloadManager = (DownloadManager) _context.GetSystemService(Context.DownloadService);
            var receiver = new DownloadReceiver();
            receiver.OnReceived += (context1, intent) =>
            {
                var action = intent.Action;
                if (DownloadManager.ActionDownloadComplete.Equals(action))
                {
                    var downloadId = intent.GetLongExtra(DownloadManager.ExtraDownloadId, 0);
                    if (downloadId == _downloadId && VersionInfo != null)
                    {
                        OnDownloadComplete(GetDownloadFile(_directory, VersionInfo));
                    }
                }
            };

            _context.RegisterReceiver(receiver, new IntentFilter(DownloadManager.ActionDownloadComplete));
        }

        private static Uri GetDownloadFile(File directory, VehicleAndroidVersionInfo info)
        {
            return Uri.FromFile(new File(directory, $"VehicleDustMonitor-{info.VersionName}.apk"));
        }

        private void OnDownloadComplete(Uri uri)
        {
            var intent = new Intent(Intent.ActionView);
            intent.SetFlags(ActivityFlags.NewTask);
            intent.SetDataAndType(uri, "application/vnd.android.package-archive");
            try
            {
                _context.StartActivity(intent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DoUpdate()
        {
            if (VersionInfo != null)
            {
                var request = new DownloadManager.Request(Uri.Parse(VersionInfo.ApkAddress));
                request.SetDestinationUri(GetDownloadFile(_directory, VersionInfo));
                try
                {
                    _downloadId = _systemDownloadManager.Enqueue(request);
                }
                catch (SecurityException)
                {
                    var f = Environment.ExternalStorageDirectory;
                    f = new File(f, "Android");
                    f = new File(f, "data");
                    f = new File(f, _context.PackageName);
                    f = new File(f, "files");
                    f = new File(f, Environment.DirectoryDownloads);
                    if (!f.Exists())
                    {
                        f.Mkdirs();
                    }
                    _directory = f;
                    request.SetDestinationUri(GetDownloadFile(_directory, VersionInfo));
                    _downloadId = _systemDownloadManager.Enqueue(request);
                }
            }
        }
    }
}