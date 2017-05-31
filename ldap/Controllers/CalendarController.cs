using ldap.Infrastructure;
using ldap.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ldap.Controllers
{
    public class CalendarController : Controller
    {


        public ActionResult CalendarView()
        {


            return View();
        }

        public ActionResult loaddata()
        {
            using (LdapDbContext dc = new LdapDbContext())
            {

                var data = (from item in dc.Events
                            orderby ((DateTime)item.StartTime).ToString()
                            select item).ToList();  // Получить все события из таблицы Events сортированные по дате
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetEvents()
        {
            //Here MyDatabaseEntities is our entity datacontext (see Step 4)
            using (LdapDbContext dc = new LdapDbContext())
            {
                var v = dc.Events.OrderBy(a => a.StartTime).ToList();
                return new JsonResult { Data = v, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        
        
    }
}
