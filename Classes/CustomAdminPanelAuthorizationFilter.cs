using QuizApps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizApps.Classes
{
    public class CustomAdminPanelAuthorizationFilter: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string userEmail = filterContext.HttpContext.Session["EmailId"] == null ? "No Id" : filterContext.HttpContext.Session["EmailId"].ToString();

            if (userEmail != null)
            {
                users theUser = new users();
                var user = theUser.GetUserByEmail(userEmail);
                if (user != null)
                {
                    QuizApps.Models.Role adminRole = new QuizApps.Models.Role().GetAdminRole();
                    if (adminRole.RoleId != user.RoleId)
                    {
                        this.HandleUnauthorizedRequest(filterContext);
                    }
                }
                else
                {
                    this.HandleUnauthorizedRequest(filterContext);
                }
            }
            else
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Home/Index");
        }
    }
}