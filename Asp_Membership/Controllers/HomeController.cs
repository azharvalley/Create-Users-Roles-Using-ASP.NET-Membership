using Asp_Membership.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static Asp_Membership.Models.MyModel;

namespace Asp_Membership.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateRole()
        {

            string[] AllRoles =  Roles.GetAllRoles();

            ViewBag.AllRoles = AllRoles;

            return View();
        }

        [HttpPost]
        public ActionResult CreateRole(string a)
        {
            
            string roleName = Request.Params["txtRole"];
            if(!Roles.RoleExists(roleName))
            {
                Roles.CreateRole(roleName);
                ViewBag.Success = "Role " + "\"" + roleName + "\"" + " successfully created";
            }
            else
            {
                ViewBag.ErrorMessage = "Role " + "\"" +  roleName + "\"" + " already exists";
            }
            string[] AllRoles = Roles.GetAllRoles();
            ViewBag.AllRoles = AllRoles;
            return View();
        }


        public ActionResult CreateUser()
        {
            string[] AllRoles = Roles.GetAllRoles();
            ViewBag.AllRoles = AllRoles;

            MembershipUserCollection AllUsers = Membership.GetAllUsers();
            ViewBag.AllUsers = AllUsers;

            string UserName = Request.Params["email"];

            if (Request.Params["D"] !=null)
            {
                if(Request.Params["D"] == "1")
                {
                    ViewBag.Success = "User " + "\"" + UserName + "\"" + " removed successfully!";
                }
                else if(Request.Params["D"] == "1")
                {
                    ViewBag.ErrorMessage = "User " + "\"" + UserName + "\"" + " does not exists!";
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(string a)
        {
            string Email = Request.Params["txtEmail"];
            string Password = Request.Params["txtPassword"];
            string SecurityQuestion = Request.Params["txtSecurityQuestion"];
            string SecurityAnswer = Request.Params["txtSecurityAnswer"];
            string Role = Request.Params["drpRole"];

                       

            if(!Roles.RoleExists(Role))
            {
                ViewBag.ErrorMessage = "Role " + "\"" + Role + "\"" + " does not exists!";
            }
            else
            {
                MembershipUser user = Membership.GetUser(Email);
                if (user == null)
                {
                    MembershipCreateStatus ss;
                    Membership.CreateUser(Email, Password, Email, SecurityQuestion, SecurityAnswer, true, out ss);
                    if (ss == MembershipCreateStatus.Success)
                    {
                        user = Membership.GetUser(Email);
                        if (user != null)
                        {
                            Roles.AddUserToRole(Email, Role);
                            ViewBag.Success = "User " + "\"" + Email + "\"" + " successfully created!";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Some error occurred. Please try again!";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "User " + "\"" + Email + "\"" + " already exists!";
                }
            }

            

            string[] AllRoles = Roles.GetAllRoles();
            ViewBag.AllRoles = AllRoles;

            MembershipUserCollection AllUsers = Membership.GetAllUsers();           
            ViewBag.AllUsers = AllUsers;

            return View();
        }

        

        [HttpGet]
        public ActionResult RemoveUser(string email)
        {
            string UserName = email;

            MembershipUser user = Membership.GetUser(UserName);

            if(user!=null)
            {
                Membership.DeleteUser(UserName, true);
                Roles.DeleteRole(UserName);

                ViewBag.Success = "User " + "\"" + UserName + "\"" + " removed successfully!";

                Response.Redirect("/Home/CreateUser?email="+UserName+"&D=1");
            }
            else
            {
                ViewBag.ErrorMessage = "User " + "\"" + UserName + "\"" + " does not exists!";
                Response.Redirect("/Home/CreateUser?email=" + UserName + "&D=2");
            }


            

            return View();
        }


        


        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string returnUrl)
        {
            string Email = Request.Params["txtEmail"];
            string Password = Request.Params["txtPassword"];

            if (!String.IsNullOrEmpty(Email) && !String.IsNullOrEmpty(Password))
            {
                string Role = "";
                string[] UserRole = Roles.GetRolesForUser(Email);
                if (UserRole.Length > 0)
                {
                    Role = UserRole[0].ToString();
                }

                if (Membership.ValidateUser(Email, Password))
                {
                    FormsAuthentication.SetAuthCookie(Email, true);
                    Response.Redirect("/Home/Dashobard");
                }
                else
                {
                    ViewBag.ErrorMessage = "User " + "\"" + Email + "\"" + " does not exists!";
                }


            


            }

            return View();
        }

        [Authorize]
        public ActionResult Dashobard()
        {
            string Email = "";
            if (User.Identity.IsAuthenticated)
            {
                Email = User.Identity.Name;
            }

            ViewBag.Email = Email;

            string[] roles =  Roles.GetRolesForUser(Email);
            ViewBag.Roles = roles;

            return View();
        }

        #region Change Password Code

        [Authorize]
        public ActionResult ChangePassword(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";

            bool hasLocalAccount;
            DataTable dtUsers = DBConnect.selectUserId(User.Identity.Name);

            string userid = dtUsers.Rows[0]["UserId"].ToString();

            DataTable dtCheckAccount = DBConnect.HasLocalAccount(userid);

            if (dtCheckAccount.Rows.Count > 0)
            {
                hasLocalAccount = true;
            }
            else
            {
                hasLocalAccount = false;
            }
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("changepassword");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {
            bool hasLocalAccount;
            DataTable dtUsers = DBConnect.selectUserId(User.Identity.Name);

            string userid = dtUsers.Rows[0]["UserId"].ToString();

            DataTable dtCheckAccount = DBConnect.HasLocalAccount(userid);

            if (dtCheckAccount.Rows.Count > 0)
            {
                hasLocalAccount = true;
            }
            else
            {
                hasLocalAccount = false;
            }
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("changepassword");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        //# Method 1: Using MembershipUser virtual method to change the password.
                        //Updates the password for the membership user in the membership data store.
                        MembershipUser u = Membership.GetUser();
                        changePasswordSucceeded = u.ChangePassword(model.OldPassword, model.NewPassword);
                        //End # Method 1

                        //# Method 2: Below code is update Password using user defined Store Procedure.
                        //if (Membership.ValidateUser(User.Identity.Name, model.OldPassword))
                        //{
                        //    DBConnect.Changepassword(userid, model.NewPassword);
                        //    changePasswordSucceeded = true;
                        //}
                        //else
                        //{
                        //    changePasswordSucceeded = false;
                        //}
                        //End # Method 2

                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("changepassword", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The old password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        Membership.CreateUser(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("changepassword", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion Change Password Code

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
    }
}