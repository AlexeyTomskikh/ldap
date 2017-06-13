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
        public ActionResult _getCalendar(int year, int month, int day)
        {
            CalendarModel calendarModel = getModel(year, month, day);

            return PartialView(calendarModel);
        }

        // Метод возвращает в част. представление _getEventsOfDay все события в выбранный день календаря
        public ActionResult _getEventsOfDay(DateTime currentDate)
        {
            List<Event> list = new EventManagement().GetAllEvents(currentDate);

            

            return PartialView(list);
        }

        // Метод возвращает список событий для отображения в расписании на день
        [HttpPost]
        public ActionResult _daySchedule(DateTime currentDate)
        {
            List<Event> list = new EventManagement().GetAllEvents(currentDate);
            ScheduleModel model = new ScheduleModel { eventList = list, curentDate = currentDate };

            return PartialView(model);
        }

        //метод формирует модель для обновления данных календаря по кнопке вперёд назад
        public CalendarModel getModel(int year, int month, int day)
        {

            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo; // Вычисляем количество строк в таблице в зависимости от года

            DateTime date1 = new DateTime(year, month, 1, new GregorianCalendar()); //получаем первый день месяца
            DateTime date2 = date1.AddMonths(1).AddDays(-1);                        // получаем последний день месяца

            Calendar cal = dfi.Calendar;
            int firstWeek = cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek); // первая неделя месяца
            int lastWeek = cal.GetWeekOfYear(date2, dfi.CalendarWeekRule, dfi.FirstDayOfWeek); // последняя неделя месяца

            //получаем количество строк в таблице
            int _numberOfLines = lastWeek - firstWeek;
            // Количество дней в месяце
            int _daysInMonth = cal.GetDaysInMonth(year, month);
            // Получаем номер дня  внеделе
            int _dayOfWeek = ((int)date1.DayOfWeek == 0) ? 7 : (int)date1.DayOfWeek;

            string _nameMonth = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month) + " " + year;

            DateTime _curentDate = new DateTime(year, month, day);

            EventManagement ev = new EventManagement();

            List<List<Event>> list = new List<List<Event>>();
            List<Event> vnutr = new List<Event>();
            for (int i = 1; i <= _daysInMonth; i++)
            {
                vnutr = ev.GetAllEvents(new DateTime(year, month, i));  // получаем список всех событий за день
                if (vnutr.Count != 0)  // если список не пустой 
                {
                    if (vnutr.Count <= 5) // событий меньше пяти?
                    {
                        list.Add(vnutr.GetRange(0, vnutr.Count)); // кладём в итоговый двумерный список все события дня
                    }
                    else
                    {
                        list.Add(vnutr.GetRange(0, 5)); // если больше 5 кладём только первые 5
                    }
                }
                else
                {
                    list.Add(null); // если пустой кладём null
                }
            }
            


            CalendarModel calendarModel = new CalendarModel //new CalendarModel();
            {
                daysInMonth = _daysInMonth,
                dayOfWeek = _dayOfWeek,
                numberOfLines = _numberOfLines,
                currentDay = day,
                currentMonth = month,
                currentYear = year,
                nameMonth = _nameMonth,
                nextDate = CalcNextDate(year, month, day),
                prevDate = CalcPrevDate(year, month, day),
                curentDate = _curentDate,
                eventNameList = list
            };

            return calendarModel;
        }



        // Вспомогат. метод вычисляет следующую дату
        private DateTime CalcNextDate(int year, int month, int day)
        {
            DateTime currentDate = new DateTime(year, month, day);
            DateTime nextDate = currentDate.AddMonths(1);
            return nextDate;
        }
        // Вспомогат. метод вычисляет предыдущую дату
        private DateTime CalcPrevDate(int year, int month, int day)
        {
            DateTime currentDate = new DateTime(year, month, day);
            DateTime prevDate = currentDate.AddMonths(-1);
            return prevDate;
        }

        // Метод выводит основную страницу 
        [HttpGet]
        [Authorize(Roles = "UserRead, Admin, UserReadAndWrite")]
        public ActionResult Logic()
        {
            LdapDbContext context = new LdapDbContext();
            List<Event> events =
                (from item in context.Events
                 orderby ((DateTime)item.StartTime).ToString()
                 select item).ToList();  // Получить все события из таблицы Events сортированные по дате

            EventManagement ev = new EventManagement();
            List<Event> listEvents = ev.GetAllEvents(DateTime.Today);

            CalendarModel currentCalendarModel = getModel(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            return View(new LogicModel { eventList = listEvents, calendarModel = currentCalendarModel });
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


        //// Метод записывает в бд и выдаёт true или выдаёт false
        //private bool CheckFreetime(EventModels model)
        //{
        //    EventManagement eventManagment = new EventManagement();
        //    IEnumerable<Event> listAllEventsDay = eventManagment.GetAllEvents(model.StartEvent.Date);// получаем все события на этот день

        //    //if (eventManagment.busyStartTime(listAllEventsDay, model.StartEvent)&&(eventManagment.busyEndTime(listAllEventsDay, model.EndEvent)))
        //    //{
        //    //        UserInfo userInfo = new UserInfo();
        //    //        int userId = userInfo.getCurrentUserId();
        //    //        eventManagment.WriteToDatabase(model.NameEvent, model.StartEvent, model.EndEvent, userId);
        //    //        return true;
        //    //}
        //    //else
        //    //{
        //    //    return false;
        //    //}

        //    if(eventManagment.IsFreeTime(listAllEventsDay, model.StartEvent, model.EndEvent)){
        //        UserInfo userInfo = new UserInfo();
        //            int userId = userInfo.getCurrentUserId();
        //            eventManagment.WriteToDatabase(model.NameEvent, model.StartEvent, model.EndEvent, userId);
        //            return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        public JsonResult AddNewEvent(EventModels model)
        {
            LdapDbContext context = new LdapDbContext();
            DateTime data = model.StartEvent.Date;
            EventManagement eventManagment = new EventManagement();
            List<Event> listAllEventsDay = eventManagment.GetAllEvents(model.StartEvent.Date); // получаем все события на этот день

            //если в этот день нет событий надо проверить нет ли в этот день событий не закончившихся со вчерашнего дня. если есть они учавствуют в проверке
            List<Event> extensionEvents = context.Events
           .Where(c => c.StartTime <= data && c.EndTime >= data).ToList();

            listAllEventsDay.AddRange(extensionEvents);
            
            
            bool result;
            if (eventManagment.IsFreeTime(listAllEventsDay, model.StartEvent, model.EndEvent))
            {
                UserInfo userInfo = new UserInfo();
                int userId = userInfo.getCurrentUserId();
                eventManagment.WriteToDatabase(model.NameEvent, model.StartEvent, model.EndEvent, userId);
                result = true;
            }
            else
            {
                result = false;
            }

            var jsondata = model.StartEvent.ToString();
            int _year = model.StartEvent.Year;
            int _month = model.StartEvent.Month;
            int _day = DateTime.Today.Day;
            return Json(new { success = result, data = jsondata, year = _year, month = _month, day = _day }, JsonRequestBehavior.AllowGet);
        }


        //метод для удаления события
        public JsonResult RemoveEvent(int event_id)
        {

            LdapDbContext context = new LdapDbContext();

            //Получаем день удаляемого события , для обновления календаря. Потом надо переписать чтобы год месяц день получать на стороне клиента , а не делать лишнее обращение к базе
            Event item = context.Events
                .Where(o => o.id == event_id)
                .FirstOrDefault();

            int _year = DateTime.Today.Year;
            int _month = DateTime.Today.Month;
            int _day = DateTime.Today.Day;
            EventManagement ev = new EventManagement();
            ev.DeleteEvent(event_id);
            return Json(new { success = true, year = _year, month = _month, day = _day });
        }



        public ActionResult SomeAction()
        {
            ViewData["Greeting"] = "Hello World!";
            return PartialView("_addEventForm");
        }
    }
}
