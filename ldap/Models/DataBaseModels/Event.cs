namespace ldap.Models.DataBaseModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ldap.Infrastructure;

    public class Event
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

        public virtual int UserId { get; set; }    // внешний ключ на ID пользователя создавшего событие

        public int RoomId { get; set; }

        public virtual ICollection<Member> Members { get; set; } // Участники события
    }
}