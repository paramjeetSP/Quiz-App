using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApps.Models.Error
{
    public class ExceptionLogging
    {
        private string exceptionMessage;
        private string controllerName;
        private string stackTrace;
        private DateTime logTime;
        private string userId;

        public string ExceptionMessage
        {
            get
            {
                return exceptionMessage;
            }

            set
            {
                exceptionMessage = value;
            }
        }

        public string ControllerName
        {
            get
            {
                return controllerName;
            }

            set
            {
                controllerName = value;
            }
        }

        public string StackTrace
        {
            get
            {
                return stackTrace;
            }

            set
            {
                stackTrace = value;
            }
        }

        public DateTime LogTime
        {
            get
            {
                return logTime;
            }

            set
            {
                logTime = value;
            }
        }

        public string UserId
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

        public void LogExceptionInDb(ExceptionLogging exceptionData)
        {
            tbl_ExceptionLogger logData = new tbl_ExceptionLogger();
            logData.ControllerName = exceptionData.controllerName;
            logData.ExceptionMessage = exceptionData.exceptionMessage;
            logData.ExceptionStackTrace = exceptionData.stackTrace;
            logData.LogTime = DateTime.Now;
            logData.UserID = exceptionData.userId;
            logData.UserID = "No Id";
            using (var db = new mocktestEntities1())
            {
                db.tbl_ExceptionLogger.Add(logData);
                db.SaveChanges();
            }
        }

    }
}