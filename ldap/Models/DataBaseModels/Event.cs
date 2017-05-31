using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace ldap.Models
{
    public class Event
    {
        public int id { get; set; }
        public string DescriptionEvent { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public virtual int UserID { get; set; }    // внешний ключ на ID пользователя создавшего событие

        public override string ToString()
        {
            return String.Format("{0} : c {1} по: {2}", DescriptionEvent, StartTime.ToString("dd.MM.yyyy hh:mm"), EndTime.ToString("dd.MM.yyyy hh:mm"));
        }
    }
}