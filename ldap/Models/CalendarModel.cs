using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ldap.Models
{
    public class CalendarModel
    {
        public int daysInMonth { get; set; }
        public int dayOfWeek { get; set; }
        public int numberOfLines { get; set; }

        public int currentDay { get; set; }
        public int currentMonth { get; set; }
        public int currentYear { get; set; }

        public DateTime nextDate { get; set; }
        public DateTime prevDate { get; set; }
    }
}