namespace ldap.Models
{
    using System.Collections.Generic;

    public class User
    {
        // ID пользователя
        public int Id { get; set; }

        // Имя пользователя
        public string Login { get; set; }
        
        // Роль пользователя
        public virtual List<Role> Roles { get; set; }
    }
}