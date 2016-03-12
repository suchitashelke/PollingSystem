using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using DAL;
using System.Web.Security;
using System.Configuration;
using System.Text;
using PollSystem.Utilities;

namespace PollSystem.Controllers
{
    public class AccountController : Controller
    {
        IUserService userservice;
        IUserRoleService useroleservice;

        public AccountController(IUserService userservice, IUserRoleService useroleservice)
        {
            this.userservice = userservice;
            this.useroleservice = useroleservice;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            try
            {
                if (Request.Cookies["userInfo"] != null)
                {
                    ViewData[Constants.EmailId] = Request.Cookies["userInfo"][Constants.EmailId];
                    ViewData[Constants.Password] = Request.Cookies["userInfo"][Constants.Password];
                    ViewData[Constants.RememberMe] = Request.Cookies["userInfo"][Constants.RememberMe];
                }
                else
                    ViewData[Constants.RememberMe] = "false";
            }
            catch (Exception ex) { }
           
            return View();
        }

        [AllowAnonymous]
        public ActionResult Signup()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return PartialView("~/Views/Shared/_ForgotPassword.cshtml");
        }

        public JsonResult CheckEmailId(string emailid)
        {
            try
            {
                bool result = userservice.CheckEmailIdExists(emailid);
                return Json(result);
            }
            catch (Exception ex) { }
            return Json(false);
        }

        public ActionResult SubmitLogin(string emailid, string password, bool rememberme)
        {
            try
            {
                User user = userservice.GetUserByEmailPassword(emailid, password);

                if (user != null)
                {
                    Session[Constants.LoginUser] = user;
                    Session[Constants.Name] = user.Name;
                    FormsAuthentication.SetAuthCookie(emailid, false);

                    SaveInCookies(emailid, password, rememberme);
                    string role = useroleservice.GetRoleById(user.RoleId);

                    if (role.ToLower().Trim() == Constants.Role_Admin.ToLower().Trim())
                        return Json("/Manage/Index");

                    return Json("/Survey/Index");
                }
            }
            catch (Exception ex) { }
            
            return Json("fail");           
        }

        private void SaveInCookies(string emailid, string password, bool rememberme)
        {
            try
            {
                HttpCookie aCookie = new HttpCookie("userInfo");
                aCookie.Values[Constants.EmailId] = emailid;
                aCookie.Values[Constants.Password] = password;

                if (rememberme)
                {
                    aCookie.Values[Constants.RememberMe] = "true";
                    aCookie.Expires = DateTime.Now.AddMonths(1);
                }
                else
                {
                    aCookie.Values[Constants.RememberMe] = "false";
                    aCookie.Expires = DateTime.Now.AddHours(1);
                }

                Response.Cookies.Add(aCookie);
            }
            catch (Exception ex) { }
        }

        [Authorize]
        public ActionResult LogOut()
        {
            try
            {
                FormsAuthentication.SignOut();
                Session.Abandon();

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();

                string[] Cookies = Request.Cookies.AllKeys;
                foreach (string cookie in Cookies)
                    Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }
            catch (Exception ex) { }

            return RedirectToAction("Index", "Home");
        }

        public JsonResult submitForgotPassword(FormCollection collection)
        {
            try
            {
                if (collection != null)
                {
                    User user = userservice.GetUserDetailsByEmailId(collection["Email"]);
                    if (user != null)
                    {
                        string body = "Dear " + user.Name + ",<br /><br />Your password for Survey System is '" + user.Password + "'.<br /><br /><br />Regards,<br />Survey System.";

                        bool result = Helper.SendEmail(ConfigurationManager.AppSettings["fromEmail"].ToString(), collection["Email"].ToString(), string.Empty, string.Empty, ConfigurationManager.AppSettings["forgotPasswordEmailSubject"].ToString(), body, string.Empty);

                        return Json(result);
                    }
                }
            }
            catch (Exception ex) { }

            return Json(false);
        }

        public JsonResult submitRegistration(FormCollection collection)
        {
            try
            {
                if (collection != null)
                {
                    User user = new User();
                    user.RoleId = useroleservice.GetRoleIdByRole(Constants.Role_User);
                    user.Name = collection["Name"];
                    user.EmailId = collection["EmailId"];
                    user.Password = collection["Password"];
                    user.IsDeleted = false;

                    int result = userservice.InsertUser(user);
                    if (result > 0)
                    {
                        Session[Constants.LoginUser] = user;
                        FormsAuthentication.SetAuthCookie(user.EmailId, false);
                        SaveInCookies(user.EmailId, user.Password, false);

                        StringBuilder body = new StringBuilder();
                        body.Append("<b>Dear " + user.Name + ",</b><br/><br/>");
                        body.Append("Warm Greetings from Survey System!!<br/><br/>");
                        body.Append("We are delighted to have you on board.<br/><br/>");
                        body.Append("Survey system is a great option for people and organisations who would like to conduct their own research – it is less time consuming, cheaper, you get the results faster, and you can transfer and use the data in various applications to answer important questions.<br/><br/>");
                        body.Append("Thanks for choosing us!<br/><br/>");
                        body.Append("For any questions feel free to contact us at support@surveysystem.in<br/><br/>");
                        body.Append("<b>Discover the power of your opinion!</b><br/>");
                        body.Append("<b>Team Survey System</b><br/>");

                        Helper.SendEmail(ConfigurationManager.AppSettings["fromEmail"].ToString(), user.EmailId, string.Empty, string.Empty, ConfigurationManager.AppSettings["welcomeEmailSubject"].ToString(), body.ToString(), string.Empty);

                        return Json(true);
                    }
                }
            }
            catch (Exception ex) { }
            return Json(false);
        }
    }
}