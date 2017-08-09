namespace ldap.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.WebPages;
    using ldap.Models;
    using ldap.Models.DataBaseModels;

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
        public void WriteToDatabase(string title, DateTime startTime, DateTime endTime, string descriptionEvent, string color, int userId, int roomId, int[] memberList)
        {
            if (title == null || title.IsEmpty())
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
                        Title = title,
                        StartTime = startTime,
                        EndTime = endTime,
                        Description = descriptionEvent,
                        Color = color,
                        UserId = userId,
                        RoomId = roomId
                    };
                    db.Events.Add(newEvent);
                    db.SaveChanges();

                    if (memberList != null)
                    {
                        WriteEventMember(memberList, newEvent.Id);
                    }
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

        // Метод возвращает всех юзеров из БД
        public List<User> GetAllUsers()
        {
            using (LdapDbContext db = new LdapDbContext())
            {
                List<User> userList = db.Users
                 .OrderBy(c => c.FirstName).ToList();
                return userList;
            }
        }

        // Метод выдаёт список имён участников события по id события
        public List<string> GetEventMembers(int eventId)
        {
            using (LdapDbContext db = new LdapDbContext())
            {
                var membersList = db.Members
                .Where(k => k.EventId == eventId)
                .Include(c => c.User).ToList();

                List<string> firstNameList = new List<string>();
                foreach (var item in membersList)
                {
                    firstNameList.Add(item.User.FirstName);
                }

                return firstNameList;
            }
        }

        // Метод добавляет участников  нового мероприятия в таблицу Members
        private static void WriteEventMember(int[] array, int eventId)
        {
            using (LdapDbContext db = new LdapDbContext())
            {

                for (int i = 0; i < array.Length; i++)
                {
                    Member newEvent = new Member { EventId = eventId, UserId = array[i], };

                    db.Members.Add(newEvent);
                    db.SaveChanges();
                }
            }
        }
    }
}