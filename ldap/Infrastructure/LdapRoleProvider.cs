namespace ldap.Models
{
    using System;
    using System.Linq;
    using System.Web.Security;
    using ldap.Infrastructure;

    public class LdapRoleProvider : RoleProvider
    {
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            using (LdapDbContext db = new LdapDbContext())
            {
                // Загрузить все роли для username
                IQueryable<User> custs = db.Users.Where(c => c.Login == username);

                if (custs.Any())
                {
                    var userroles = custs.Single().Roles.Select(p => p.Name).ToArray();

                    return userroles;
                }
                else
                {
                    return new string[0];
                }
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            bool result = false;

            using (LdapDbContext db = new LdapDbContext())
            {
                try
                {
                    User user = db.Users.FirstOrDefault(p => p.Login == username); // Находим пользователя

                    if (user != null)
                    {
                        result = user.Roles.Any(p => p.Name == roleName);           // получаем роль 
                    }
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}