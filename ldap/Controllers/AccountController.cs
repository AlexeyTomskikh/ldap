namespace ldap.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;
    using ldap.Models.ViewsFormModels;

    public class AccountController : Controller
    {
        // GET-вариант метода выдает страницу авторизации
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (Membership.ValidateUser(model.UserName, model.Password))  
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Calendar", "Home");
            }

            ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль");

            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}
