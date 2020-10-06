using QuizApps.Classes;
using QuizApps.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QuizApps.Controllers
{
    [CustomExceptionFilter]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult login(Login log)
        {
            mocktestEntities1 db = new mocktestEntities1();
            var userDetail = db.users.Where(x => x.EmailId == log.EmailId && x.Password == log.Password).FirstOrDefault();
            if (userDetail != null)
            {
                Session["EmailId"] = log.EmailId.ToString();
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, log.EmailId, DateTime.Now, DateTime.Now.AddDays(30), true, FormsAuthentication.FormsCookiePath);
                string encTicket = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                if (userDetail.roleId == 1)
                {
                    Session["RoleId"] = userDetail.roleId;
                    Session["UserId"] = userDetail.UserId;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Session["UserId"] = userDetail.UserId;
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["alertMessage"] = "Invalid Username and Password !";
                ModelState.AddModelError("", "Invalid EmailId and Password");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult Register()
        {
            mocktestEntities1 db = new mocktestEntities1();
            List<string> objRole = new List<string>();
            List<Role> objListOfRole = db.Roles.ToList();
            objRole.Add("Admin");
            objRole.Add("User");
            int count = 0;
            Role role = new Role();
            foreach (var item in objRole)
            {
                if (objListOfRole.Count > 0)
                {
                    foreach (var roleItem in objListOfRole)
                    {
                        if (item == roleItem.RoleName)
                        {
                            count++;
                        }
                    }
                }
                if (count != 0)
                {

                }
                else
                {
                    role.RoleName = item;
                    db.Roles.Add(role);
                    db.SaveChanges();
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult Register(SignUp reg)
        {
            if (ModelState.IsValid)
            {
                mocktestEntities1 db = new mocktestEntities1();
                int Count = db.users.AsNoTracking().Where(a => a.EmailId.ToLower() == reg.EmailId.ToLower()).Count();
                if (Count <= 0)
                {
                    user newuser = new user();
                    List<user> userList = new List<user>();
                    userList = db.users.ToList();
                    newuser.Name = reg.Name;
                    newuser.EmailId = reg.EmailId;
                    newuser.Password = reg.Password;
                    newuser.Branch = reg.Branch;
                    newuser.RollNo = reg.Rollno;
                    newuser.roleId = 2;
                    db.users.Add(newuser);
                    db.SaveChanges();
                    var userDetail = db.users.Where(x => x.EmailId == reg.EmailId && x.Password == reg.Password).FirstOrDefault();
                    if (userDetail != null)
                    {
                        Session["EmailId"] = reg.EmailId.ToString();
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, reg.EmailId, DateTime.Now, DateTime.Now.AddDays(30), true, FormsAuthentication.FormsCookiePath);
                        string encTicket = FormsAuthentication.Encrypt(ticket);
                        Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                        Session["UserId"] = userDetail.UserId;
                        return RedirectToAction("TakeQuiz", "Home");
                    }
                    return RedirectToAction("Index", "Home", new { signed = "true" });
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { signed = "false" });
                }
            }
            else
            {
                return View(reg);
            }
        }
        public ActionResult Logout()
        {
            Session["EmailId"] = null;
            Session["RoleId"] = null;
            Session["UserId"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Profile()
        {
            using (mocktestEntities1 db = new mocktestEntities1())
            {
                user result = new user();
                SignUp sgn = new SignUp();
                if (Session["EmailId"] != null)
                {

                    string EmailId = Session["EmailId"].ToString();
                    result = db.users.Where(a => a.EmailId == EmailId).FirstOrDefault<user>();
                    sgn.EmailId = result.EmailId;
                    sgn.Name = result.Name;
                    sgn.Password = result.Password;

                }
                return Json(sgn, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult Profile(SignUp sgn)
        {
            user current = new user();
            string emId = Session["EmailId"].ToString();
            using (var ctx = new mocktestEntities1())
            {
                current = ctx.users.Where(a => a.EmailId == emId).FirstOrDefault<user>();
            }
            current.Name = sgn.Name;
            using (mocktestEntities1 db = new mocktestEntities1())
            {
                db.Entry(current).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return PartialView();
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult ChangePassword(SignUp sgnp)
        {
            user current = new user();
            string emId = Session["EmailId"].ToString();
            using (var ctx = new mocktestEntities1())
            {
                current = ctx.users.Where(a => a.EmailId == emId).FirstOrDefault<user>();
            }
            current.Password = sgnp.Password;
            using (mocktestEntities1 db = new mocktestEntities1())
            {
                db.Entry(current).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return PartialView();
        }
    }
}