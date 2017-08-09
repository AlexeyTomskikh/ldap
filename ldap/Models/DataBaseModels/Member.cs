namespace ldap.Infrastructure
{
    using ldap.Models;
    using ldap.Models.DataBaseModels;

    public class Member
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public Event Event { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
