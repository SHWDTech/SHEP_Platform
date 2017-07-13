using System;

namespace VehicleDustMonitor.Xamarin.Component
{
    public class HttpResponseHandler
    {
        public Action<HttpRequestEventArgs> OnResponse { get; set; }

        public Action<HttpRequestEventArgs> OnError { get; set; }

        public void ProcessResponse(string response)
        {
            OnResponse?.Invoke(new HttpRequestEventArgs
            {
                Response = response
            });
        }

        public void ProcessError(Exception exception)
        {
            OnError?.Invoke(new HttpRequestEventArgs
            {
                Exception = exception
            });
        }
    }
}