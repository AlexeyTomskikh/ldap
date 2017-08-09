namespace ldap.Models
{
    using System.Collections.Generic;

    using ldap.Infrastructure;
    using ldap.Models.DataBaseModels;

    public class User
    {
        // ID пользователя
        public int Id { get; set; }

        // Имя пользователя
        public string Login { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        // Роль пользователя
        public virtual List<Role> Roles { get; set; }

        // События в которых юзер является участником
        public ICollection<Member> Members { get; set; }
    }
}