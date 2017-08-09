namespace ldap.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using ldap.Infrastructure;
    using ldap.Models;
    using ldap.Models.DataBaseModels;
    using ldap.Models.ViewsFormModels;

    public class LogicController : Controller
    {
        [Authorize]
        public JsonResult GetEvents(double start, double end)
        {
            var fromDate = ConvertFromUnixTimestamp(start);
            var toDate = ConvertFromUnixTimestamp(end);
            EventManager ev = new EventManager();
            List<Event> list = ev.GetAllEventsOfPeriod(fromDate, toDate);

            var eventList = from item in list
                            select new
                            {
                                id = item.Id,
                                title = item.Title,
                                start = item.StartTime.ToString("O"),
                                end = item.EndTime.ToString("O"),
                                descriptionEvent = item.Description,
                                color = item.Color,
                                editable = false,
                                user = item.UserId,
                                room = item.RoomId
                            };
            return Json(eventList.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, UserReadAndWrite")]
        public JsonResult AddEvent(EventModel eventModel)
        {
            EventManager eventManagment = new EventManager();
            bool result = eventManagment.IntersectionEvents(eventModel.StartTime, eventModel.EndTime); // определяем есть ли пересечение(свободно ли время)

            UserManager userInfo = new UserManager();
            int userId = userInfo.GetCurrentUserId();

            string errorMessage = string.Empty;

            if (userId == 0)
            {
                errorMessage = "Ошибка авторизациии. Добавление невозможно. Обратитесь к администратору";
                result = false;
            }
            else if (string.IsNullOrEmpty(eventModel.Title))
            {
                errorMessage = "Название события должно быть заполнено";
                result = false;
            }
            else if (eventModel.StartTime == default(DateTime))
            {
                errorMessage = "Некорректное начало события";
                result = false;
            }
            else if (eventModel.EndTime == default(DateTime))
            {
                errorMessage = "Некорректное окончание события";
                result = false;
            }
            else if (!result)
            {
                try
                {
                    eventManagment.WriteToDatabase(eventModel.Title, eventModel.StartTime, eventModel.EndTime, eventModel.Description, eventModel.Color, userId, eventModel.RoomId, eventModel.MemberList);
                    result = true;
                }
                catch (Exception e)
                {
                    result = false;
                    errorMessage = "Произошла непредвиденная ошибка";
                }
            }
            else
            {
                result = false;
                errorMessage = "Время уже занято. Попробуйте выбрать другое";
            }

            return Json(new { success = result, message = errorMessage });
        }

        [Authorize(Roles = "Admin, UserReadAndWrite")]
        public JsonResult RemoveEvent(int eventId)
        {
            bool result;
            try
            {
                EventManager ev = new EventManager();
                ev.DeleteEvent(eventId);
                result = true;
            }
            catch (Exception e)
            {
                result = false;
                throw new Exception(e.Message);
            }

            return Json(new { success = result });
        }

        // Метод отдаёт список всех юзеров из бд для формирования списка участников
        public JsonResult GetAllUsers()
        {
            EventManager ev = new EventManager();
            List<User> list = ev.GetAllUsers();

            var userList = from item in list
                            select new
                            {
                                id = item.Id,
                                firstName = item.FirstName
                            };

            return Json(userList.ToArray(), JsonRequestBehavior.AllowGet);
        }

        // Метод отдаёт список участников по id мероприятия
        public JsonResult GetMembers(int eventId)
        {
            EventManager ev = new EventManager();
            List<string> firstNameList = ev.GetEventMembers(eventId);

            var membersList = from item in firstNameList
                           select new
                           {
                               firstName = item
                           };

            return Json(membersList.ToArray(), JsonRequestBehavior.AllowGet);
        }


        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
    }
}
