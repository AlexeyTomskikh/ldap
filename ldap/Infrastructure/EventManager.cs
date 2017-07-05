namespace ldap.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.WebPages;
    using ldap.Models;


    public class EventManager
    {
        // Метод возвращает список событий для календаря
        public List<Event> GetAllEventsOfPeriod(DateTime fromDate, DateTime toDate)
        {
            using (LdapDbContext db = new LdapDbContext())
            {
                List<Event> eventList = db.Events
                 .Where(c => c.StartTime < toDate && c.EndTime >= fromDate).OrderBy(c => c.StartTime).ToList();
                return eventList;
            }
        }

        // Метод проверяет не занято ли время
        public bool IntersectionEvents(DateTime start, DateTime end)
        {
            using (LdapDbContext db = new LdapDbContext())
            {
                bool result = db.Events.Any(a => a.StartTime < end && a.EndTime >= start);
                return result;
            }
        }


        // Метод записывает новое мероприятие в БД
        public void WriteToDatabase(string descriptionEvent, DateTime startTime, DateTime endTime, int userId)
        {
            if (descriptionEvent == null || descriptionEvent.IsEmpty())
            {
                throw new ArgumentException("Наименование события не указано", "descriptionEvent");
            }

            if (startTime == default(DateTime))
            {
                throw new ArgumentException("Не указано \"Начало события\"", "startTime");
            }

            if (endTime == default(DateTime))
            {
                throw new ArgumentException("Не указано\"Конец события\"", "endTime");
            }

            if (userId == 0)
            {
                throw new ArgumentException("Похоже, что вы не авторизованы", "userId");
            }

            using (LdapDbContext db = new LdapDbContext())
            {
                Event newEvent = new Event
                                     {
                                         DescriptionEvent = descriptionEvent,
                                         StartTime = startTime,
                                         EndTime = endTime,
                                         UserId = userId
                                     };
                db.Events.Add(newEvent);
                db.SaveChanges();
            }
        }

        // Метод для удаления события из БД
        public void DeleteEvent(int eventId)
        {
            using (LdapDbContext db = new LdapDbContext())
            {
                Event item = db.Events
                .FirstOrDefault(o => o.Id == eventId);

                if (item == null)
                {
                    throw new ArgumentException("Переданный параметр eventId не существует");
                }

                db.Events.Remove(item);
                db.SaveChanges();
            }
        }
    }
}