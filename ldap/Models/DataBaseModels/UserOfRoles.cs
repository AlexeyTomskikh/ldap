namespace ldap.Models.DataBaseModels
{
    using System.Collections.Generic;

    public class UserOfRoles
    {
        public int RoleId { get; set; }

        public int UserId { get; set; }

        public virtual List<User> Users { get; set; }

        public virtual List<Role> Roles { get; set; }
    }
}