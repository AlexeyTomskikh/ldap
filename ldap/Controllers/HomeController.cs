namespace ldap.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // если пользователь авторизован попадаем сразу в календарь
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Calendar", "Home");
            }

            // если нет идём на страницу аутентификации
            return RedirectToAction("Login", "Account");
        }
        [Authorize]
        public ActionResult Calendar()
        {
           return View();
        }
    }
}
