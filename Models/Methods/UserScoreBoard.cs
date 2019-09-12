using QuizApps.Models.Score;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApps.Models.Methods
{
    public class UserScoreBoard
    {

        public List<GetScore> GetAllTestDataByBranch(string branchName)
        {
            List<GetScore> allScoreData = new List<GetScore>();
            UserScoreBoard addLists = new UserScoreBoard();
            allScoreData = addLists.GetAllTestDataByBranchForProgrammintTests(branchName);

            allScoreData.AddRange(addLists.GetAllTestDataByBranchForQuiz(branchName));
            return allScoreData.OrderBy(x => x.Id).ToList();
        }

        private List<GetScore> GetAllTestDataByBranchForProgrammintTests(string branchName)
        {
            using (var db = new mocktestEntities1())
            {
                return (from answers in db.Tbl_Stud_ProgTest_Ans
                        join studData in db.users on answers.Stud_ID equals studData.UserId into StudData
                        join allAnswers in db.Tbl_Stud_ProgTest_Ans on answers.Score_id equals allAnswers.Score_id into AllAnswers
                        join testData in db.Tbl_Prog_Test on answers.Test_ID equals testData.Test_ID into TestData
                        join scoreBoardData in db.Tbl_Stud_ProgTest_Result on answers.Score_id equals scoreBoardData.Score_ID into ScoreBoardData
                        where StudData.FirstOrDefault().Branch == branchName
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
                        }).OrderByDescending(x => x.Id).Distinct().ToList();
            }
        }
        private List<GetScore> GetAllTestDataByBranchForQuiz(string branchName)
        {
            scoreDetails obj = new scoreDetails();
            using (mocktestEntities1 db = new mocktestEntities1())
            {
                var userList = db.QuesDetails.
                    Join(db.ScoreDetails, ques => ques.QuesDetailId, score => score.QuesDetailId, (ques, score) => new { ques, score })
                    .Join(db.SubTopics, a => a.ques.SubTopicId, sub => sub.SubTopicId, (a, sub) => new { a, sub })
                    .Join(db.Topics, b => b.sub.TopicId, top => top.TopicId, (b, top) => new { b, top })
                    .Join(db.users, c => c.b.a.score.UserId, us => us.UserId, (c, us) => new { c, us }).Where(a => a.c.b.a.ques.Active == true && a.us.Branch == branchName).ToList();
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
                //interval = interval + 20;

                return obj.scoreGrid;
            }
        }
        public List<GetScore> GetAllTestDataBySubTopic(string subTopic)
        {
            List<GetScore> allScoreData = new List<GetScore>();
            UserScoreBoard addLists = new UserScoreBoard();
            allScoreData = addLists.GetAllTestDataBySubTopicForProgrammintTests(subTopic);
            allScoreData.AddRange(addLists.GetAllTestDataBySubTopicForQuiz(subTopic));

            return allScoreData.OrderBy(x => x.Id).ToList();
        }

        private List<GetScore> GetAllTestDataBySubTopicForQuiz(string subTopic)
        {
            scoreDetails obj = new scoreDetails();
            using (mocktestEntities1 db = new mocktestEntities1())
            {
                var userList = db.QuesDetails.
                    Join(db.ScoreDetails, ques => ques.QuesDetailId, score => score.QuesDetailId, (ques, score) => new { ques, score })
                    .Join(db.SubTopics, a => a.ques.SubTopicId, sub => sub.SubTopicId, (a, sub) => new { a, sub })
                    .Join(db.Topics, b => b.sub.TopicId, top => top.TopicId, (b, top) => new { b, top })
                    .Join(db.users, c => c.b.a.score.UserId, us => us.UserId, (c, us) => new { c, us }).Where(a => a.c.b.a.ques.Active == true && a.c.b.sub.Name == subTopic).ToList();
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
                //interval = interval + 20;

                return obj.scoreGrid;
            }
        }

        private List<GetScore> GetAllTestDataBySubTopicForProgrammintTests(string subTopic)
        {
            using (var db = new mocktestEntities1())
            {
                return (from answers in db.Tbl_Stud_ProgTest_Ans
                        join studData in db.users on answers.Stud_ID equals studData.UserId into StudData
                        join allAnswers in db.Tbl_Stud_ProgTest_Ans on answers.Score_id equals allAnswers.Score_id into AllAnswers
                        join testData in db.Tbl_Prog_Test on answers.Test_ID equals testData.Test_ID into TestData
                        join scoreBoardData in db.Tbl_Stud_ProgTest_Result on answers.Score_id equals scoreBoardData.Score_ID into ScoreBoardData
                        where TestData.FirstOrDefault().Test_Name == subTopic
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
                        }).OrderByDescending(x => x.Id).Distinct().ToList();
            }
        }

        public List<GetScore> GetAllTestDataByDate(DateTime from, DateTime to)
        {
            List<GetScore> allScoreData = new List<GetScore>();
            UserScoreBoard addLists = new UserScoreBoard();
            allScoreData = addLists.GetAllTestDataByDateForProgrammintTests(from, to);
            allScoreData.AddRange(addLists.GetAllTestDataByDateForQuiz(from, to));

            return allScoreData.OrderBy(x => x.Id).ToList();
        }

        private List<GetScore> GetAllTestDataByDateForProgrammintTests(DateTime fromDate, DateTime toDate)
        {
            using (var db = new mocktestEntities1())
            {
                return (from answers in db.Tbl_Stud_ProgTest_Ans
                        join studData in db.users on answers.Stud_ID equals studData.UserId into StudData
                        join allAnswers in db.Tbl_Stud_ProgTest_Ans on answers.Score_id equals allAnswers.Score_id into AllAnswers
                        join testData in db.Tbl_Prog_Test on answers.Test_ID equals testData.Test_ID into TestData
                        join scoreBoardData in db.Tbl_Stud_ProgTest_Result on answers.Score_id equals scoreBoardData.Score_ID into ScoreBoardData
                        where answers.CreatedOn >= fromDate && answers.CreatedOn <= toDate
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
                        }).OrderByDescending(x => x.Id).Distinct().ToList();
            }
        }

        private List<GetScore> GetAllTestDataByDateForQuiz(DateTime fromDate, DateTime toDate)
        {
            scoreDetails obj = new scoreDetails();
            using (mocktestEntities1 db = new mocktestEntities1())
            {
                var userList = db.QuesDetails.
                    Join(db.ScoreDetails, ques => ques.QuesDetailId, score => score.QuesDetailId, (ques, score) => new { ques, score })
                    .Join(db.SubTopics, a => a.ques.SubTopicId, sub => sub.SubTopicId, (a, sub) => new { a, sub })
                    .Join(db.Topics, b => b.sub.TopicId, top => top.TopicId, (b, top) => new { b, top })
                    .Join(db.users, c => c.b.a.score.UserId, us => us.UserId, (c, us) => new { c, us }).Where(a => a.c.b.a.ques.Active == true && a.us.ScoreDetails.FirstOrDefault().date >= fromDate && a.us.ScoreDetails.FirstOrDefault().date <= toDate).ToList();
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
                //interval = interval + 20;

                return obj.scoreGrid;
            }
        }

        public List<GetScore> GetAllTestsSubmitted()
        {
            List<GetScore> allScoreData = new List<GetScore>();
            UserScoreBoard addLists = new UserScoreBoard();
            allScoreData = addLists.GetAllQuizTestSubmitted();
            allScoreData.AddRange(addLists.GetAllProgrammingTestSubmitted());

            return allScoreData;
        }

        private List<GetScore> GetAllQuizTestSubmitted()
        {
            scoreDetails obj = new scoreDetails();
            using (mocktestEntities1 db = new mocktestEntities1())
            {
                var userList = db.QuesDetails.
                    Join(db.ScoreDetails, ques => ques.QuesDetailId, score => score.QuesDetailId, (ques, score) => new { ques, score })
                    .Join(db.SubTopics, a => a.ques.SubTopicId, sub => sub.SubTopicId, (a, sub) => new { a, sub })
                    .Join(db.Topics, b => b.sub.TopicId, top => top.TopicId, (b, top) => new { b, top })
                    .Join(db.users, c => c.b.a.score.UserId, us => us.UserId, (c, us) => new { c, us }).Where(a => a.c.b.a.ques.Active == true).ToList();
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
                //interval = interval + 20;

                return obj.scoreGrid;
            }
        }

        private List<GetScore> GetAllProgrammingTestSubmitted()
        {
            using (var db = new mocktestEntities1())
            {
                return (from answers in db.Tbl_Stud_ProgTest_Ans
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
                        }).OrderByDescending(x => x.Id).Distinct().ToList();
            }
        }
    }
}