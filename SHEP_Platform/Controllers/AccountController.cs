using System;
using System.Web.Mvc;
using System.Web.Security;
using SHEP_Platform.Models.Account;
using SHEP_Platform.Process;

namespace SHEP_Platform.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.LoginTitle = "欢迎登陆本系统";
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loginProcess = new AuthenticationProcess();
            if (loginProcess.Login(model.UserName, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.UserName, true);
                return Redirect(returnUrl);
            }

            ModelState.AddModelError("", "登陆失败，请重新尝试");
            return View(model);
        }

        // POST: /Account/LogOff
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}