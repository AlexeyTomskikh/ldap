namespace ldap.Infrastructure
{
    using System.Linq;
    using System.Web;
    using ldap.Models;

    public class UserManager
    {
        // Метод получает id текущего пользователя
        public int GetCurrentUserId()
        {
            string username = HttpContext.Current.User.Identity.Name;
            if (username != string.Empty)
            {
                using (LdapDbContext db = new LdapDbContext())
                {
                    IQueryable<User> users = db.Users.Where(c => c.Login == username);
                    int currentUserId = users.Single().Id;
                    return currentUserId;
                }
            }

            return 0;
        }
    }
}