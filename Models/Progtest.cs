using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizApps.Models
{
    public class Progtest
    {
        [Required]
        public int Testid { get; set; }
        [Required]
        [AllowHtml]
        [System.Web.Mvc.Remote("CheckExistingProgramingTest", "RemoteValidation", ErrorMessage = "Test Name already exists!")]
        public string TestName { get; set; }
        [Required]
        //[MaxLength(3, ErrorMessage = "Duration Cannot be more than 999")]
        public int? TestDuration { get; set; }
        public int? NoofQuestion { get; set; }
        public bool? HasQuestions { get; set; }

        public ProgrammingTests GetAllTests()
        {
            ProgrammingTests List1 = new ProgrammingTests();
            List1.ProgList = new List<Progtest>();
            using (var DB = new mocktestEntities1())
            {
                //var choice = DB.Tbl_Prog_Test.Where(x => x.Test_IsActive == true).ToList();
                var choice = (from test in DB.Tbl_Prog_Test
                              join noOfQuestions in DB.Tbl_Prog_Ques on test.Test_ID equals noOfQuestions.Test_ID into allQuestions
                              where (allQuestions.Where(post => post.Test_ID == test.Test_ID && post.Ques_IsActive == true && test.Test_IsActive == true).Count() > 0)
                              select test
                              ).ToList();
                foreach (var item in choice)
                {
                    QuestionOperation hasQuestions = CheckIfTestHasQuestions(item.Test_ID);
                    List1.ProgList.Add(new Progtest()
                    {
                        Testid = item.Test_ID,
                        TestName = item.Test_Name,
                        TestDuration = (item.Test_Duration),
                        NoofQuestion = hasQuestions.count,
                        HasQuestions = hasQuestions.hasQuestions
                    });
                }
                return List1;
            }
        }

        public Progtest GetTest(int testId)
        {
            Progtest theTest = new Progtest();

            using (var DB = new mocktestEntities1())
            {
                QuestionOperation hasQuestions = CheckIfTestHasQuestions(testId);
                var theTestFromDb = (from test in DB.Tbl_Prog_Test
                                     join noOfQues in DB.Tbl_Prog_Ques on test.Test_ID equals noOfQues.Test_ID into theQuesCount
                                     where test.Test_ID == testId
                                     select new
                                     {
                                         test,
                                         noOfQuestions = theQuesCount.Where(post => post.Test_ID == testId && post.Ques_IsActive == true).Count()
                                     }).SingleOrDefault();

                theTest.TestName = theTestFromDb.test.Test_Name;
                theTest.TestDuration = theTestFromDb.test.Test_Duration;
                theTest.NoofQuestion = theTestFromDb.noOfQuestions;
            }
            return theTest;
        }

        private QuestionOperation CheckIfTestHasQuestions(int test_ID)
        {
            QuestionOperation forNew = new QuestionOperation();
            using (var DB = new mocktestEntities1())
            {
                var list = DB.Tbl_Prog_Ques.Where(x => x.Test_ID == test_ID && x.Ques_IsActive == true).ToList();
                if (list.Count > 0)
                {
                    forNew.hasQuestions = true;
                    forNew.count = list.Count;
                    return forNew;
                }
                else
                {
                    forNew.hasQuestions = false;
                    forNew.count = 0;
                    return forNew;
                }
            }
        }
        public List<AllProgrammingTestsUserViewModel> GetAllTestsWithCheckIfUserHasGivenTest(string userEmail)
        {
            users theUser = new users();
            var userInfo = theUser.GetUserByEmail(userEmail);
            var AllTestsToShow = new List<AllProgrammingTestsUserViewModel>();
            Progtest theTests = new Progtest();
            var allTests = theTests.GetAllTests();
            var testsGivenByUser = theTests.GetAllTestsGivenByUser(userInfo.UserId);
            Progtest tests = new Progtest();
            foreach (var theTest in allTests.ProgList)
            {
                var theTestToDisplay = new AllProgrammingTestsUserViewModel();
                theTestToDisplay.TheTest = theTest;
                foreach (var UserGivenTest in testsGivenByUser)
                {
                    if (theTest.Testid == UserGivenTest.TheTest.Testid)
                    {
                        theTestToDisplay.HasGiven = true;
                        break;
                    }
                    else
                    {
                        theTestToDisplay.HasGiven = false   ;
                    }
                }
                AllTestsToShow.Add(theTestToDisplay);
            }
            return AllTestsToShow;
        }
        public List<AllProgrammingTestsUserViewModel> GetAllTestsGivenByUser(int userId)
        {
            List<Progtest> allTests = new List<Progtest>();

            using (var db = new mocktestEntities1())
            {
                return (from submittedTests in db.Tbl_Stud_ProgTest_Ans
                        join testData in db.Tbl_Prog_Test on submittedTests.Test_ID equals testData.Test_ID into TestData
                        join quesData in db.Tbl_Prog_Ques on TestData.FirstOrDefault().Test_ID equals quesData.Ques_ID into QuesData
                        where submittedTests.Stud_ID == userId && TestData.FirstOrDefault().Test_IsActive == true
                        select new AllProgrammingTestsUserViewModel()
                        {
                            TheTest = new Progtest()
                            {
                                TestName = TestData.FirstOrDefault().Test_Name,
                                HasQuestions = true,
                                NoofQuestion = QuesData.Count(),
                                TestDuration = TestData.FirstOrDefault().Test_Duration,
                                Testid = TestData.FirstOrDefault().Test_ID
                            },
                            HasGiven = true
                        }).Distinct().ToList();
            }

        }

    }

    public class ProgrammingTests
    {
        public List<Progtest> ProgList { get; set; }
    }
    public class ProgamingTestList
    {
        public List<Progtest> ProgList { get; set; }
        public Progtest newProgTest { get; set; }
    }
}