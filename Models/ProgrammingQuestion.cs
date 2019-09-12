using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApps.Models
{
    public class ProgrammingQuestion
    {
        public int QuestionID { get; set; }
        public string QuestionDescription { get; set; }
        public string Answer { get; set; }
        public int? ProgLanguageId { get; set; }
        public int? MaxMarks { get; set; }
        //Test Cases//
        public List<SecretTestCases> scerteTestCase;
        public List<SampleTestCases> sampleTestCase;


        public ProgrammingQuestion GetTheQuestionAnswerSubmitted(int questionId, int scoreId)
        {
            ProgrammingQuestion theSubmittedQuestionAnswer = new ProgrammingQuestion();

            using (var db = new mocktestEntities1())
            {
                theSubmittedQuestionAnswer = db.Tbl_Stud_ProgTest_Ans.Where(x => x.Ques_ID == questionId && x.Score_id == scoreId).Select(x => new ProgrammingQuestion()
                {
                    QuestionID = x.Ques_ID,
                    QuestionDescription = null,
                    Answer = x.Stud_Ans,
                    ProgLanguageId = x.Prog_Langid
                }).FirstOrDefault();
            }

            return theSubmittedQuestionAnswer;
        }

        public List<Tbl_Stud_ProgTest_Ans> GetAllQuestionAnswerSubmitted()
        {
            ProgrammingQuestion theSubmittedQuestionAnswer = new ProgrammingQuestion();

            using (var db = new mocktestEntities1())
            {
                return db.Tbl_Stud_ProgTest_Ans.ToList();
            }
        }

        public List<ProgrammingQuestion> GetAllQuestions()
        {
            using (var db = new mocktestEntities1())
            {
                return db.Tbl_Prog_Ques.
                    Select(x => new ProgrammingQuestion()
                    {
                        QuestionDescription = x.Ques_Desc,
                        QuestionID = x.Ques_ID,
                        MaxMarks = x.Max_Marks
                    }).ToList();
            }
        }
    }

    public class SecretTestCases
    {
        public int Secret_TestCaseID { get; set; }
        public int Question_ID { get; set; }
        public string TestCase_Input { get; set; }
        public string TestCase_Output { get; set; }
        public int TestID { get; set; }
        public bool? Secert_type { get; set; }
    }
    public class SampleTestCases
    {
        public int Question_ID { get; set; }
        public string TestCase_Input { get; set; }
        public string TestCase_Output { get; set; }
        public int TestID { get; set; }
        public bool? Sample_type { get; set; }
    }

}