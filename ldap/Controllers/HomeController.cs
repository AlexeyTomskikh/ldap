using ldap.Infrastructure;
using ldap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ldap.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {   
            //если пользователь авторизован попадаем сразу в расписание
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Logic", "Logic");
            }
            // если нет идём на страницу аутентификации
            return RedirectToAction("Login", "Account");

        }

    }
}
