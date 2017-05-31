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
    public class LogicController : Controller
    {

        // Метод возвращает модель calendarModel в частичное представление _getCalendar
        [HttpPost]
        public ActionResult _getCalendar(int year, int month)
        {
            CalendarModel calendarModel = getModel(year, month);

            return PartialView(calendarModel);
        }

        public CalendarModel getModel(int year, int month)
        {
            // Вычисляем количество строк в таблице в зависимости от года
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;

            DateTime date1 = new DateTime(year, month, 1, new GregorianCalendar());
            DateTime date2 = date1.AddMonths(1).AddDays(-1);

            Calendar cal = dfi.Calendar;
            int firstWeek = cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek); // первая неделя месяца
            int lastWeek = cal.GetWeekOfYear(date2, dfi.CalendarWeekRule, dfi.FirstDayOfWeek); // последняя неделя месяца

            //получаем количество строк в таблице
            int _numberOfLines = lastWeek - firstWeek;
            // Количество дней в месяце
            int _daysInMonth = cal.GetDaysInMonth(year, month);
            // Получаем номер дня  внеделе
            int _dayOfWeek = ((int)date1.DayOfWeek == 0) ? 7 : (int)date1.DayOfWeek;

            CalendarModel calendarModel = new CalendarModel //new CalendarModel();
            {
                daysInMonth = _daysInMonth,
                dayOfWeek = _dayOfWeek,
                numberOfLines = _numberOfLines
            };

            return calendarModel;
        }

        // Метод выводит основную страницу расписания (список текущих событий) + форма добвления нового мероприятия
        [HttpGet]
        [Authorize(Roles = "UserRead, Admin, UserReadAndWrite")]
        public ActionResult Logic()
        {
            LdapDbContext context = new LdapDbContext();
            List<Event> events =
                (from item in context.Events
                 orderby ((DateTime)item.StartTime).ToString()
                 select item).ToList();  // Получить все события из таблицы Events сортированные по дате


            ViewBag.Events = events; // Передача этого списка в частичное представление, откуда попадает в Logic

            CalendarModel currentCalendarModel = getModel(DateTime.Now.Year, DateTime.Now.Month);

            return View(currentCalendarModel);
        }


        [HttpPost]
        public ActionResult ShowPeriod(DateTime FilterStartEvent, DateTime FilterEndEvent)
        {
            EventManagement ev = new EventManagement();
            ViewBag.Events = ev.GetAllEventsOfPeriod(FilterStartEvent, FilterEndEvent);
             return View("Logic");
        }

        // Метод контролирует процесс создания нового мероприятия 
        [HttpPost]
        [Authorize(Roles = "Admin, UserReadAndWrite")]
        public ActionResult Reservation(EventModels model, string returnUrl)
        {
            EventManagement eventManagment = new EventManagement();
            IEnumerable<Event> listAllEventsDay = eventManagment.GetAllEvents(model.StartEvent.Date);// получаем все события на этот день

            if (eventManagment.busyStartTime(listAllEventsDay, model.StartEvent))
            {
                if (eventManagment.busyEndTime(listAllEventsDay, model.EndEvent))
                {
                    UserInfo userInfo = new UserInfo();
                    int userId = userInfo.getCurrentUserId();
                    eventManagment.WriteToDatabase(model.NameEvent, model.StartEvent, model.EndEvent, userId);
                    LdapDbContext db = new LdapDbContext();
                    IEnumerable<Event> ev = db.Events;
                    ViewBag.Events = ev;
                    return View("Logic");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Время конца события уже занято!");
                    LdapDbContext db = new LdapDbContext();
                    IEnumerable<Event> ev = db.Events;
                    ViewBag.Events = ev;
                    return View("Logic");

                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Время начала события уже занято!");
                LdapDbContext db = new LdapDbContext();
                IEnumerable<Event> ev = db.Events;
                ViewBag.Events = ev;
                return View("Logic");

            }
        }


        //метод для удаления события
        public ActionResult RemoveEvent(int event_id)
        {
            EventManagement ev = new EventManagement();
            ev.DeleteEvent(event_id);
            LdapDbContext db = new LdapDbContext();
            IEnumerable<Event> ev1 = db.Events;
            ViewBag.Events = ev1;
            return View("Logic");
        }

        
    }
}
