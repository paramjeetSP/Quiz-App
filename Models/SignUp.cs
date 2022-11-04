using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace QuizApps.Models
{
    public class SignUp
    {
        public int userId { get; set; }

        //[Required]
        [Display(Name = "Name")]
        [StringLength(50, ErrorMessage = "Name should be between 6 and 50 character", MinimumLength = 6)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Email Id")]
       
        [System.Web.Mvc.Remote("CheckExistingEmail", "RemoteValidation", ErrorMessage = "Email already exists!")]
        [DataType(DataType.EmailAddress)]
        //[CustomValidator(ErrorMessage="Name contains invalid character.")]
        public string EmailId { get; set; }

        [Required]
        [Display(Name = "Roll Number")]
        public string Rollno { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public string Branch { get; set; }

        [Required]
        [Display(Name = "Mobile Number")]
        [StringLength(10, ErrorMessage = "Enter valid mobile number", MinimumLength = 10)]
        public string Mobno { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "College Name")]
        public string CllgeName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(15, ErrorMessage = "Password should be between 6 and 15 character", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}