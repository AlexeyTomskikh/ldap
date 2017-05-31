using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ldap.Models.DataBaseModels
{
    public class UserOfRoles
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }

        public virtual List<User> Users { get; set; }
        public virtual List<Role> Roles { get; set; }
    }
}