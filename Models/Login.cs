using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuizApps.Models
{
    public class Login
    {

        [Display(Name = "Email Id")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [Display(Name = "Password")]
        [StringLength(15, ErrorMessage = "Password should be between 6 and 15 character", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string JavascriptToRun { get; set;}

        public string ControllerName { get; set; }
        public string ActionName { get; set; }

    }
}