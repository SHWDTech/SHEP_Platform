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
    
    public partial class T_TaskNotice
    {
        public long TaskId { get; set; }
        public int DevId { get; set; }
        public byte[] Data { get; set; }
        public Nullable<short> Length { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}
