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
    
    public partial class PerTestCaseResult
    {
        public int ID { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> Test_ID { get; set; }
        public Nullable<int> Ques_ID { get; set; }
        public Nullable<int> Test_Case_ID { get; set; }
        public Nullable<int> Stud_Ans_ID { get; set; }
        public Nullable<int> Stud_ID { get; set; }
    
        public virtual Tbl_Prog_Ques Tbl_Prog_Ques { get; set; }
        public virtual Tbl_Stud_ProgTest_Ans Tbl_Stud_ProgTest_Ans { get; set; }
        public virtual Tbl_Prog_Test Tbl_Prog_Test { get; set; }
    }
}
