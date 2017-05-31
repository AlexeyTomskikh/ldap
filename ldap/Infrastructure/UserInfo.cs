using ldap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ldap.Infrastructure
{
    public class UserInfo
    {
        // Метод получает id текущего пользователя
        public int getCurrentUserId(){
            string username = HttpContext.Current.User.Identity.Name;
            LdapDbContext context = new LdapDbContext();
            IQueryable<User> users = context.Users.Where(c => c.Login == username);
            int _currentUserId = users.Single().Id;
            return _currentUserId;
        }
    }
}