using ClosedXML.Excel;
using Excel;
using Newtonsoft.Json;
using QuizApps.Classes;
using QuizApps.Models;
using QuizApps.Models.Methods;
using QuizApps.Models.Program;
using QuizApps.Models.quiz;
using QuizApps.Models.Score;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace QuizApps.Controllers
{

    [CustomExceptionFilter]
    public class HomeController : Controller
    {
        // GET: Home
        mocktestEntities1 db;
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult TakeQuiz()
        {
            if (Session["EmailId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var userEmail = Session["EmailId"].ToString();
            Progtest tests = new Progtest();
            var GetAllTestsWithCheckIfUserHasGivenTest = tests.GetAllTestsWithCheckIfUserHasGivenTest(userEmail);
            TakeAllTestsIntro allTests = new TakeAllTestsIntro();
            allTests.ProgrammingTests = GetAllTestsWithCheckIfUserHasGivenTest;
            return View(allTests);
        }
        public ActionResult Instruction(Int32 subId)
        {
            TempData["SubId"] = subId;
            TempData.Keep("SubId");
            return PartialView("~/Views/Home/Instruction.cshtml");
        }
        [HttpGet]
        public ActionResult StartQuiz()
        {
            if (Session["EmailId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult StartQuiz(takeQuiz quiz)
        {
            return View();
        }
        public ActionResult GetScore(Int32 correctAnswers, string totalTime, DateTime today, Int32 currentQuestion, Int32 quesId, int? subId, bool isAutoSubmitted)
        {
            Int32 uid = 0;
            mocktestEntities1 db = new mocktestEntities1();
            if (Session["UserId"] != null)
            {
                uid = Convert.ToInt32(Session["UserId"]);
                var alreadyGiven = db.ScoreDetails.Where(x => x.UserId == uid && x.SubID == subId).Count();
                if (alreadyGiven == 0)
                {
                    ScoreDetail newscore = new ScoreDetail();
                    newscore.QuesDetailId = quesId;
                    newscore.UserId = uid;
                    newscore.Corrected = correctAnswers;
                    newscore.date = today;
                    newscore.Duration = totalTime;
                    newscore.Score = correctAnswers;
                    newscore.Attempted = currentQuestion;
                    newscore.Active = true;
                    newscore.SubID = subId;
                    db.ScoreDetails.Add(newscore);
                    db.SaveChanges();

                }
                //else
                //{
                //    ScoreDetail newscore = new ScoreDetail();
                //    newscore.QuesDetailId = quesId;
                //    newscore.UserId = uid;
                //    newscore.Corrected = correctAnswers;
                //    newscore.date = today;
                //    newscore.Duration = totalTime;
                //    newscore.Score = correctAnswers;
                //    newscore.Attempted = currentQuestion;
                //    newscore.Active = true;
                //    newscore.SubID = subId;
                //    db.ScoreDetails.Add(newscore);
                //    db.SaveChanges();

                //}
            }
            if (isAutoSubmitted)
            {
                AutoSubmittedDetail submittedDetail = new AutoSubmittedDetail();
                submittedDetail.SubTopicId = subId;
                submittedDetail.UserId = uid;
                submittedDetail.Reason = "User leave the test";
                db.AutoSubmittedDetails.Add(submittedDetail);
                db.SaveChanges();
            }
            var GivenTestCount = db.ScoreDetails.Where(x => x.UserId == uid).Count();
            SubmitTest submitTest = new SubmitTest();
            submitTest.TotalTestSubmitted = GivenTestCount;
            return Json(submitTest, JsonRequestBehavior.AllowGet);
            // return JsonConvert.SerializeObject(result, Formatting.Indented);
            // return PartialView("~/Views/Home/GetScore.cshtml");
        }
        [HttpGet]
        [CustomAdminPanelAuthorizationFilter]
        public ActionResult UserList()
        {
            return View();
        }
        [HttpGet]
        public ActionResult UserArea()
        {
            scoreDetails obj = new scoreDetails();
            using (mocktestEntities1 selectStatement = new mocktestEntities1())
            {
                var userList = selectStatement.QuesDetails.
                    Join(selectStatement.ScoreDetails, ques => ques.QuesDetailId, score => score.QuesDetailId, (ques, score) => new { ques, score })
                    .Join(selectStatement.SubTopics, a => a.ques.SubTopicId, sub => sub.SubTopicId, (a, sub) => new { a, sub })
                    .Join(selectStatement.Topics, b => b.sub.TopicId, top => top.TopicId, (b, top) => new { b, top })
                    .Join(selectStatement.users, c => c.b.a.score.UserId, us => us.UserId, (c, us) => new { c, us }).Where(a => a.c.b.a.ques.Active == true).ToList();
                obj.scoreGrid = userList.Select(c => new GetScore
                {
                    today = c.c.b.a.score.date,
                    user = c.us.Name,
                    RollNo = c.us.RollNo,
                    Branch = c.us.Branch,
                    topicname = c.c.top.Name,
                    subname = c.c.b.sub.Name,
                    score = Convert.ToInt32(c.c.b.a.score.Score),
                    Attempted = c.c.b.a.score.Attempted,
                    correctAnswers = c.c.b.a.score.Corrected,
                    totalTime = c.c.b.a.score.Duration,
                    TestType = 1, // This type is to filter the results on the User Score Board, it is not stored in DB
                    MobNo = c.us.Mob,
                    Gender = c.us.Gender,
                    Email = c.us.EmailId,
                    CllgeName = c.us.CllgeName,
                    FName=c.us.FName,
                    LName = c.us.LName
                }).OrderByDescending(x=>x.score).ToList();
                obj.scoreGrid = obj.scoreGrid;
                obj.scoreGrid = GetProgrammingTestData(obj.scoreGrid);
                return Json(obj.scoreGrid, JsonRequestBehavior.AllowGet);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public FileResult ExporttoExport()
        {
            SqlConnection cnn;
            DataTable table = new DataTable("ExportScoreReport");
            string Connectionstring = ConfigurationManager.ConnectionStrings["ScoreReport"].ConnectionString; //@"Data Source=192.168.0.78;Initial Catalog=Mocktest_SP;User ID=soft;Password=@dm!n!$tr@t0r";
            cnn = new SqlConnection(Connectionstring);
            cnn.Open();
            using (var cmd = new SqlCommand("[dbo].[sp_ScoreReport]", cnn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            cnn.Close();

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(table);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SecoreReport.xlsx");
                }
            }
        }
        [HttpGet]
        public ActionResult UserAreaProgrammingTest()
        {
            scoreDetails obj = new scoreDetails();
            obj.scoreGrid = obj.scoreGrid;
            obj.scoreGrid = GetProgrammingTestData(obj.scoreGrid);
            return PartialView("_UserListQuizTest", obj.scoreGrid);
        }
        [HttpGet]
        public ActionResult UserAreaQuizTest()
        {
            scoreDetails obj = new scoreDetails();
            using (mocktestEntities1 selectStatement = new mocktestEntities1())
            {
                var userList = selectStatement.QuesDetails.
                    Join(selectStatement.ScoreDetails, ques => ques.QuesDetailId, score => score.QuesDetailId, (ques, score) => new { ques, score })
                    .Join(selectStatement.SubTopics, a => a.ques.SubTopicId, sub => sub.SubTopicId, (a, sub) => new { a, sub })
                    .Join(selectStatement.Topics, b => b.sub.TopicId, top => top.TopicId, (b, top) => new { b, top })
                    .Join(selectStatement.users, c => c.b.a.score.UserId, us => us.UserId, (c, us) => new { c, us }).Where(a => a.c.b.a.ques.Active == true).ToList();
                obj.scoreGrid = userList.Select(c => new GetScore
                {
                    Id = c.c.b.a.score.ScoreDetailsId,
                    today = c.c.b.a.score.date,
                    user = c.us.Name,
                    RollNo = c.us.RollNo,
                    Branch = c.us.Branch,
                    topicname = c.c.top.Name,
                    subname = c.c.b.sub.Name,
                    score = Convert.ToInt32(c.c.b.a.score.Score),
                    Attempted = c.c.b.a.score.Attempted,
                    correctAnswers = c.c.b.a.score.Corrected,
                    totalTime = c.c.b.a.score.Duration,
                    TestType = 1 // This type is to filter the results on the User Score Board, it is not stored in DB
                }).OrderByDescending(x => x.Id).ToList();
                obj.scoreGrid = obj.scoreGrid;
                return PartialView("_UserListQuizTest", obj.scoreGrid);
            }
        }
        private List<GetScore> GetProgrammingTestData(List<GetScore> scoreGrid)
        {
            if (scoreGrid == null)
            {
                scoreGrid = new List<Models.Score.GetScore>();
            }
            List<GetScore> progTest = new List<GetScore>();
            using (var db = new mocktestEntities1())
            {
                var progList = (from answers in db.Tbl_Stud_ProgTest_Ans
                                join studData in db.users on answers.Stud_ID equals studData.UserId into StudData
                                join allAnswers in db.Tbl_Stud_ProgTest_Ans on answers.Score_id equals allAnswers.Score_id into AllAnswers
                                join testData in db.Tbl_Prog_Test on answers.Test_ID equals testData.Test_ID into TestData
                                join scoreBoardData in db.Tbl_Stud_ProgTest_Result on answers.Score_id equals scoreBoardData.Score_ID into ScoreBoardData
                                select new GetScore()
                                {
                                    Id = ScoreBoardData.FirstOrDefault().Score_ID,
                                    user = StudData.FirstOrDefault().Name,
                                    Branch = StudData.FirstOrDefault().Branch,
                                    Attempted = AllAnswers.ToList().Count,
                                    correctAnswers = 0,
                                    RollNo = StudData.FirstOrDefault().RollNo,
                                    score = (int)AllAnswers.Sum(x => x.Marks),
                                    subname = TestData.FirstOrDefault().Test_Name,
                                    topicname = "Programming Test",
                                    today = ScoreBoardData.FirstOrDefault().CreatedOn,
                                    totalTime = ScoreBoardData.FirstOrDefault().Test_Duration,
                                    TestType = 2// This type is to filter the results on the User Score Board, it is not stored in DB
                                }
                                ).OrderByDescending(x => x.Id).Distinct().ToList();
                progTest = progList.OrderByDescending(x => x.Id).ToList();
            }

            foreach (var item in progTest)
            {
                scoreGrid.Add(item);
            }

            return scoreGrid;
        }
        [ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult CreateQuiz()
        {
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CreateQuiz(createOption ques)
        {
            mocktestEntities1 db = new mocktestEntities1();

            List<Topic> topicName = new List<Topic>();
            List<SubTopic> subName = new List<SubTopic>();
            List<QuesDetail> questList = new List<QuesDetail>();
            bool Active = true;

            QuesDetail newQuestion = new QuesDetail();
            OptionDetail newOption = new OptionDetail();
            int quesId = 0;
            topicName = db.Topics.ToList();
            subName = db.SubTopics.ToList();
            if (QuestionIsVaild(ques.question.Question, ques.SubId))
            {
                Int32 Id = ques.SubId;
                newQuestion.SubTopicId = Id;
                string Queval = ques.question.Question.Replace("&lt", "<");
                string Queval1 = Queval.Replace("&gt", ">");
                newQuestion.Question = Queval1;
                newQuestion.OpCorrect = ques.question.optionCorrect;
                newQuestion.Active = Active;

                newOption.OpOne = ques.optionOne;
                newOption.OpTwo = ques.optionTwo;
                newOption.OpThree = ques.optionThree;
                newOption.OpFour = ques.optionFour;
                db.QuesDetails.Add(newQuestion);

                db.SaveChanges();
                var QSTID = newQuestion.QuesDetailId;

                newOption.QuesDetailId = QSTID;
                newOption.Active = Active;
                db.OptionDetails.Add(newOption);
                db.SaveChanges();

                return Json(new { result = 0, message = "Success" });
            }
            return Json(new { result = 1, message = "Question Already Exist!" });
        }
        [HttpGet]
        public ActionResult AddTopic()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddTopic(string TopicName)
        {
            mocktestEntities1 mock = new mocktestEntities1();

            var trim = TopicName.TrimStart();
            Topic newTopic = new Topic();
            using (mocktestEntities1 select = new mocktestEntities1())
            {
                var count = select.Topics.Where(a => a.Name == trim);
                if (count.Count() < 1)
                {
                    newTopic.Name = TopicName;
                    newTopic.Active = true;
                    mock.Topics.Add(newTopic);
                    mock.SaveChanges();
                }
                else
                {

                }
            }

            return View();
        }
        public ActionResult DeleteTopic(Int32 TopicDelId)
        {
            Topic Active;
            bool result = false;
            using (mocktestEntities1 db = new mocktestEntities1())
            {
                Active = db.Topics.Where(a => a.TopicId == TopicDelId).FirstOrDefault<Topic>();

            }
            if (Active != null)
            {
                Active.Active = false;
            }
            using (mocktestEntities1 db2 = new mocktestEntities1())
            {
                db2.Entry(Active).State = System.Data.Entity.EntityState.Modified;
                db2.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditConfTop(Int32 TopicEditId, string TopicName)
        {
            bool result = false;
            Topic Name;
            using (mocktestEntities1 db = new mocktestEntities1())
            {
                Name = db.Topics.Where(a => a.TopicId == TopicEditId).FirstOrDefault<Topic>();
            }
            if (Name != null)
            {
                Name.Name = TopicName;
            }
            using (mocktestEntities1 db2 = new mocktestEntities1())
            {
                db2.Entry(Name).State = System.Data.Entity.EntityState.Modified;
                db2.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditTopic(Int32 TopicEditId)
        {
            string resultName;
            Topic Name;
            using (mocktestEntities1 db = new mocktestEntities1())
            {
                Name = db.Topics.Where(a => a.TopicId == TopicEditId).FirstOrDefault<Topic>();
                resultName = Name.Name;
                return Json(resultName, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddSub()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddSub(Int32 TopId, string SubTopicName)
        {
            mocktestEntities1 mock = new mocktestEntities1();
            List<Topic> Tp = new List<Topic>();
            SubTopic newsubT = new SubTopic();
            var trim = SubTopicName.TrimStart();
            using (mocktestEntities1 select = new mocktestEntities1())
            {
                var count = select.SubTopics.Where(a => a.Name == trim && a.TopicId == TopId);
                if (count.Count() < 1)
                {
                    newsubT.Name = SubTopicName;
                    newsubT.Active = true;
                    newsubT.TopicId = TopId;
                    mock.SubTopics.Add(newsubT);
                    mock.SaveChanges();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteSub(Int32 SubDelId)
        {
            SubTopic Active;
            bool result = false;
            using (mocktestEntities1 db = new mocktestEntities1())
            {
                Active = db.SubTopics.Where(a => a.SubTopicId == SubDelId).FirstOrDefault<SubTopic>();
            }
            if (Active != null)
            {
                Active.Active = false;
            }
            using (mocktestEntities1 db2 = new mocktestEntities1())
            {
                db2.Entry(Active).State = System.Data.Entity.EntityState.Modified;
                db2.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditConfSub(Int32 SubEditId, string SubName)
        {
            bool result = false;
            SubTopic Name;
            using (mocktestEntities1 db = new mocktestEntities1())
            {
                Name = db.SubTopics.Where(a => a.SubTopicId == SubEditId).FirstOrDefault<SubTopic>();
            }
            if (Name != null)
            {
                Name.Name = SubName;
            }
            using (mocktestEntities1 db2 = new mocktestEntities1())
            {
                db2.Entry(Name).State = System.Data.Entity.EntityState.Modified;
                db2.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditSub(Int32 SubEditId)
        {
            string resultName;
            SubTopic Name;
            using (mocktestEntities1 db = new mocktestEntities1())
            {
                Name = db.SubTopics.Where(a => a.SubTopicId == SubEditId).FirstOrDefault<SubTopic>();
                resultName = Name.Name;
                return Json(resultName, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GettopicList()
        {
            if (Session["EmailId"] == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            createOption obj = new createOption();
            IEnumerable<Topic> TopicName = new List<Topic>();
            TopicName = getTopic();
            obj.topicDropdown = TopicName.Select(b => new SelectListItem
            {
                Value = b.TopicId.ToString(),
                Text = b.Name
            }).ToList();
            return Json(obj.topicDropdown, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetsubList(int TopicId)
        {
            if (Session["EmailId"] == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            createOption obj = new createOption();
            List<SubTopic> SubName = new List<SubTopic>();

            List<string> subList = new List<string>();
            int topiciD = Convert.ToInt32(TopicId);

            var userId = Session["UserId"].ToString();

            if (Session["RoleId"] != null)
            {
                using (mocktestEntities1 selectStatement = new mocktestEntities1())
                {
                    SubName = (selectStatement.SubTopics.Where(x => x.TopicId == topiciD & x.Active == true)).ToList();
                    obj.SubList = SubName.Select(a => new SelectListItem
                    {
                        Value = a.SubTopicId.ToString(),
                        Text = a.Name
                    }).ToList();
                    return Json(obj.SubList, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                using (mocktestEntities1 mkt = new mocktestEntities1())
                {
                    SubName = (from t in mkt.Topics join s in mkt.SubTopics on t.TopicId equals s.TopicId join q in mkt.QuesDetails on s.SubTopicId equals q.SubTopicId where (from r in mkt.QuesDetails where r.SubTopicId == q.SubTopicId select r).Count() > 19 & t.TopicId == topiciD & s.Active == true select s).Distinct().ToList();
                    obj.SubList = ModifySubTopicsToEnableDisable(SubName, userId);
                    return Json(obj.SubList, JsonRequestBehavior.AllowGet);
                }
            }
        }
        private List<SelectListItem> ModifySubTopicsToEnableDisable(List<SubTopic> subName, string userId)
        {
            var newSubName = new List<SelectListItem>();
            var userIdParsed = Int32.Parse(userId);
            using (var db = new mocktestEntities1())
            {
                var userTestsSubmitted = db.ScoreDetails.Where(x => x.UserId == userIdParsed).ToList();

                if (userTestsSubmitted.Count > 0)
                {
                    foreach (var subTopic in subName)
                    {
                        var alreadyGivenTest = userTestsSubmitted.Where(x => x.SubID == subTopic.SubTopicId).Count();
                        if (alreadyGivenTest > 0)
                        {
                            SelectListItem theItem = new SelectListItem();
                            theItem.Text = subTopic.Name;
                            theItem.Value = subTopic.SubTopicId.ToString();
                            theItem.Selected = true;
                            //newSubName.Add(theItem);
                        }
                        else
                        {
                            SelectListItem theItem = new SelectListItem();
                            theItem.Text = subTopic.Name;
                            theItem.Value = subTopic.SubTopicId.ToString();
                            theItem.Selected = false;
                            newSubName.Add(theItem);
                        }
                    }
                    return newSubName;
                }
                else
                {
                    var item = subName[0];
                    //foreach (var item in subName)
                    //{
                    SelectListItem theItem = new SelectListItem();
                    theItem.Text = item.Name;
                    theItem.Value = item.SubTopicId.ToString();
                    theItem.Selected = false;
                    newSubName.Add(theItem);
                    //}
                    return newSubName;
                }

            }
        }
        public JsonResult GetQuestions(Int32 subId, int? Page)
        {
            createOption obj = new createOption();
            List<QuesDetail> QuestionName = new List<QuesDetail>();
            Int32 subiD = subId;
            int pageSize = 8;
            bool Active = true;
            int pageNumber = (Page ?? 1);
            if (Session["RoleId"] != null)
            {
                using (mocktestEntities1 selectStatement = new mocktestEntities1())
                {
                    var quesdetail = selectStatement.QuesDetails.Join(selectStatement.OptionDetails, a => a.QuesDetailId, b => b.QuesDetailId, (a, b) => new { a, b })
        .Join(selectStatement.SubTopics, a => a.a.SubTopicId, b => b.SubTopicId, (a, b) => new { a, b }).Where(a => a.a.a.SubTopicId == subId & a.a.a.Active == Active).ToList();
                    obj.QuestionGrid = quesdetail.Select(c => new ShowQuiz
                    {
                        Qid = c.a.a.QuesDetailId,
                        OpId = c.a.b.OptionDetailsId,
                        QuestionName = c.a.a.Question.ToString(),
                        OpOne = c.a.b.OpOne.ToString(),
                        OpTwo = c.a.b.OpTwo.ToString(),
                        OpThree = c.a.b.OpThree.ToString(),
                        OpFour = c.a.b.OpFour.ToString(),
                        Correct = c.a.a.OpCorrect.ToString()

                    }).ToList();
                    return Json(obj.QuestionGrid, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                using (mocktestEntities1 selectStatement = new mocktestEntities1())
                {
                    if (TempData["SubId"] != null)
                    {
                        subId = Convert.ToInt32(TempData["SubId"]);
                    }
                    var quesdetail = selectStatement.QuesDetails.Join(selectStatement.OptionDetails, a => a.QuesDetailId, b => b.QuesDetailId, (a, b) => new { a, b })
       .Join(selectStatement.SubTopics, a => a.a.SubTopicId, b => b.SubTopicId, (a, b) => new { a, b }).Where(a => a.a.a.SubTopicId == subId & a.a.a.Active == Active).OrderBy(r => Guid.NewGuid()).Take(40).ToList();
                    obj.QuestionGrid = quesdetail.Select(c => new ShowQuiz
                    {
                        Qid = c.a.a.QuesDetailId,
                        OpId = c.a.b.OptionDetailsId,
                        QuestionName = c.a.a.Question.ToString(),
                        OpOne = c.a.b.OpOne.ToString(),
                        OpTwo = c.a.b.OpTwo.ToString(),
                        OpThree = c.a.b.OpThree.ToString(),
                        OpFour = c.a.b.OpFour.ToString(),
                        Correct = c.a.a.OpCorrect.ToString()

                    }).ToList();
                    return Json(obj.QuestionGrid, JsonRequestBehavior.AllowGet);

                }
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult ShowQuiz()
        {
            if (Session["EmailId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpGet]
        public ActionResult Edit(int QId)
        {
            createOption objcreateOption = new createOption();


            using (mocktestEntities1 db = new mocktestEntities1())
            {
                var option = db.OptionDetails.FirstOrDefault(a => a.QuesDetailId == QId);
                if (option != null)
                {
                    objcreateOption.question.EditQuestion = option.QuesDetail.Question;
                    objcreateOption.question.optionCorrect = option.QuesDetail.OpCorrect;
                    objcreateOption.optionOne = option.OpOne;
                    objcreateOption.optionTwo = option.OpTwo;
                    objcreateOption.optionThree = option.OpThree;
                    objcreateOption.optionFour = option.OpFour;
                    objcreateOption.Qid = QId;
                }
            }
            return Json(objcreateOption, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Edit(createOption option)
        {
            OptionDetail Op = new OptionDetail();
            QuesDetail ques = new QuesDetail();
            using (var ctx = new mocktestEntities1())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                Op = ctx.OptionDetails.Where(s => s.QuesDetailId == option.Qid).FirstOrDefault<OptionDetail>();
                ques = ctx.QuesDetails.Where(a => a.QuesDetailId == option.Qid).FirstOrDefault<QuesDetail>();
            }
            ques.Question = option.question.EditQuestion;
            ques.OpCorrect = option.question.optionCorrect;
            Op.OpOne = option.optionOne;
            Op.OpTwo = option.optionTwo;
            Op.OpThree = option.optionThree;
            Op.OpFour = option.optionFour;

            using (mocktestEntities1 selectStatement = new mocktestEntities1())
            {
                selectStatement.Entry(ques).State = System.Data.Entity.EntityState.Modified;
                selectStatement.Entry(Op).State = System.Data.Entity.EntityState.Modified;
                selectStatement.SaveChanges();
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int Qid)
        {
            bool result = false;
            QuesDetail Active1;
            OptionDetail Active2;
            using (mocktestEntities1 db = new mocktestEntities1())
            {
                Active1 = db.QuesDetails.Where(a => a.QuesDetailId == Qid).FirstOrDefault<QuesDetail>();
                Active2 = db.OptionDetails.Where(a => a.QuesDetailId == Qid).FirstOrDefault<OptionDetail>();
            }
            if (Active1 != null && Active2 != null)
            {
                Active1.Active = false;
                Active2.Active = false;
            }
            using (mocktestEntities1 db2 = new mocktestEntities1())
            {
                db2.Entry(Active1).State = System.Data.Entity.EntityState.Modified;
                db2.Entry(Active2).State = System.Data.Entity.EntityState.Modified;
                db2.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public FileResult DownloadExcel()
        {
            string path = Server.MapPath("/ExcelFile/QuestionDetails.xlsx");
            return File(path, "application/vnd.ms-excel", "QuestionDetails.xlsx");
        }
        public IEnumerable<Topic> getTopic()
        {
            mocktestEntities1 db = new mocktestEntities1();
            List<Topic> topicName = new List<Topic>();
            if (Session["RoleId"] != null)
            {
                topicName = db.Topics.Where(x => x.Active == true).ToList();
                return topicName;
            }
            else
            {
                using (mocktestEntities1 mkt = new mocktestEntities1())
                {

                    topicName = (from t in mkt.Topics join s in mkt.SubTopics on t.TopicId equals s.TopicId join q in mkt.QuesDetails on s.SubTopicId equals q.SubTopicId where q.QuesDetailId != 0 & t.Active == true select t).Distinct().ToList();
                    return topicName;
                }

            }

        }
        public IEnumerable<SubTopic> getSub()
        {
            mocktestEntities1 db = new mocktestEntities1();
            List<SubTopic> subName = new List<SubTopic>();
            subName = db.SubTopics.ToList();
            return subName;
        }
        public List<OptionDetail> getOption(int QuesId)
        {
            int quesiD = Convert.ToInt32(QuesId);
            using (mocktestEntities1 selectStatement = new mocktestEntities1())
            {
                List<OptionDetail> OptionName = new List<OptionDetail>();
                OptionName = (selectStatement.OptionDetails.Where(x => x.QuesDetailId == QuesId)).ToList();
                return OptionName;
            }

        }
        public bool QuestionIsVaild(string name, Int32 SubID)
        {

            List<QuesDetail> quesname = new List<QuesDetail>();
            using (mocktestEntities1 mk = new mocktestEntities1())
            {
                quesname = mk.QuesDetails.Where(x => x.Question == name & x.Active == true & x.SubTopicId == SubID).ToList();
                if (quesname.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        [HttpGet]
        [CustomAdminPanelAuthorizationFilter]
        public ActionResult ProgrammingQuiz_list()
        {
            ProgamingTestList List1 = new ProgamingTestList();
            List1.ProgList = new List<Progtest>();
            List1.newProgTest = new Progtest();
            using (var DB = new mocktestEntities1())
            {
                var choice = DB.Tbl_Prog_Test.Where(x => x.Test_IsActive == true).ToList();
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
                return View(List1);
            }
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
        [HttpGet]
        public ActionResult ShowProgramingTestPartialJson()
        {
            ProgamingTestList List1 = new ProgamingTestList();
            List1.ProgList = new List<Progtest>();
            List1.newProgTest = new Progtest();
            using (var DB = new mocktestEntities1())
            {
                var choice = DB.Tbl_Prog_Test.Where(x => x.Test_IsActive == true).ToList();
                foreach (var item in choice)
                {
                    List1.ProgList.Add(new Progtest()
                    {
                        Testid = item.Test_ID,
                        TestName = item.Test_Name,
                        TestDuration = (item.Test_Duration),
                        NoofQuestion = item.Noofquestion
                    });
                }
            }
            return PartialView("~/Views/Home/ShowProgramingTest.cshtml", List1);
        }
        [HttpGet]
        public ActionResult Test_Prog(Progtest prgt)
        {

            try
            {
                Tbl_Prog_Test _objectProg = new Tbl_Prog_Test();
                using (db = new mocktestEntities1())
                {
                    _objectProg.Test_Name = prgt.TestName;
                    _objectProg.Test_Duration = prgt.TestDuration;
                    _objectProg.Test_IsActive = true;
                    db.Tbl_Prog_Test.Add(_objectProg);
                    db.SaveChanges();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);

            }

        }
        [HttpGet]
        public ActionResult Edittest(int id)
        {
            var testid = id;
            Progtest _objpr = new Progtest();
            try
            {
                using (db = new mocktestEntities1())
                {
                    var Testprog = db.Tbl_Prog_Test.Where(x => x.Test_ID == testid).FirstOrDefault();
                    _objpr.TestName = Testprog.Test_Name;
                    _objpr.TestDuration = Testprog.Test_Duration;
                    _objpr.NoofQuestion = Testprog.Noofquestion;
                    _objpr.Testid = Testprog.Test_ID;
                    return Json(_objpr, JsonRequestBehavior.AllowGet);
                    //return PartialView("~/Views/Home/EditTest.cshtml", _objpr);
                }
            }
            catch (Exception ex)
            {

                return Json(_objpr, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult UpdateTest(int id, Progtest modal)
        {
            bool success = false;
            try
            {
                using (var db = new mocktestEntities1())
                {
                    var result = db.Tbl_Prog_Test.SingleOrDefault(b => b.Test_ID == id);
                    if (result != null)
                    {
                        result.Test_Name = modal.TestName;
                        result.Test_Duration = modal.TestDuration;
                        db.SaveChanges();
                    }
                }
                success = true;
            }
            catch (Exception)
            {
                success = false;
                throw;
            }
            return Json(success);
        }
        [HttpGet]
        public ActionResult AddQuestions(int id = 29)
        {
            ViewBag.TestID = id;
            List<QuizApps.Models.QuesDetail> newQuestions = new List<Models.QuesDetail>();
            Progtest theTest = new Progtest();

            using (db = new mocktestEntities1())
            {
                var Testprog = db.Tbl_Prog_Test.Where(x => x.Test_ID == id).FirstOrDefault();
                theTest.TestName = Testprog.Test_Name;
                theTest.TestDuration = Testprog.Test_Duration;
                theTest.Testid = Testprog.Test_ID;
                theTest.NoofQuestion = Testprog.Noofquestion;
            }
            ViewBag.TestName = theTest.TestName;
            ViewBag.TestDuration = theTest.TestDuration;
            ViewBag.Testid = theTest.Testid;
            ViewBag.NoofQuestion = theTest.NoofQuestion;

            return View(newQuestions);
        }
        [HttpPost]
        public ActionResult AddQuestions(List<Tbl_Prog_Ques> quesList)
        {
            bool added = false;
            var questionAdd = new Tbl_Prog_Ques();
            int id = 0;
            var TestCases = new Tbl_Stud_ProgTest_TestCases();
            try
            {
                //using (db = new mocktestEntities1())
                //{
                foreach (var ques in quesList)
                {
                    using (db = new mocktestEntities1())
                    {
                        //var questionsAddedOrNot = db.Tbl_Prog_Ques.Add(ques);
                        questionAdd.Test_ID = ques.Test_ID;
                        questionAdd.Max_Marks = ques.Max_Marks;
                        questionAdd.Ques_Desc = ques.Ques_Desc;
                        questionAdd.Ques_IsActive = true;
                        db.Tbl_Prog_Ques.Add(questionAdd);
                        db.SaveChanges();
                        id = questionAdd.Ques_ID;

                    }
                    using (db = new mocktestEntities1())
                    {
                        //var Questions = db.Tbl_Prog_Ques.OrderByDescending(t => t.Ques_ID).FirstOrDefault();
                        foreach (var item in ques.Tbl_Stud_ProgTest_TestCases)
                        {

                            TestCases.Test_ID = ques.Test_ID;
                            TestCases.Ques_ID = id;
                            TestCases.Test_Case_Output = item.Test_Case_Output;
                            TestCases.Test_Case_Input = item.Test_Case_Input;
                            TestCases.Type = item.Type;
                            TestCases.IsActive = true;
                            db.Tbl_Stud_ProgTest_TestCases.Add(TestCases);
                            db.SaveChanges();
                        }
                    }
                    //TestCases.Test_Case_Description = ques.


                }
                //db.SaveChanges();
                //}
                added = true;
            }
            catch (Exception ex)
            {
                added = false;
            }
            SetViewBagProps(added);
            return Json(added);
        }
        private void SetViewBagProps(bool added)
        {
            if (added)
            {
                ViewBag.Message = "Added";
            }
            else
            {
                ViewBag.Message = "Can't Added";
            }
            ViewBag.OperationSuccess = added;

        }
        [HttpGet]
        [CustomAdminPanelAuthorizationFilter]
        public ActionResult EditQuestion(int id)
        {
            if (Session["EmailId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TestID = id;
            List<Tbl_Prog_Ques> allQuestions = new List<Tbl_Prog_Ques>();
            Progtest theTest = new Progtest();

            using (db = new mocktestEntities1())
            {
                var Testprog = db.Tbl_Prog_Test.Where(x => x.Test_ID == id).FirstOrDefault();
                theTest.TestName = Testprog.Test_Name;
                theTest.TestDuration = Testprog.Test_Duration;
                theTest.Testid = Testprog.Test_ID;
                theTest.NoofQuestion = Testprog.Noofquestion;

                //allQuestions = db.Tbl_Prog_Ques.Where(x => x.Test_ID == id && x.Ques_IsActive == true).ToList();
                allQuestions = db.Tbl_Prog_Ques.Where(x => x.Test_ID == id && x.Ques_IsActive == true).Include(x => x.Tbl_Stud_ProgTest_TestCases).ToList();
            }
            QuestionOperation hasQuestions = CheckIfTestHasQuestions(id);
            ViewBag.TestName = theTest.TestName;
            ViewBag.TestDuration = theTest.TestDuration;
            ViewBag.Testid = theTest.Testid;
            ViewBag.NoofQuestion = hasQuestions.count;

            return View(allQuestions);
        }
        public ActionResult UpdateQuestion(int id, List<Tbl_Prog_Ques> quesList)
        {
            if (Session["EmailId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TestID = id;
            bool success = false;
            var questionAdd = new Tbl_Prog_Ques();
            var TestCases = new Tbl_Stud_ProgTest_TestCases();
            try
            {
                //using (db = new mocktestEntities1())
                //{
                //using (db = new mocktestEntities1())
                //{
                //    db.Tbl_Stud_ProgTest_TestCases.RemoveRange(db.Tbl_Stud_ProgTest_TestCases.Where(x => x.Test_ID == id));
                //    db.SaveChanges();
                //}
                //using (db = new mocktestEntities1())
                //{
                //    //var questionsAddedOrNot = db.Tbl_Prog_Ques.Add(ques);
                //    db.Tbl_Prog_Ques.RemoveRange(db.Tbl_Prog_Ques.Where(x => x.Test_ID == id));
                //    db.SaveChanges();
                //}
                using (var db = new mocktestEntities1())
                {
                    var result = db.Tbl_Prog_Ques.Where(b => b.Test_ID == id && b.Ques_IsActive == true);
                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            item.Ques_IsActive = false;
                        }
                        db.SaveChanges();
                    }
                    var testCaseResult = db.Tbl_Stud_ProgTest_TestCases.Where(b => b.Test_ID == id && b.IsActive == true);
                    if (testCaseResult != null)
                    {
                        foreach (var item1 in testCaseResult)
                        {
                            item1.IsActive = false;
                        }
                        db.SaveChanges();
                    }
                }
                foreach (var ques in quesList)
                {
                    using (db = new mocktestEntities1())
                    {
                        //var questionsAddedOrNot = db.Tbl_Prog_Ques.Add(ques);
                        questionAdd.Test_ID = ques.Test_ID;
                        questionAdd.Max_Marks = ques.Max_Marks;
                        questionAdd.Ques_Desc = ques.Ques_Desc;
                        questionAdd.Ques_IsActive = true;
                        db.Tbl_Prog_Ques.Add(questionAdd);
                        db.SaveChanges();
                        id = questionAdd.Ques_ID;

                    }
                    using (db = new mocktestEntities1())
                    {
                        //var Questions = db.Tbl_Prog_Ques.OrderByDescending(t => t.Ques_ID).FirstOrDefault();
                        foreach (var item in ques.Tbl_Stud_ProgTest_TestCases)
                        {

                            TestCases.Test_ID = ques.Test_ID;
                            TestCases.Ques_ID = id;
                            TestCases.Test_Case_Output = item.Test_Case_Output;
                            TestCases.Test_Case_Input = item.Test_Case_Input;
                            TestCases.Type = item.Type;
                            TestCases.IsActive = true;
                            db.Tbl_Stud_ProgTest_TestCases.Add(TestCases);
                            db.SaveChanges();
                        }
                    }
                    //TestCases.Test_Case_Description = ques.


                }
                //db.SaveChanges();
                //}
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                throw;
            }
            return Json(success);
        }
        [HttpGet]
        [CustomAdminPanelAuthorizationFilter]
        public ActionResult ShowQuestions(int id)
        {
            if (Session["EmailId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TestID = id;
            List<Tbl_Prog_Ques> allQuestions = new List<Tbl_Prog_Ques>();
            //List<QuestionList> allQuestions = new List<QuestionList>();
            Progtest theTest = new Progtest();

            using (db = new mocktestEntities1())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var Testprog = db.Tbl_Prog_Test.Where(x => x.Test_ID == id).FirstOrDefault();
                theTest.TestName = Testprog.Test_Name;
                theTest.TestDuration = Testprog.Test_Duration;
                theTest.Testid = Testprog.Test_ID;
                theTest.NoofQuestion = Testprog.Noofquestion;

                allQuestions = db.Tbl_Prog_Ques.Where(x => x.Test_ID == id && x.Ques_IsActive == true).Include(x => x.Tbl_Stud_ProgTest_TestCases).ToList();
            }


            //using (db = new mocktestEntities1())
            //{
            //    var Testprog = db.Tbl_Prog_Test.Where(x => x.Test_ID == id).FirstOrDefault();
            //    theTest.TestName = Testprog.Test_Name;
            //    theTest.TestDuration = Testprog.Test_Duration;
            //    theTest.Testid = Testprog.Test_ID;
            //    theTest.NoofQuestion = Testprog.Noofquestion;

            //    var selectQuestions = db.Tbl_Prog_Ques.Join(db.Tbl_Stud_ProgTest_TestCases, PQ => PQ.Ques_ID, TC => TC.Ques_ID, (PQ, TC) => new { PQ, TC }
            //   ).Where(x => x.PQ.Test_ID == id && x.PQ.Ques_IsActive == true);

            //    allQuestions = selectQuestions.Select(main => new QuestionList
            //    {
            //        Max_Marks = main.PQ.Max_Marks,
            //        Ques_Desc = main.PQ.Ques_Desc,
            //        Test_Case_Description = main.TC.Test_Case_Description,
            //        Type = main.TC.Type,
            //    }).ToList();

            //allQuestions
            //}
            QuestionOperation hasQuestions = CheckIfTestHasQuestions(id);
            ViewBag.TestName = theTest.TestName;
            ViewBag.TestDuration = theTest.TestDuration;
            ViewBag.Testid = theTest.Testid;
            ViewBag.NoofQuestion = hasQuestions.count;

            return View(allQuestions);
        }
        [HttpGet]
        public ActionResult ShowProgramingTest()
        {
            if (Session["EmailId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ProgamingTestList List1 = new ProgamingTestList();
            List1.ProgList = new List<Progtest>();
            List1.newProgTest = new Progtest();
            using (var DB = new mocktestEntities1())
            {
                var choice = DB.Tbl_Prog_Test.Where(x => x.Test_IsActive == true).ToList();
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
                return PartialView("~/Views/Home/ShowProgramingTest.cshtml", List1);
            }
        }
        [HttpPost]
        public ActionResult DeleteProgrammingTest(int id)
        {
            bool success = false;
            try
            {
                using (var db = new mocktestEntities1())
                {
                    var res = db.Tbl_Prog_Test.Where(x => x.Test_ID == id).FirstOrDefault();
                    if (res != null)
                    {
                        res.Test_IsActive = false; db.SaveChanges();
                    }
                }
                success = true;
            }
            catch (Exception)
            {
                success = false;
                throw;
            }
            return Json(success);
        }
        [HttpGet]
        public ActionResult GetAllBranchesData()
        {
            Branch branch = new Branch();
            try
            {
                return Json(branch.GetAllBranches(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult GetScoreBoardDataByBranch(string branchName)
        {
            scoreDetails obj = new scoreDetails();
            UserScoreBoard getDataByBranch = new UserScoreBoard();
            obj.scoreGrid = obj.scoreGrid;
            obj.scoreGrid = getDataByBranch.GetAllTestDataByBranch(branchName);
            //interval = interval + 20;

            return PartialView("_UserListQuizTest", obj.scoreGrid);
        }
        [HttpGet]
        public ActionResult GetAllSubTopicsData()
        {
            SubTopicM subTopics = new SubTopicM();
            try
            {
                return Json(subTopics.GetAllSubTopicsForUserScoreBoard(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult GetScoreBoardDataBySubTopic(string subTopic)
        {
            scoreDetails obj = new scoreDetails();
            var getData = new UserScoreBoard().GetAllTestDataBySubTopic(subTopic);

            return PartialView("_UserListQuizTest", getData);
        }
        [HttpGet]
        public ActionResult GetScoreBoardDataByDate(DateTime from, DateTime to)
        {
            scoreDetails obj = new scoreDetails();
            var getData = new UserScoreBoard().GetAllTestDataByDate(from, to);

            return PartialView("_UserListQuizTest", getData);
        }
        [HttpGet]
        public ActionResult GetAllTestsSubmitted()
        {
            var getData = new UserScoreBoard().GetAllTestsSubmitted();
            //interval = interval + 20;

            return PartialView("_UserListQuizTest", getData);
        }
        [HttpGet]
        public ActionResult QuizSubmitted()
        {
            return View();
        }
        public ActionResult BrowserAccess()
        {
            return View();
        }
    }
}
