using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApps.Models
{
    public class ProgrammingTestAnswer
    {
        private string createdBy;
        private DateTime createdOn;
        private int ques_ID;
        private int? score_id;
        private string stud_Ans;
        private int stud_ID;
        private bool isActive;

        public string CreatedBy
        {
            get
            {
                return createdBy;
            }

            set
            {
                createdBy = value;
            }
        }

        public DateTime CreatedOn
        {
            get
            {
                return createdOn;
            }

            set
            {
                createdOn = value;
            }
        }

        public int Ques_ID
        {
            get
            {
                return ques_ID;
            }

            set
            {
                ques_ID = value;
            }
        }

        public int? Score_id
        {
            get
            {
                return score_id;
            }

            set
            {
                score_id = value;
            }
        }

        public string Stud_Ans
        {
            get
            {
                return stud_Ans;
            }

            set
            {
                stud_Ans = value;
            }
        }

        public int Stud_ID
        {
            get
            {
                return stud_ID;
            }

            set
            {
                stud_ID = value;
            }
        }

        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                isActive = value;
            }
        }
    }
}