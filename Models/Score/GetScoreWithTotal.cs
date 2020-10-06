using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApps.Models.Score
{
    public class GetScoreWithTotal
    {
        public string RollNo { get; set; }
        public string EmailId { get; set; }
        public string StudentName { get; set; }
        public string BranchName { get; set; }
        public string Score { get; set; }
        public string SubTopic1{ get; set; }
        public string SubTopic1Score { get; set; }
        public string SubTopic2 { get; set; }
        public string SubTopic2Score { get; set; }
    }
}