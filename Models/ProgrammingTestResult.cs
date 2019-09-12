using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApps.Models
{
    public class ProgrammingTestResult
    {
        private int scoreId;
        private int? studentId;
        private bool? isMarked;
        private string testDuration;

        public int ScoreId
        {
            get
            {
                return scoreId;
            }

            set
            {
                scoreId = value;
            }
        }

        public int? StudentId
        {
            get
            {
                return studentId;
            }

            set
            {
                studentId = value;
            }
        }

        public bool? IsMarked
        {
            get
            {
                return isMarked;
            }

            set
            {
                isMarked = value;
            }
        }

        public string TestDuration
        {
            get
            {
                return testDuration;
            }

            set
            {
                testDuration = value;
            }
        }

        public ProgrammingTestResults GetAllResults()
        {
            var allResults = new ProgrammingTestResults();
            using (var db = new mocktestEntities1())
            {
                allResults.allResults = db.Tbl_Stud_ProgTest_Result.Select(x => new ProgrammingTestResult {
                    ScoreId = x.Score_ID,
                    TestDuration = x.Test_Duration,
                    StudentId = x.Stud_ID,
                    IsMarked = x.Is_Marked
                }).ToList();
            }
            return allResults;
        }
    }

    public class ProgrammingTestResults
    {
        public List<ProgrammingTestResult> allResults;
    }
}