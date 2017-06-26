namespace ldap.Infrastructure
{
    using ldap.Models;

    public class ScheduleScheme
    {
        public bool Permit { get; set; }

        public int NumberRowspan { get; set; }

        public Event InnerEvent { get; set; }
    }
}