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
    
    public partial class T_Users_GetModel_Result
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Nullable<byte> Status { get; set; }
        public System.DateTime RegTime { get; set; }
        public int RoleId { get; set; }
        public Nullable<System.DateTime> LastTime { get; set; }
        public Nullable<System.DateTime> NowTime { get; set; }
        public string Remark { get; set; }
    }
}
