using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApps.Models.Methods
{
    public class SubTopicM
    {
        public UserScoreBoardSubTopics GetAllSubTopicsForUserScoreBoard()
        {
            UserScoreBoardSubTopics allTopics = new UserScoreBoardSubTopics();
            allTopics.listOfProgrammintTestTopics = GetProgrammintTestAsSubTopicsForUserScoreBoard();
            allTopics.listOfQuizTestSubTopics = GetQuizSubTopics();

            return allTopics;
        }

        private List<SubTopic> GetQuizSubTopics()
        {
            using (var db = new mocktestEntities1())
            {
                return db.SubTopics.Where(x => x.Active == true).Select(x => new SubTopic() {
                    Name = x.Name,
                    SubTopicId = x.SubTopicId,
                    TopicId = x.SubTopicId
                }).Distinct().ToList();
            }
        }

        // We are treating the Programming Test Name as a sub topic in this context
        private List<Progtest> GetProgrammintTestAsSubTopicsForUserScoreBoard()
        {
            Progtest theTest = new Progtest();

            return theTest.GetAllTests().ProgList;
        }

    }
}