using QuizApps.Classes;
using QuizApps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizApps.Controllers
{
    [CustomExceptionFilter]
    public class ProgrammingTestController : Controller
    {
        // GET: ProgrammingTest
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["EmailId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Progtest tests = new Progtest();
            ProgrammingTests allTests = new ProgrammingTests();
            allTests = tests.GetAllTests();
            return View(allTests);
        }
        [HttpGet]
        public ActionResult AllProgrammingTests()
        {
            if (Session["EmailId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var userEmail = Session["EmailId"].ToString();
            Progtest tests = new Progtest();
            var GetAllTestsWithCheckIfUserHasGivenTest = tests.GetAllTestsWithCheckIfUserHasGivenTest(userEmail);
            return PartialView("_ProgrammingAllTest", GetAllTestsWithCheckIfUserHasGivenTest);
        }

        [HttpGet]
        public ActionResult GetTestData(int id)
        {
            ViewBag.TestId = id;
            Progtest test = new Progtest();
            var model = test.GetTest(id);
            return PartialView("_ProgrammingInstruct", test.GetTest(id));
        }

        [HttpGet]
        public ActionResult StartProgrammingTest(int id)
        {
            if (Session["EmailId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string userEmail = Session["EmailId"].ToString();
            
            ProgrammingTest theTest = new ProgrammingTest();
            if (theTest.CheckIfUserAlreadyGivenTest(userEmail, id))
            {
                return RedirectToAction("TakeQuiz", "Home");
            }

            var userInfo = theTest.GetUserInfo(userEmail);
            int scoreBoardId = theTest.StartTheTest(id, userInfo);
            //var testInfo = theTest.GetTestInfo(id);
            //theTest.TestName = testInfo.TestName;
            //theTest.TestID = id;
            //theTest.TestDuration= testInfo.TestDuration;
            theTest.SetTestQuestions(id);

            theTest.SetBasicTestInfo(id);
            theTest.SetProgrammingLanguages();
            theTest.ScoreBoardId = scoreBoardId;
            theTest.StudentID = userInfo.UserId;
            return View(theTest);
        }


        [HttpPost]
        public ActionResult SubmitOrUpdateQuestionAnswer(int currentID, ProgrammingTest theTest)
        {
            ProgrammingTest submitTheTest = new ProgrammingTest();
            //theTest.ScoreBoardId = 36;// TODO: Setting The Score_ID intentionally for testing purposes
            submitTheTest = theTest;
            int recordId = submitTheTest.UpdateOrAddAnswer(theTest);
            bool isFinalSubmit = submitTheTest.CheckDataInSecertTestTable(currentID, theTest.StudentID);
            var result = new { RecordId = recordId, IsFinalSubmit = isFinalSubmit };
            return Json(result, JsonRequestBehavior.AllowGet);
            //return Json(RecordId:recordId, IsFinalSubmit: isFinalSubmit);
        }

        [HttpGet]
        public ActionResult QuestionAnswerSubmitted(int QuestionID, int ScoreID)
        {
            ProgrammingQuestion theQuestionToReturn = new ProgrammingQuestion();
            var theData = theQuestionToReturn.GetTheQuestionAnswerSubmitted(QuestionID, ScoreID);
            return Json(theData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [CustomAdminPanelAuthorizationFilter]
        public ActionResult CheckTest()
        {
            if (Session["EmailId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ProgrammingTests allTests = new ProgrammingTests();
            Progtest getAllTests = new Progtest();
            allTests = getAllTests.GetAllTests();
            //ProgrammingTestSubmission getAllSub = new ProgrammingTestSubmission();
            //ViewBag.Submissions = getAllSub.GetAllSubmissions();
            return View(allTests);
        }
        [HttpGet]
        public ActionResult CheckTestSub(int userId,int testId, int scoreBoardId)
        {
            ProgrammingTestSubmission getAllSub = new ProgrammingTestSubmission();
            var theSubmissions = getAllSub.GetTheSubmission(userId, testId, scoreBoardId);
            return PartialView("_CheckTest", theSubmissions);
        }
        [HttpGet]
        public ActionResult GetAllSubmissions(int testId)
        {
            ProgrammingTestSubmission getAllSub = new ProgrammingTestSubmission();
            return PartialView("_ProgrammingTestSubmissions", getAllSub.GetAllSubmissions(testId));
        }

        [HttpPost]
        public ActionResult SubmitProgrammingMarks(List<ProgrammingTestSubmitMarks> data)
        {
            ProgrammingTestSubmitMarks submitAllMarks = new ProgrammingTestSubmitMarks();
            return Json(submitAllMarks.SubmitAllMarkings(data), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TestSubmitted()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitTestDuration(string duration, int scoreBoardId)
        {
            ProgrammingTestSubmission submitDuration = new ProgrammingTestSubmission();
            var res = submitDuration.SubmitTestDuration(duration, scoreBoardId);
            return Json(res);
        }
        //[HttpPost]
        //public ActionResult SubmitOrUpdateQuestionAnswerAll(ProgrammingTest theTest)
        //{

        //}
        [HttpPost]
        public ActionResult SubmitPerQuestion(FinalQuestionObject questionAfterEvaluation)
        {
            ProgrammingTest prg = new ProgrammingTest();
            bool isSubmitted = prg.SubmitForEvaluation(questionAfterEvaluation);
            return Json(isSubmitted);
        }
        [HttpPost]
        public ActionResult SumUpTestSubmitted (int Score_ID)
        {
            ProgrammingTest sumUp = new ProgrammingTest();
            bool Is_SumpCalculated = sumUp.CalculateSumOfMarks(Score_ID);
            return Json(Is_SumpCalculated);
        }
        [HttpPost]
        public ActionResult CheckSubmitFinalQuestions(int Test_ID, int StudentID)
        {
            ProgrammingTest checkfinalQuestion = new ProgrammingTest();
            int total_SubmitQuestion = checkfinalQuestion.checkFinalQuestionToSubmit(Test_ID, StudentID);
            return Json(total_SubmitQuestion);
        }
    }
}