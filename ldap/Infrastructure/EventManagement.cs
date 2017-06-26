using ldap.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ldap.Infrastructure
{
    public class EventManagement
    {



        // Метод возвращает список всех событий в указанный день
        public List<Event> GetAllEvents(DateTime day)
        {
            LdapDbContext context = new LdapDbContext();
            DateTime data = day.Date.AddDays(1);
            List<Event> custs = context.Events
                .Where(c => c.StartTime >= day.Date && c.StartTime < data).OrderBy(c => c.StartTime).ToList();

            // Ищем события между днями и добавляем их в список
            LdapDbContext context1 = new LdapDbContext();
            DateTime prevDay = day.Date.AddDays(-1);
       
            List<Event> custs2 = context.Events
                .Where(c => c.StartTime > prevDay && c.StartTime < day.Date && c.EndTime > day).ToList();

            custs2.AddRange(custs);

            return custs2.ToList();

        }



        // Метод возвращает список всех событий за определённый период
        public IEnumerable<Event> GetAllEventsOfPeriod(DateTime start, DateTime end)
        {
            LdapDbContext context = new LdapDbContext();

            IEnumerable<Event> custs = context.Events
                .Where(c => c.StartTime >= start && c.EndTime <= end).OrderBy(c => c.StartTime);

            return custs;
        }

        //Метод определяет не попадает ли время начало в уже существующие мероприятия
        public bool busyStartTime(IEnumerable<Event> result, DateTime startTimeUser)
        {
            bool freetime = true;

            foreach (Event item in result)
            {
                //if (startTimeUser.TimeOfDay.CompareTo(item.StartTime.TimeOfDay) >= 0 && startTimeUser.TimeOfDay.CompareTo(item.EndTime.TimeOfDay) <= 0)
                //{
                //    freetime = false; //To Do ошибка время начала уже занято.выберете другое
                //}
                if (startTimeUser >= item.StartTime && startTimeUser <= item.EndTime)
                {
                    freetime = false; //To Do ошибка время начала уже занято.выберете другое
                }

            }
            return freetime;
        }

        //Метод определяет не попадает ли конец мероприятия в уже существующие мероприятия
        public bool busyEndTime(IEnumerable<Event> result, DateTime endTimeUser)
        {
            bool freetime = true;

            foreach (Event item in result)
            {
                if (endTimeUser.TimeOfDay.CompareTo(item.StartTime.TimeOfDay) >= 0 && endTimeUser.TimeOfDay.CompareTo(item.EndTime.TimeOfDay) <= 0)
                {
                    freetime = false; //To Do ошибка время окончания мероприятия уже занято.выберете другое
                }

            }
            return freetime;
        }


        //Общий метод для проверки свободного времени в графике. Выдаёт true или false
        public bool IsFreeTime(IEnumerable<Event> result, DateTime startTimeUser, DateTime endTimeUser)
        {
            bool freetime;
            EventManagement ev = new EventManagement();
            bool freeStartTime = ev.busyStartTime(result, startTimeUser);
            bool freeEndTime = ev.busyEndTime(result, endTimeUser);
            List<Event> freeTimeBetweenEvent = ev.GetAllEventsOfPeriod(startTimeUser, endTimeUser).ToList();
            if (freeTimeBetweenEvent.Count == 0 && freeStartTime && freeEndTime)
            {
                freetime = true;
            }
            else
            {
                freetime = false;
            }
            return freetime;
        }



        // Метод записывает новое мероприятие в БД
        public void WriteToDatabase(string descriptionEvent, DateTime startTime, DateTime endTime, int userId)
        {
            if (descriptionEvent != null && startTime != default(DateTime))
            {
                using (LdapDbContext db = new LdapDbContext())
                {
                    Event newEvent = new Event { DescriptionEvent = descriptionEvent, StartTime = startTime, EndTime = endTime, UserID = userId };
                    db.Events.Add(newEvent);     // добавляем их в бд
                    db.SaveChanges();
                }
            }
            else
            {
                // TO DO ошибка! Данные не введены
            }
        }

        // Метод для удаления события из БД
        public void DeleteEvent(int event_id)
        {
            LdapDbContext context = new LdapDbContext();

            Event item = context.Events
                .Where(o => o.id == event_id)
                .FirstOrDefault();

            context.Events.Remove(item);
            context.SaveChanges();
        }


        public int[] CalcRowspan(List<Event> eventList)
        {
            int[] Rowspan = new int[eventList.Count];

            for (int i = 0; i <= eventList.Count - 1; i++)
            {
                Rowspan[i] = Math.Abs(eventList.ElementAt(i).StartTime.Hour - eventList.ElementAt(i).EndTime.Hour);
            }
            return Rowspan;
        }



        // ************расфасовка событий в массив по часам ********************

        public List<Event>[] GetAllEventByTheHour(List<Event> list)
        {
            List<Event>[] array = new List<Event>[24]; // Массив c списком событий по часам
            List<Event> eventsInThisHour = new List<Event>(); // сам список событий
            List<Event> listCopy = new List<Event>(); // массив для копирования

            // Заполняем массив событиями
            for (int i = 0; i < 24; i++)
            {
                eventsInThisHour.Clear();
                foreach (var b in list)
                {
                    if (b.StartTime.Hour == i)
                    {
                        eventsInThisHour.Add(b);
                    }
                }

                if (eventsInThisHour.Count == 0)
                {
                    array[i] = null;   //если событий в этом часу не найдено кладём в этот час null
                }
                else
                {
                    listCopy = eventsInThisHour.GetRange(0, eventsInThisHour.Count);
                    array[i] = listCopy; // если найдено кладём их в этот час

                }
            }


            List<Event>[] result = new List<Event>[24];
            result = array;

            return result;
        }
    }
}