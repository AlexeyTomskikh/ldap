namespace ldap.Models
{
    using System;

    public class Event
    {
        public int Id { get; set; }

        public string DescriptionEvent { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public virtual int UserId { get; set; }    // внешний ключ на ID пользователя создавшего событие

        public override string ToString()
        {
            return string.Format("{0}", DescriptionEvent);
        }
    }
}