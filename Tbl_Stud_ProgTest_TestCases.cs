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
    
    public partial class Tbl_Stud_ProgTest_TestCases
    {
        public int Test_Case_ID { get; set; }
        public int Ques_ID { get; set; }
        public int Test_ID { get; set; }
        public string Test_Case_Input { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> Type { get; set; }
        public string Test_Case_Output { get; set; }
    
        public virtual Tbl_Prog_Ques Tbl_Prog_Ques { get; set; }
        public virtual Tbl_Prog_Test Tbl_Prog_Test { get; set; }
    }
}
