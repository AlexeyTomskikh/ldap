using ldap.Models;
using System;
using System.Collections.Generic;
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
            IEnumerable<Event> custs = context.Events
                .Where(c => c.StartTime >= day.Date && c.StartTime < data);


            return custs.ToList();
        }

       

        // Метод возвращает список всех событий за определённый период
        public IEnumerable<Event> GetAllEventsOfPeriod(DateTime start, DateTime end)
        {
            LdapDbContext context = new LdapDbContext();
            DateTime _start = start.Date;
            DateTime _end = end.Date.AddDays(1);
            IEnumerable<Event> custs = context.Events
                .Where(c => c.EndTime >= _start && c.StartTime <= _end).OrderBy(c => c.StartTime);

            return custs;
        }

        //Метод определяет не попадает ли время начало в уже существующие мероприятия
        public bool busyStartTime(IEnumerable<Event> result, DateTime startTimeUser)
        {
            bool freetime = true;

            foreach (Event item in result)
            {
                if (startTimeUser.TimeOfDay.CompareTo(item.StartTime.TimeOfDay) >= 0 && startTimeUser.TimeOfDay.CompareTo(item.EndTime.TimeOfDay) <= 0)
                {
                    freetime = false; //To Do ошибка время начала уже занято.выберете другое
                }
                else
                {
                    freetime = true; // начало мероприятия в это время возможно
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
                else
                {
                    freetime = true; // конец мероприятия в это время возможен
                }
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
        public void DeleteEvent( int event_id)
        {
            LdapDbContext context = new LdapDbContext();

            Event item = context.Events
                .Where(o => o.id == event_id)
                .FirstOrDefault();

            context.Events.Remove(item);
            context.SaveChanges();
        }

        

      
    }
}