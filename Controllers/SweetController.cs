using QuizApps.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SweetAlert.Controllers
{
    [CustomExceptionFilter]
    public class SweetController : Controller
    {
        // GET: Sweet
        public ActionResult Alert()
        {
             return View();
        }
    }
}