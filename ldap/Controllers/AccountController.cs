using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ldap.Models;

namespace ldap.Controllers
{
    public class AccountController : Controller
    {

        // GET-вариант метода выдает страницу авторизации
        [HttpGet]
        public ActionResult Login(string ReturnUrl)
        {
            return View();
        }

        //POST-версия обрабатывает введенные данные, устанавливая соответствующие куки.
        [HttpPost]
        public ActionResult Login(Login model, string returnUrl)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            if (Membership.ValidateUser(model.UserName, model.Password))  // если пользователь находится в нашей базе
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                if (this.Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return this.Redirect(returnUrl);
                }

                return this.RedirectToAction("Logic", "logic");
            }
            else 
            {
                ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль");

                return this.View();
            }

            
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return this.RedirectToAction("Index", "Home");
        }
    }
}
