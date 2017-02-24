using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SHEP_Platform.Models.Account;
using SHEP_Platform.Process;

namespace SHEP_Platform.Controllers
{
    public class AccountController : ControllerBase
    {
        private AuthenticationProcess AccountProcess { get; }

        public AccountController()
        {
            AccountProcess = new AuthenticationProcess();
        }
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



            var result = AccountProcess.Login(model.UserName, model.Password);

            if (result.User == null)
            {
                ViewBag.LoginTitle = "欢迎登陆本系统";
                ModelState.AddModelError(result.ErrorElement, result.ErrorMessage);
                return View(model);
            }

            FormsAuthentication.SetAuthCookie(model.UserName, true);
            var usrIdCookie = Request.Cookies?.Get("UserId") ?? new HttpCookie("UserId");
            usrIdCookie.Value = result.User.UserId.ToString();
            usrIdCookie.Expires = DateTime.Now.AddMonths(1);
            Response.AppendCookie(usrIdCookie);
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }
            return Redirect(returnUrl);
        }

        // POST: /Account/LogOff
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}