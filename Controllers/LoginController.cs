using HRTool.CommanMethods;
using HRTool.DataModel;
using HRTool.Models;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
namespace HRTool.Controllers
{
    public class LoginController : Controller
    {
        EvolutionEntities _db = new EvolutionEntities();
        // GET: Login
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoginRedirect()
        {

            //return RedirectToAction("Index", "Login");
                return Redirect("~/Login");
            
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string Password = Common.Encrypt(model.Password, true);
                    var user = _db.AspNetUsers.Where(x => x.UserName == model.UserName && x.PasswordHash == Password).FirstOrDefault();
                    if (user != null)
                    {
                        SessionProxy.UserId = user.Id;
                        SessionProxy.UserName = user.UserName;
                        SessionProxy.ImageUrl = "../../Upload/Resources/" + user.image;
                        //  SessionProxy.ImageUrl = System.Web.Hosting.HostingEnvironment.MapPath("../../Upload/Resources/" + user.image);
                        if (user.SSOID.StartsWith("C"))
                        {
                            SessionProxy.IsCustomer = true;
                        }
                        else
                        {
                            SessionProxy.IsCustomer = false;
                        }
                        //Session["UserRole"] = userRole.role
                        FormsAuthentication.SetAuthCookie(user.UserName, false);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                        return View(model);
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}