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
    }
}