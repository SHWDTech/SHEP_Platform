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
    
    public partial class DeviceException
    {
        public int Id { get; set; }
        public byte ExceptionType { get; set; }
        public int DevId { get; set; }
        public Nullable<int> StatId { get; set; }
        public Nullable<double> ExceptionValue { get; set; }
        public System.DateTime ExceptionTime { get; set; }
        public bool Processed { get; set; }
    }
}
