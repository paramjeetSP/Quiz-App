using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApps.Models.Program
{
    public class QuestionList
    {
        public int Ques_ID { get; set; }
        public int Test_ID { get; set; }
        public string Ques_Desc { get; set; }
        public Nullable<bool> Ques_IsActive { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> Max_Marks { get; set; }
        public string Test_Case_Description { get; set; }
        public string TCUpdatedBy { get; set; }
        public Nullable<System.DateTime> TCUpdatedOn { get; set; }
        public string TCCreatedBy { get; set; }
        public Nullable<System.DateTime> TCCreatedOn { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> Type { get; set; }
    }
}