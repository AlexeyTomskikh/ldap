namespace ldap.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using ldap.Infrastructure;
    using ldap.Models;
    using ldap.Models.ViewsFormModels;

    public class LogicController : Controller
    {
        [Authorize]
        public JsonResult GetCalendarEvents(double start, double end)
        {
            var fromDate = ConvertFromUnixTimestamp(start);
            var toDate = ConvertFromUnixTimestamp(end);
            EventManager ev = new EventManager();
            List<Event> list = ev.GetAllEventsOfPeriod(fromDate, toDate);

            var eventList = from item in list
                            select new
                            {
                                id = item.Id,
                                title = item.DescriptionEvent,
                                start = item.StartTime.ToString("O"),
                                end = item.EndTime.ToString("O"),
                                editable = false,
                            };
            return Json(eventList.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, UserReadAndWrite")]
        public JsonResult AddNewEvent(EventModels model)
        {
            EventManager eventManagment = new EventManager();
            bool result = eventManagment.IntersectionEvents(model.StartEvent, model.EndEvent); // определяем есть ли пересечение(свободно ли время)

            UserManager userInfo = new UserManager();
            int userId = userInfo.GetCurrentUserId();

            string errorMessage = string.Empty;

            if (userId == 0)
            {
                errorMessage = "Ошибка авторизациии. Добавление невозможно. Обратитесь к администратору";
                result = false;
            }
            else if (string.IsNullOrEmpty(model.NameEvent))
            {
                errorMessage = "Название события должно быть заполнено";
                result = false;
            }
            else if (model.StartEvent == default(DateTime))
            {
                errorMessage = "Некорректное начало события";
                result = false;
            }
            else if (model.EndEvent == default(DateTime))
            {
                errorMessage = "Некорректное окончание события";
                result = false;
            }
            else if (!result)
            {
                try
                {
                    eventManagment.WriteToDatabase(model.NameEvent, model.StartEvent, model.EndEvent, userId);
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

        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
    }
}
