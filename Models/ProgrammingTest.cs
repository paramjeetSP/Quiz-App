using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace QuizApps.Models
{
    public class ProgrammingTest
    {


        private int studentID;
        private int testID;
        private string testName;
        private int? testDuration;
        private float? timeTaken;
        private List<ProgrammingQuestion> questionsList;
        private List<ProgrammingTestLanguages> programmingLanguages;
        private int? pTestId;
        private int? scoreBoardID;


        public int StudentID
        {
            get
            {
                return studentID;
            }

            set
            {
                studentID = value;
            }
        }

        public int TestID
        {
            get
            {
                return testID;
            }

            set
            {
                testID = value;
            }
        }

        public string TestName
        {
            get
            {
                return testName;
            }

            set
            {
                testName = value;
            }
        }

        public int? TestDuration
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

        public float? TimeTaken
        {
            get
            {
                return timeTaken;
            }

            set
            {
                timeTaken = value;
            }
        }

        public List<ProgrammingQuestion> QuestionsList
        {
            get
            {
                return questionsList;
            }

            set
            {
                questionsList = value;
            }
        }

        public List<ProgrammingTestLanguages> ProgrammingLanguages
        {
            get
            {
                return programmingLanguages;
            }

            set
            {
                programmingLanguages = value;
            }
        }

        public int? PTestId
        {
            get
            {
                return pTestId;
            }

            set
            {
                pTestId = value;
            }
        }

        public int? ScoreBoardId
        {
            get
            {
                return scoreBoardID;
            }

            set
            {
                scoreBoardID = value;
            }
        }


        //public void SetTestQuestions(int testId)
        //{
        //    List<ProgrammingQuestion> theQuesList = new List<ProgrammingQuestion>();

        //    using (var db = new mocktestEntities1())
        //    {
        //        theQuesList = db.Tbl_Prog_Ques.Where(x => x.Test_ID == testId && x.Ques_IsActive == true).Select(x => new ProgrammingQuestion
        //        {
        //            QuestionID = x.Ques_ID,
        //            QuestionDescription = x.Ques_Desc,
        //            Answer = null,
        //            MaxMarks = x.Max_Marks
        //        }).ToList();
        //    }
        //    questionsList = theQuesList;
        //}

        public void SetTestQuestions(int testId)
        {
            //List<ProgrammingQuestion> theQuesList = new List<ProgrammingQuestion>();
            List<Tbl_Prog_Ques> theQuesList = new List<Tbl_Prog_Ques>();

            using (var db = new mocktestEntities1())
            {
                theQuesList = db.Tbl_Prog_Ques.Where(x => x.Test_ID == testId && x.Ques_IsActive == true).Include(x => x.Tbl_Stud_ProgTest_TestCases).ToList();

            }
            questionsList = theQuesList.Select(x => new ProgrammingQuestion
            {
                QuestionID = x.Ques_ID,
                QuestionDescription = x.Ques_Desc,
                Answer = null,
                MaxMarks = x.Max_Marks,
                scerteTestCase = x.Tbl_Stud_ProgTest_TestCases.Where(y => y.IsActive == true && y.Type == true).Select(sc => new SecretTestCases
                {
                    Question_ID = sc.Ques_ID,
                    TestCase_Input = sc.Test_Case_Input,
                    TestCase_Output = sc.Test_Case_Output,
                    TestID = sc.Test_ID,
                    Secert_type = sc.Type,
                    Secret_TestCaseID = sc.Test_Case_ID,

                }).ToList(),

                sampleTestCase = x.Tbl_Stud_ProgTest_TestCases.Where(y => y.IsActive == true && y.Type == false).Select(sp => new SampleTestCases
                {
                    Question_ID = sp.Ques_ID,
                    TestCase_Input = sp.Test_Case_Input,
                    TestCase_Output = sp.Test_Case_Output,
                    TestID = sp.Test_ID,
                    Sample_type = sp.Type,

                }).ToList(),

            }).ToList(); ;
        }

        public void SetBasicTestInfo(int testId)
        {
            Progtest theTest = new Progtest();
            using (var db = new mocktestEntities1())
            {
                theTest = db.Tbl_Prog_Test.Where(x => x.Test_ID == testId && x.Test_IsActive == true).Select(x => new Progtest
                {
                    TestName = x.Test_Name,
                    TestDuration = x.Test_Duration,
                    Testid = x.Test_ID
                }).FirstOrDefault();
            }
            testName = theTest.TestName;
            testDuration = theTest.TestDuration;
            testID = theTest.Testid;
        }

        public void SetProgrammingLanguages()
        {
            ProgrammingLanguages = new List<ProgrammingTestLanguages>();
            ProgrammingTestLanguages theLang = new ProgrammingTestLanguages();
            theLang.Name = "C#";
            theLang.Slug = "c-sharp";
            theLang.SampleProgram = GetProgram("c#");
            ProgrammingLanguages.Add(theLang);
            theLang = new ProgrammingTestLanguages();
            theLang.Name = "Javascript";
            theLang.Slug = "javascript";
            theLang.SampleProgram = GetProgram("javascript");
            ProgrammingLanguages.Add(theLang);
            theLang = new ProgrammingTestLanguages();
            theLang.Name = "NodeJS";
            theLang.Slug = "node-js";
            theLang.SampleProgram = GetProgram("nodejs");
            ProgrammingLanguages.Add(theLang);
            theLang = new ProgrammingTestLanguages();
            theLang.Name = "Php";
            theLang.Slug = "php";
            theLang.SampleProgram = GetProgram("php");
            ProgrammingLanguages.Add(theLang);
            theLang = new ProgrammingTestLanguages();
            theLang.Name = "Swift";
            theLang.Slug = "swift";
            theLang.SampleProgram = GetProgram("swift");
            ProgrammingLanguages.Add(theLang);
            theLang = new ProgrammingTestLanguages();
            theLang.Name = "Java";
            theLang.Slug = "java";
            theLang.SampleProgram = GetProgram("java");
            ProgrammingLanguages.Add(theLang);
            theLang = new ProgrammingTestLanguages();
            theLang.Name = "C++";
            theLang.Slug = "cpp";
            theLang.SampleProgram = GetProgram("cpp");
            ProgrammingLanguages.Add(theLang);

        }

        private string GetProgram(string lang)
        {
            string program = @"";
            if (lang == "c#")// get c# progrma
            {
                program = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rextester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(""Hello, world!"");
        }
    }
}";
            }
            else if (lang == "javascript")// get javascript program
            {
                program = @"print(""Hello, world!"")";
            }
            else if (lang == "nodejs")// get nodejs program
            {
                program = @"console.log(""Hello, World!"");";
            }
            else if (lang == "php")// get php program
            {
                program = @"<?php

    echo ""Hello, world!""

?> ";
            }
            else if (lang == "swift")// get swift program
            {
                program = @"print(""Hello, world!"")";
            }
            else if (lang == "cpp")
            {
                program = @"#include <iostream>
using namespace std;

int main()
{
    std::cout << ""Hello, world!\n"";
}
            ";
            }
            else if (lang == "java")// get java program
            {
                program = @"import java.util.*;
import java.lang.*;

class Rextester
{  
    public static void main(String args[])
    {
        System.out.println(""Hello, World!"");
    }
        }";
            }
            return program;
        }


        public bool SubmitAllAnswers(ProgrammingTest theTest)
        {
            bool dataSubmittedSuccess = false;
            using (var db = new mocktestEntities1())
            {
                if (theTest.questionsList != null)
                {

                }
            }
            return dataSubmittedSuccess;
        }

        public int UpdateOrAddAnswer(ProgrammingTest theTest)
        {
            if (theTest.questionsList == null)// If no answer to update
            {
                return 0;
            }
            if (!QuestionAnswerAdded(theTest))//QuestionAnswerAdded when false 
            {
                return SubmitAnswer();// As we will have just 1 question to add in list
            }
            int answerUpdated = 0;

            return UpdateAnswer();
        }

        private int SubmitAnswer()
        {
            int submittedAns = 0;
            int updatedAns = 0;
            int questionIdL = questionsList[0].QuestionID;
            int scoreBoardIdL = (int)this.scoreBoardID;
            Tbl_Stud_ProgTest_Ans questionAnswer = new Tbl_Stud_ProgTest_Ans();
            questionAnswer.CreatedBy = GetUserInfo().Name;// This will work if the StudentId has already been set
            questionAnswer.CreatedOn = DateTime.Now;
            questionAnswer.IsActive = true;
            questionAnswer.Ques_ID = questionsList[0].QuestionID;
            questionAnswer.Score_id = scoreBoardID;
            questionAnswer.Stud_Ans = questionsList[0].Answer;
            questionAnswer.Stud_ID = studentID;
            questionAnswer.Test_ID = testID;
            questionAnswer.Prog_Langid = questionsList[0].ProgLanguageId;

            try
            {

                using (var db = new mocktestEntities1())
                {
                    var res = db.Tbl_Stud_ProgTest_Ans.Add(questionAnswer);
                    db.SaveChanges();
                    submittedAns = res.Stud_Ans_ID;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine(@"Entity of type ""{0}"" in state ""{1}"" 
                   has the following validation errors:",
                        eve.Entry.Entity.GetType().Name,
                        eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine(@"- Property: ""{0}"", Error: ""{1}""",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            return submittedAns;
        }

        public user GetUserInfo(string userEmail)
        {
            using (var db = new mocktestEntities1())
            {
                var res = db.users.Where(x => x.EmailId == userEmail).FirstOrDefault();

                return res;
            }
        }
        public user GetUserInfo()
        {
            using (var db = new mocktestEntities1())
            {
                var res = db.users.Where(x => x.UserId == studentID).FirstOrDefault();

                return res;
            }
        }

        private int UpdateAnswer()
        {
            int updatedAns = 0;
            int questionIdL = questionsList[0].QuestionID;
            int scoreBoardIdL = (int)this.scoreBoardID;
            try
            {
                using (var db = new mocktestEntities1())
                {
                    // Working on updating the question answer based on QID, UID, ScoreBoardID
                    var res = db.Tbl_Stud_ProgTest_Ans.Where(x => x.Ques_ID == questionIdL && x.Stud_ID == studentID && x.Score_id == scoreBoardIdL).FirstOrDefault();
                    res.Stud_Ans = questionsList[0].Answer;
                    res.Prog_Langid = questionsList[0].ProgLanguageId;
                    db.SaveChanges();
                    updatedAns = res.Stud_Ans_ID;
                }
            }
            catch (Exception)
            {
                updatedAns = 0;
                throw;
            }

            return updatedAns;
        }

        public int StartTheTest(int testID, user userInfo)
        {
            Tbl_Stud_ProgTest_Result theResBoard = new Tbl_Stud_ProgTest_Result();
            theResBoard.CreatedBy = userInfo.Name;
            theResBoard.UpdatedBy = userInfo.Name;
            theResBoard.CreatedOn = DateTime.Now;
            theResBoard.IsActive = true;
            theResBoard.Stud_ID = userInfo.UserId;

            using (var db = new mocktestEntities1())
            {
                var res = db.Tbl_Stud_ProgTest_Result.Add(theResBoard);
                db.SaveChanges();
                scoreBoardID = theResBoard.Score_ID;
            }
            return (int)scoreBoardID;
        }

        public Progtest GetTestInfo(int id)
        {
            using (var db = new mocktestEntities1())
            {
                var res = (from test in db.Tbl_Prog_Test
                           join noOfQues in db.Tbl_Prog_Ques on test.Test_ID equals noOfQues.Test_ID into theQuesCount
                           where test.Test_ID == id
                           select new Progtest()
                           {
                               TestName = test.Test_Name,
                               TestDuration = test.Test_Duration,
                               Testid = test.Test_ID,
                               NoofQuestion = theQuesCount.Where(post => post.Test_ID == id && post.Ques_IsActive == true).Count()
                           }).SingleOrDefault();
                return res;
            }
        }

        public List<Progtest> GetAllTests()
        {
            using (var db = new mocktestEntities1())
            {
                return (from test in db.Tbl_Prog_Test
                        join noOfQues in db.Tbl_Prog_Ques on test.Test_ID equals noOfQues.Test_ID into theQuesCount
                        select new Progtest()
                        {
                            TestName = test.Test_Name,
                            TestDuration = test.Test_Duration,
                            Testid = test.Test_ID,
                            NoofQuestion = theQuesCount.Count()
                        }).ToList();
            }
        }

        private bool QuestionAnswerAdded(ProgrammingTest theTest)
        {
            bool questinAddedBefore = false;
            int QuestionID = (int)theTest.QuestionsList[0].QuestionID;
            using (var db = new mocktestEntities1())
            {
                var res = db.Tbl_Stud_ProgTest_Ans.Where(x => x.Score_id == theTest.ScoreBoardId && x.Ques_ID == QuestionID).FirstOrDefault();
                if (res != null)
                {
                    questinAddedBefore = true;
                }
            }
            return questinAddedBefore;
        }

        public bool CheckIfUserAlreadyGivenTest(string email, int testId)
        {
            users user = new users();
            return user.CheckIfUserHasGivenTest(email, testId);
        }

        //Submit for Evaluation
        public bool SubmitForEvaluation(FinalQuestionObject questionAfterEvaluation)
        {
            Tbl_Stud_ProgTest_Ans Answers = new Tbl_Stud_ProgTest_Ans();
            PerTestCaseResult testCaseResult = new PerTestCaseResult();
            bool questinAddedBefore = false;
            int AnswID = 0;
            int Stud_ID = 0;
            try
            {
                using (var db = new mocktestEntities1())
                {
                    var res = db.Tbl_Stud_ProgTest_Ans.Where(x => x.Score_id == questionAfterEvaluation.QuestionDetailsObject.ScoreBoardID && x.Ques_ID == questionAfterEvaluation.QuestionDetailsObject.questionId).FirstOrDefault();
                    if (res != null)
                    {
                        questinAddedBefore = true;
                    }
                }
                if (!questinAddedBefore)
                {

                    using (var db = new mocktestEntities1())
                    {
                        //var questionsAddedOrNot = db.Tbl_Prog_Ques.Add(ques);
                        Answers.Test_ID = questionAfterEvaluation.QuestionDetailsObject.testID;
                        Answers.Marks = questionAfterEvaluation.QuestionDetailsObject.QuestionMarksAsPerStatus;
                        Answers.Prog_Langid = questionAfterEvaluation.QuestionDetailsObject.TestLanguageId;
                        Answers.Ques_ID = questionAfterEvaluation.QuestionDetailsObject.questionId;
                        Answers.Score_id = questionAfterEvaluation.QuestionDetailsObject.ScoreBoardID;
                        Answers.Stud_Ans = questionAfterEvaluation.QuestionDetailsObject.answer;
                        Answers.Stud_ID = questionAfterEvaluation.QuestionDetailsObject.StudentID;
                        Answers.CreatedOn = DateTime.Now;
                        Answers.IsActive = true;
                        db.Tbl_Stud_ProgTest_Ans.Add(Answers);
                        db.SaveChanges();
                        AnswID = Answers.Stud_Ans_ID;
                        Stud_ID = Answers.Stud_ID;
                    }
                }
                else
                {
                    using (var db = new mocktestEntities1())
                    {
                        // Working on updating the question answer based on QID, UID, ScoreBoardID
                        var res = db.Tbl_Stud_ProgTest_Ans.Where(x => x.Ques_ID == questionAfterEvaluation.QuestionDetailsObject.questionId && x.Stud_ID == questionAfterEvaluation.QuestionDetailsObject.StudentID && x.Score_id == questionAfterEvaluation.QuestionDetailsObject.ScoreBoardID).FirstOrDefault();
                        res.Stud_Ans = questionAfterEvaluation.QuestionDetailsObject.answer;
                        res.Prog_Langid = questionAfterEvaluation.QuestionDetailsObject.TestLanguageId;
                        res.Marks = questionAfterEvaluation.QuestionDetailsObject.QuestionMarksAsPerStatus; 
                        db.SaveChanges();
                        AnswID = res.Stud_Ans_ID;
                        Stud_ID = res.Stud_ID;
                    }
                }
                foreach (var item in questionAfterEvaluation.arrOfSecretTestCases)
                {
                    using (var db1 = new mocktestEntities1())
                    {
                        testCaseResult.Ques_ID = item.questionId;
                        testCaseResult.Stud_Ans_ID = AnswID;
                        testCaseResult.Test_Case_ID = item.TestCaseID;
                        testCaseResult.Test_ID = questionAfterEvaluation.QuestionDetailsObject.testID;
                        testCaseResult.Status = item.testCaseStatus;
                        testCaseResult.Stud_ID = Stud_ID;
                        db1.PerTestCaseResults.Add(testCaseResult);
                        db1.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }

        public bool CheckDataInSecertTestTable(int QuesID, int Stud_ID)
        {
            bool isFinalSubmit = false;
            try
            {
                using (var db1 = new mocktestEntities1())
                {
                    var res = db1.PerTestCaseResults.Where(x =>  x.Ques_ID == QuesID && x.Stud_ID == Stud_ID).FirstOrDefault();
                    if (res != null)
                    {
                        isFinalSubmit = true;
                    }
                }
                return isFinalSubmit;
            }
            catch (Exception ex)
            {
                return isFinalSubmit;
            }
        }


        public bool CalculateSumOfMarks(int Score_ID)
        {
            int sumOFMarks = 0;
            try
            {
                using ( var db = new mocktestEntities1())
                {
                    var res = db.Tbl_Stud_ProgTest_Ans.Where(x => x.Score_id == Score_ID).FirstOrDefault();
                    if(res != null)
                    {
                        var marksLists = db.Tbl_Stud_ProgTest_Ans.Where(x => x.Score_id == Score_ID).ToList();
                        sumOFMarks = (int)marksLists.Sum(x => x.Marks);
;                    }
                }
                using (var db1 = new mocktestEntities1())
                {
                    var res1 = db1.Tbl_Stud_ProgTest_Result.Where(x => x.Score_ID == Score_ID).FirstOrDefault();
                    if (res1 != null)
                    {
                        var res2 = db1.Tbl_Stud_ProgTest_Result.Where(x => x.Score_ID == Score_ID).FirstOrDefault();
                        res2.Is_Marked = true;
                        res2.Stud_Score = sumOFMarks;
                        db1.SaveChanges();
                    }
                }
                return true;

            }
            catch(Exception ex)
            {
                return false;
            }
          
        }

        public int checkFinalQuestionToSubmit(int Test_ID, int StudentID)
        {
            int total_SubmitQuestion = 0;
            try
            {
                using (var db = new mocktestEntities1())
                {
                    var countofquestions = db.PerTestCaseResults.Where(x => x.Stud_ID == StudentID && x.Test_ID == Test_ID);
                    if(countofquestions != null)
                    {
                        total_SubmitQuestion =  countofquestions.Select(x=>x.Ques_ID).Distinct().Count();
                    }
                }
                return total_SubmitQuestion;
            }
            catch(Exception ex)
            {
                return total_SubmitQuestion;
            }
        }

    }



    public class SubmitProgrammingQuestion
    {
        public int questionId { get; set; }
        public string answer { get; set; }
        public int QuestionMarksAsPerStatus { get; set; }
        public int TestLanguageId { get; set; }
        public int StudentID { get; set; }
        public int ScoreBoardID { get; set; }
        public int testID { get; set; }
    }
    public class SubmitTestCase
    {
        public int TestCaseID { get; set; }
        public bool testCaseStatus { get; set; }
        public int questionId { get; set; }
    }
    public class FinalQuestionObject
    {
        public SubmitProgrammingQuestion QuestionDetailsObject { get; set; }
        public List<SubmitTestCase> arrOfSecretTestCases { get; set; }
    }
}