using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuizApps.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public Role GetAdminRole()
        {
            using (var db = new mocktestEntities1())
            {
                return db.Roles.Where(x => x.RoleName == "Admin").Select(x => new Role() {
                    RoleId  = x.RoleId,
                    RoleName = x.RoleName
                }).FirstOrDefault();
            }
        }
    }
}
