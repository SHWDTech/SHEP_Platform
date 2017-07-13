﻿using System.Net;
using System.Text;

namespace VehicleDustMonitor.Xamarin.Component
{
    public class HttpRequestAsyncState
    {
        public HttpWebRequest Request { get; }

        public StringBuilder BodyParamters { get; }

        public HttpResponseHandler Handler { get; }

        public HttpRequestAsyncState(HttpWebRequest request, StringBuilder bodyParamters, HttpResponseHandler handler)
        {
            Request = request;
            BodyParamters = bodyParamters;
            Handler = handler;
        }
    }
}