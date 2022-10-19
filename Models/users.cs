using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace QuizApps.Models
{
    
    public class users
    {
        [NotMapped]
        [Required]
        [Key]
        public int UserId { get; set; }

        [NotMapped]
        [Display(Name = "Name")]
        [StringLength(50, ErrorMessage = "Name should be between 6 and 50 character", MinimumLength = 6)]
        public string Name { get; set; }

        [NotMapped]
        [Display(Name = "Email Id")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [NotMapped]
        [Display(Name = "Password")]
        [StringLength(15, ErrorMessage = "Password should be between 6 and 15 character", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        [DataType(DataType.Password)]
        public bool ConfirmPassword { get; set; }

        public int? RoleId { get; set; }


        public List<users> GetAllUsers()
        {
            using (var db = new mocktestEntities1())
            {
                return db.users.Select(x => new users() {
                    UserId = x.UserId,
                    Name = x.Name,
                    EmailId = x.EmailId,
                    Password = x.Password
                }).ToList();
            }
        }

        public users GetUserByEmail(string email)
        {
            using (var db = new mocktestEntities1())
            {
                var res = db.users.Where(x => x.EmailId == email).Select( x =>  new users() {
                    Name = x.Name,
                    EmailId = x.EmailId,
                    Password = x.Password,
                    UserId = x.UserId,
                    RoleId = x.roleId
                }).FirstOrDefault();
                return res;
            }
        }

        public bool CheckIfUserHasGivenTest(string email,  int testId)
        {
            users user = new users().GetUserByEmail(email);
            bool hasGivenTest = false;
            using (var db = new mocktestEntities1())
            {
                var testsGiven = db.Tbl_Stud_ProgTest_Ans.Where(x => x.Stud_ID == user.UserId && x.Test_ID == testId).ToList();
                if (testsGiven.Count > 0)
                {
                    hasGivenTest = true;
                }
            }
            return hasGivenTest;
        }
    }
}
