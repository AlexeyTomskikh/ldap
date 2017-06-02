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

        public string nameMonth { get; set; }

        public DateTime nextDate { get; set; }
        public DateTime prevDate { get; set; }
        public DateTime curentDate { get; set; }
        public List<Event> eventNameList { get; set; }
    }


    public class LogicModel
    {

        public List<Event> eventList { get; set; }

        public CalendarModel calendarModel { get; set; }

    }
}