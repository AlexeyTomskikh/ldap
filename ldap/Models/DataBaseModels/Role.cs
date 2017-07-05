namespace ldap.Models
{
    using System.Collections.Generic;

    public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<User> Users { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}