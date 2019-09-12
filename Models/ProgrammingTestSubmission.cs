using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApps.Models
{
    public class ProgrammingTestSubmission
    {
        private string userName;
        private string testName;
        private int scoreBoardId;
        private int testId;
        private int userId;
        private decimal? totalMarks;
        private bool? isMarked;

        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
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

        public decimal? TotalMarks
        {
            get
            {
                return totalMarks;
            }

            set
            {
                totalMarks = value;
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

        public List<ProgrammingTestSubmission> GetAllSubmissions(int testId)
        {
            List<ProgrammingTestSubmission> allTestSubmitted = new List<ProgrammingTestSubmission>();

            using (var db = new mocktestEntities1())
            {


                var scoreIds = db.Tbl_Stud_ProgTest_Ans.Select(x => x.Score_id).Distinct().ToList();

                ProgrammingQuestion getAllQuestions = new ProgrammingQuestion();
                var allQuestions = getAllQuestions.GetAllQuestions();

                users getAllUsers = new users();
                var allUsers = getAllUsers.GetAllUsers();

                Progtest getAllTests = new Progtest();
                ProgrammingTests allTests = new ProgrammingTests();
                allTests = getAllTests.GetAllTests();

                ProgrammingTestResult getResults = new ProgrammingTestResult();
                ProgrammingTestResults allResults = new ProgrammingTestResults();
                allResults = getResults.GetAllResults();

                allTestSubmitted = GiveProperListBack(scoreIds, allUsers, allTests, testId, allResults);
            }
            return allTestSubmitted;
        }

        public List<ProgrammingTestSubmitMarks> GetTheSubmission(int userId, int testId, int scoreBoardId)
        {
            ProgrammingLanguageInfo allLanguagesAndInfo = new ProgrammingLanguageInfo();
            List<ProgrammingLanguageInfo> allLanguages = new List<ProgrammingLanguageInfo>();
            allLanguages = allLanguagesAndInfo.GetAllProgrammingLanguagesInfo();
            using (var db = new mocktestEntities1())
            {
                /** Here we want to get the student name and test name, we get the tbl_progtest_ans which
               contains both the student id and test id for a particular scoreBoardId (tbl_progtest_result)
               we want to join on the tbl_prog_test and tbl_prog_ques to get botht the names we want
               **/
                var res = (from answerData in db.Tbl_Stud_ProgTest_Ans
                           join studData in db.users on answerData.Stud_ID equals studData.UserId into StudData
                           join testData in db.Tbl_Prog_Test on answerData.Test_ID equals testData.Test_ID into TestData
                           join quesData in db.Tbl_Prog_Ques on answerData.Ques_ID equals quesData.Ques_ID into QuesData
                           where answerData.Score_id == scoreBoardId && answerData.Test_ID == testId && answerData.Stud_ID == userId
                           select new ProgrammingTestSubmitMarks
                           {
                               UserId = StudData.FirstOrDefault().UserId,
                               TestId = TestData.FirstOrDefault().Test_ID,
                               ScoreBoardId = (int)answerData.Score_id,
                               QuestionId = answerData.Ques_ID,
                               QuesDescription = QuesData.FirstOrDefault().Ques_Desc,
                               Answer = answerData.Stud_Ans,
                               Marks = answerData.Marks,
                               Prog_LangId = answerData.Prog_Langid,
                               MaxMarks = QuesData.FirstOrDefault().Max_Marks
                               //ProgLangText = LangData.FirstOrDefault().language
                               //TestName = TestData.Select(x => new { x.Test_Name }).FirstOrDefault().Test_Name,
                               //UserName = StudData.Select(x => new { x.Name }).FirstOrDefault().Name
                           }).ToList();
                res = SetLanguageTextForThisResultSet(res, allLanguages);
                res = SetTestCasesForEachResultSet(res, userId);
                Console.WriteLine(res);
                return res;
            }
        }

        private List<ProgrammingTestSubmitMarks> SetLanguageTextForThisResultSet(List<ProgrammingTestSubmitMarks> res, List<ProgrammingLanguageInfo> allLanguages)
        {
            List<ProgrammingTestSubmitMarks> returnList = new List<ProgrammingTestSubmitMarks>();
            foreach (var item in res)
            {
                if (item.Prog_LangId != null)
                {
                    for (int i = 0; i < allLanguages.Count; i++)
                    {
                        if (item.Prog_LangId == allLanguages[i].id)
                        {
                            var itemToUpdate = item;
                            itemToUpdate.ProgLangText = allLanguages[i].language;
                            var insertThisUpdatedresultSet = itemToUpdate;
                            returnList.Add(insertThisUpdatedresultSet);
                        }
                    }
                }
            }

            return returnList;
        }

        private List<ProgrammingTestSubmission> MakeTheAboveDataConvertableToDestination(List<A> res)
        {
            throw new NotImplementedException();
        }

        private List<ProgrammingTestSubmission> GiveProperListBack(List<int?> theDistinctScorId, List<users> allUsers, ProgrammingTests allTests, int testId, ProgrammingTestResults allResults)
        {
            List<ProgrammingTestSubmission> allSubmissions = new List<ProgrammingTestSubmission>();
            using (var db = new mocktestEntities1())
            {
                foreach (var scoreId in theDistinctScorId)
                {
                    var rawQuery = db.Tbl_Stud_ProgTest_Ans.Where(x => x.Score_id == scoreId && x.Test_ID == testId);
                    var res = rawQuery.Distinct().FirstOrDefault();
                    if (res != null)
                    {
                        var user = allUsers.Where(x => x.UserId == res.Stud_ID).FirstOrDefault();
                        var test = allTests.ProgList.Where(x => x.Testid == testId).FirstOrDefault();
                        var results = allResults.allResults.Where(x => x.ScoreId == res.Score_id).FirstOrDefault();
                        if (user != null && test != null)
                        {
                            allSubmissions.Add(new ProgrammingTestSubmission
                            {
                                UserName = user.Name,
                                TestName = test.TestName,
                                ScoreBoardId = (int)res.Score_id,
                                TestId = test.Testid,
                                UserId = user.UserId,
                                TotalMarks = rawQuery.Sum(x => x.Marks),
                                IsMarked = results.IsMarked
                            });
                        }
                    }
                }
            }
            return allSubmissions;
        }

        public bool SubmitTestDuration(string duration, int scoreBoardId)
        {
            bool success = false;
            try
            {
                using (var db = new mocktestEntities1())
                {
                    var res = db.Tbl_Stud_ProgTest_Result.Where(x => x.Score_ID == scoreBoardId).FirstOrDefault();
                    res.Test_Duration = duration;
                    db.SaveChanges();
                }
                success = true;
            }
            catch (Exception)
            {
                success = false;
                throw;
            }
            return success;
        }

        private class A
        {
            public List<Tbl_Prog_Test> TestData;
            public List<user> UserData;
        }

        private List<ProgrammingTestSubmitMarks> SetTestCasesForEachResultSet(List<ProgrammingTestSubmitMarks> res, int userId)
        {
            
            using (var db = new mocktestEntities1())
            {
                foreach (var item in res)
                {
                    List<TestCases> newList = new List<TestCases>();
                    int testCasesCount = 0;
                    testCasesCount = db.PerTestCaseResults.Where(x => x.Ques_ID == item.QuestionId && x.Stud_ID == userId).Count();
                    if (testCasesCount !=0)
                    {
                        var testCases = db.PerTestCaseResults.Where(x => x.Ques_ID == item.QuestionId && x.Stud_ID == userId).ToList();
                        item.submittedAnswer = true;
                        foreach (var testCaseItems in testCases)
                        {
                           var result =  db.Tbl_Stud_ProgTest_TestCases.Where(x => x.Test_Case_ID == testCaseItems.Test_Case_ID).FirstOrDefault();
                            if (result != null)
                            {
                                TestCases perTestCase = new TestCases();
                                perTestCase.Input = result.Test_Case_Input;
                                perTestCase.Output = result.Test_Case_Output;
                                perTestCase.questionId = item.QuestionId;
                                perTestCase.testCaseStatus = (bool)testCaseItems.Status;
                                perTestCase.TestCaseID = result.Test_Case_ID;
                                newList.Add(perTestCase);
                            }
                        }
                    }
                    else
                    {
                        var testCasesNotAnswered = db.Tbl_Stud_ProgTest_TestCases.Where(x => x.Ques_ID == item.QuestionId && x.Type == true && x.IsActive == true).ToList();
                        item.submittedAnswer = false;
                        if(testCasesNotAnswered != null)
                        {
                            foreach (var testCaseItems in testCasesNotAnswered)
                            {
                                TestCases perTestCase = new TestCases();
                                perTestCase.Input = testCaseItems.Test_Case_Input;
                                perTestCase.Output = testCaseItems.Test_Case_Output;
                                perTestCase.questionId = item.QuestionId;
                                perTestCase.testCaseStatus = false;
                                perTestCase.TestCaseID = testCaseItems.Test_Case_ID;
                                newList.Add(perTestCase);
                            }
                        }
                    }
                    item.testcase = newList;
                }
            }
            return res;
        }
    }
}