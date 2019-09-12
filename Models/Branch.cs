using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApps.Models
{
    public class Branch
    {
        public string BranchName { get; set; }

        public List<Branch> GetAllBranches()
        {
            using (var db  = new mocktestEntities1())
            {
                return db.users.Where(x => x.Branch != null).Select(x =>new Branch() {
                    BranchName = x.Branch
                }).Distinct().ToList();
            }
        }
    }
}