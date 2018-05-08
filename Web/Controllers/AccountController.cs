using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthFramework;
using Web.Models;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            
            if (Session["user"] != null)
            {
                return View("~/Views/Account/Logged.cshtml");
            }
            
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //[AllowAnonymous]
        public ActionResult Logoff(string returnUrl)
        {

            if (Session["user"] != null)
            {
                Session["user"] = null;
                return View("~/Views/Account/LoggedOff.cshtml");
            }
            ViewBag.ReturnUrl = returnUrl;
            return Redirect("Home");
        }

    // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(BlogUser user, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            var authenticator = new Authenticator();
            if (authenticator.CanAuthenticate(user))
            {
                authenticator.Authenticate(user);
            }
            else return View("~/Views/Shared/Error.cshtml");
            ModelState.AddModelError("", "Bad Email or Password");
            Session["user"] = user;
            return View("Logged",user);
             
        }
        

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (Session["user"] != null)
            {
                return Redirect("/");
            }
            return View();
        }
        
        
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(BlogUser model)
        {
            var authenticator = new Authenticator();

            if (ModelState.IsValid & !authenticator.UserExists(model))
            {
                var user = new BlogUser()
                {
                    Username = model.Username,
                    Password = model.Password,
                    FullName = model.FullName
                };

                authenticator.Register(user);
                var contextProvider = new MysqlContextProvider();
                // we have to add blog user after authenticator.Register() because we need that user to authenticate the transaction
                using (var context = contextProvider.CreateContext<BlogContext>(user)) // authenticated by user1
                {
                    context.BlogUsers.Add(user);
                    context.SaveChanges();
                }

                // this will set user1 as the default for all following transaction authentications
                if (!authenticator.Authenticate(user))
                    throw new InvalidOperationException("This should never happen because user was registered");
                
                Session["user"] = user;
                return RedirectToAction("Index", "Account");
            }
            else
            {
                ModelState.AddModelError("Username", "Username already in use!");
                return RedirectToAction("Register", "Account");
            }
            // If we got this far, something failed, redisplay form
            return View(model);

        }
        





    }







}