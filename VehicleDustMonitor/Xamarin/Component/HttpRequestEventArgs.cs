using System;

namespace VehicleDustMonitor.Xamarin.Component
{
    public class HttpRequestEventArgs
    {
        public string Response { get; set; }

        public string Error { get; set; }

        public Exception Exception { get; set; }
    }
}