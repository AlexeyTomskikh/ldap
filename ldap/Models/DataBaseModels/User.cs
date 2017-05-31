using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ldap.Models
{
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