//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DeviceExceptionChecker.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_ESHour
    {
        public long Id { get; set; }
        public int StatId { get; set; }
        public double TP { get; set; }
        public double DB { get; set; }
        public Nullable<double> PM25 { get; set; }
        public Nullable<double> PM100 { get; set; }
        public Nullable<double> VOCs { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public string DataStatus { get; set; }
        public int ValidDataNum { get; set; }
        public int DevId { get; set; }
        public string Country { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
        public double WindDirection { get; set; }
    }
}
