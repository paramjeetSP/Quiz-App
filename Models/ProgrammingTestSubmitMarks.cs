using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApps.Models
{
    public class ProgrammingTestSubmitMarks
    {
        private int userId;
        private int testId;
        private int questionId;
        private int scoreBoardId;
        private decimal marks;
        private string quesDescription;
        private string answer;
        private int? progLangId;
        private string progLangText;
        private int? maxMarks;
        public List <TestCases> testcase { get; set; }
        public bool submittedAnswer { get; set; }
        public int ScoreBoardId
        {
            get
            {
                return scoreBoardId;
            }

            set
            {
                scoreBoardId = value;
            }
        }

        public int QuestionId
        {
            get
            {
                return questionId;
            }

            set
            {
                questionId = value;
            }
        }

        public int TestId
        {
            get
            {
                return testId;
            }

            set
            {
                testId = value;
            }
        }

        public int UserId
        {
            get
            {
                return userId;
            }

            set
            {
                userId = value;
            }
        }

        public decimal Marks
        {
            get
            {
                return marks;
            }

            set
            {
                marks = value;
            }
        }

        public string QuesDescription
        {
            get
            {
                return quesDescription;
            }

            set
            {
                quesDescription = value;
            }
        }

        public string Answer
        {
            get
            {
                return answer;
            }

            set
            {
                answer = value;
            }
        }

        public int? Prog_LangId
        {
            get
            {
                return progLangId;
            }

            set
            {
                progLangId = value;
            }
        }

        public string ProgLangText
        {
            get
            {
                return progLangText;
            }

            set
            {
                progLangText = value;
            }
        }

        public int? MaxMarks
        {
            get
            {
                return maxMarks;
            }

            set
            {
                maxMarks = value;
            }
        }

        public bool SubmitMarks(ProgrammingTestSubmitMarks theData)
        {
            // TODO: Working On Submitting the Marks
            bool success = false;

            try
            {
                using (var db = new mocktestEntities1())
                {
                    var res = db.Tbl_Stud_ProgTest_Ans.Where(x => x.Ques_ID == theData.QuestionId && x.Test_ID == theData.TestId && x.Score_id == theData.ScoreBoardId).FirstOrDefault();
                    var updateScoreBoardMarking = db.Tbl_Stud_ProgTest_Result.Where(x => x.Score_ID == theData.ScoreBoardId).FirstOrDefault();
                    if (theData.Marks != null)
                    {
                        res.Marks = theData.Marks;
                        updateScoreBoardMarking.Is_Marked = true;
                        db.SaveChanges();
                    }
                    success = true;
                }
            }
            catch (Exception e)
            {
                success = false;
                throw;
            }

            return success;
        }

        public bool SubmitAllMarkings(List<ProgrammingTestSubmitMarks> listOfAllMarkings)
        {
            bool success = false;

            foreach (var item in listOfAllMarkings)
            {
                if (SubmitMarks(item))
                {
                    success = true;
                }
                else
                {
                    success = false;
                }
            }

            return success;
        }

        public bool CalculateProgramMarks()
        {
            return true;
        }
    }

    public class TestCases
    {
        public int TestCaseID { get; set; }
        public bool testCaseStatus { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public int questionId { get; set; }
    }
}