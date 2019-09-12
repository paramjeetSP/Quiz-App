using QuizApps.Models.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizApps.Classes
{
    public class CustomExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                var logger = NLog.LogManager.GetCurrentClassLogger();
                logger.Error(filterContext.Exception);
                LogErrorInDb(filterContext);
                filterContext.Result = new RedirectResult("~/Error/Error.html");
                filterContext.ExceptionHandled = true;
            }
        }

        private void LogErrorInDb(ExceptionContext exception)
        {
            ExceptionLogging newException = new ExceptionLogging();
            newException.ControllerName = exception.RouteData.Values["controller"].ToString();
            newException.ExceptionMessage = exception.Exception.Message;
            newException.LogTime = DateTime.Now;
            newException.StackTrace = exception.Exception.StackTrace;
            newException.UserId = exception.HttpContext.Session["EmailId"] == null ? "No Id" : exception.HttpContext.Session["EmailId"].ToString();

            newException.LogExceptionInDb(newException);
        }
    }
}