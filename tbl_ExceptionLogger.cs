//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuizApps
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_ExceptionLogger
    {
        public int ID { get; set; }
        public string ExceptionMessage { get; set; }
        public string ControllerName { get; set; }
        public string ExceptionStackTrace { get; set; }
        public Nullable<System.DateTime> LogTime { get; set; }
        public string UserID { get; set; }
    }
}
