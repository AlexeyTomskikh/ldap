using ldap.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ldap.Models
{
    public class AuthorizeRole : RoleProvider
    {

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

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
            // Загрузить все роли для username
            LdapDbContext context = new LdapDbContext();
            IQueryable<User> custs = context.Users.Where(c => c.Login == username);
           

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

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();

        }

        public override bool IsUserInRole(string username, string roleName)
        {
           bool result = false;
            // Находим пользователя
            using (LdapDbContext context = new LdapDbContext())
            {
                try
                {
                    // Получаем пользователя
                    User user = (from u in context.Users
                                 where u.Login == username
                                 select u).FirstOrDefault();
                    if (user != null)
                    {
                        // получаем роль
                        Role userRole = context.Roles.Find(user.Roles);

                        //сравниваем
                        if (userRole != null && userRole.Name == roleName)
                        {
                            result = true;
                        }
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