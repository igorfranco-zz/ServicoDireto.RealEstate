using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SpongeSolutions.ServicoDireto.Services.AccountSystem.Contracts;
using SpongeSolutions.ServicoDireto.Services.AccountSystem.Implementation;
using SpongeSolutions.Core.RestFul;
using System.IO;
using System.Data.OleDb;
using System.Data;
using SpongeSolutions.Core.Helpers;
using System.Xml.Linq;
using System.Text;
//using MvcSiteMapProvider;
using SpongeSolutions.ServicoDireto.Admin;
using SpongeSolutions.ServicoDireto.Model.AccountSystem;
using System.Web.Management;
////using SpongeSolutions.Core.Attributes;
using SpongeSolutions.ServicoDireto.Model;
//using Microsoft.Practices.EnterpriseLibrary.Logging;
namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    public class AccountController : Controller
    {
        public IFormsAuthenticationContract FormsService { get; set; }
        public IMembershipContract MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        [AllowAnonymous]
        public ActionResult LogOn(bool isModalWindow = false, bool isValidUser = true)
        {
            FormsService.SignOut();
            this.ViewBag.IsValidUser = isValidUser;
            this.ViewBag.IsModalWindow = isModalWindow;
            if (Request.IsAuthenticated && !this.ViewBag.IsModalWindow)
                return RedirectToAction("/index", "manager");
            else
            {
                return View();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string ReturnUrl, bool isModalWindow = false)
        {
            //System.Web.Management.SqlServices.Install("imobenet_v3", SqlFeatures.All, "Password=sa;Persist Security Info=True;User ID=sa;Initial Catalog=master;Data Source=(local)");
            this.ViewBag.IsModalWindow = isModalWindow;
            if (ModelState.IsValid)
            {
                var isValidUser = MembershipService.ValidateUser(model.UserName, model.Password);
                this.ViewBag.IsValidUser = isValidUser;
                if (isValidUser)
                {
                    //Logger.Write(String.Format("Tentativa Autenticação - Sucesso, UserName: {0}", model.UserName), "Smarttan", 10, 1, System.Diagnostics.TraceEventType.Warning);
                    //Services.ServiceContext.LogService.Register<LogOnModel>(model, BusinessEntities.Enums.ActionType.Login, this.GetType());
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("/index", "manager");
                    }
                }
                else
                {
                    //Logger.Write(String.Format("Tentativa Autenticação - Inválido, UserName: {0}", model.UserName), "Smarttan", 10, 1, System.Diagnostics.TraceEventType.Warning);
                    ModelState.AddModelError("", SpongeSolutions.ServicoDireto.Internationalization.Message.Password_Incorrect);
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            Session.Abandon();
            FormsService.SignOut();
            return RedirectToAction("/index", "Home");
        }

        public ActionResult Index()
        {
            return View(this.MembershipService.ListUsers(0, SiteContext.MaximumRows));
        }

        [HttpPost]
        public JsonResult Delete(string[] userName)
        {
            try
            {
                MembershipService.DeleteUser(userName);
                return Json(Response<dynamic>.WrapResponse(new { Deleted = true }, SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
            }
            catch (Exception ex)
            {
                return Json(Response<string>.WrapResponse(null, ex.ToString(), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.Error));
            }
        }

        [AllowAnonymous]
        public ActionResult Recover()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword(string code)
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ForgotPassword(string email, string code)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", SpongeSolutions.ServicoDireto.Internationalization.Message.Email_Empty);
            }
            else
            {
                this.SendEmailPasswordRecover(Services.ServiceContext.CustomerService.GetByUserEmail(email));
                this.TempData["Message"] = Internationalization.Message.Email_Password_Recover_Sent;
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Recover(PasswordRecoverModel passwordRecoverModel)
        {
            if (string.IsNullOrEmpty(passwordRecoverModel.Email) && string.IsNullOrEmpty(passwordRecoverModel.UserName))
            {
                ModelState.AddModelError("", SpongeSolutions.ServicoDireto.Internationalization.Message.Email_Empty);
            }
            else
            {
                RegisterModel user = null;
                if (!string.IsNullOrEmpty(passwordRecoverModel.UserName))
                    user = MembershipService.GetUserByName(passwordRecoverModel.UserName, 0, 1).FirstOrDefault();
                else
                    user = MembershipService.GetUserByEmail(passwordRecoverModel.Email, 0, 1).FirstOrDefault();

                this.SendEmailUserNameRecover(user);
                this.TempData["Message"] = Internationalization.Label.Access_Data_Recovered;
            }
            return View(passwordRecoverModel);
        }

        [AllowAnonymous]
        public ActionResult ChangePassword(string code)
        {
            var model = new ChangePasswordModel();
            if (!string.IsNullOrEmpty(code))
                model.ActivateCode = new Guid(code);

            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool passwordChanged = false;
                FormsService.SignOut();
                if (model.ActivateCode != Guid.Empty)
                {
                    var customer = Services.ServiceContext.CustomerService.GetByActivateKey(model.ActivateCode);
                    passwordChanged = (MembershipService.ChangePassword(customer.UserName, model.NewPassword));
                }
                else
                {
                    passwordChanged = (MembershipService.ChangePassword(SiteContext.ActiveUserName, model.OldPassword, model.NewPassword));
                }

                if (passwordChanged)
                {
                    this.TempData["Message"] = Internationalization.Message.Password_Changed_Successfully;
                    return RedirectToAction("index", "manager");
                }
                else
                {
                    ModelState.AddModelError("", SpongeSolutions.ServicoDireto.Internationalization.Message.Password_Invalid);
                    // If we got this far, something failed, redisplay form
                }
            }
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Create(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (!MembershipService.VerifyUserExistence(model.UserName))
                {
                    //Verificar se existem usuários com o mesmo email informado, caso afirmativo desabilita-los
                    Services.ServiceContext.CustomerService.DisableUser(model.Email);
                    foreach (var user in this.MembershipService.GetUserByEmail(model.Email, 0, 100))
                    {
                        MembershipUser membershipUser = Membership.GetUser(user.UserName, true);
                        membershipUser.IsApproved = false;
                        Membership.UpdateUser(membershipUser);
                    }

                    // Attempt to register the user
                    MembershipCreateStatus createStatus = MembershipService.CreateUser(model);
                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        //We must create a record on customer table
                        var customer = new Customer()
                        {
                            Email = model.Email,
                            IDCity = 1,
                            Name = model.UserName,
                            Address = "-",
                            Phone1 = "-",
                            AddressNumber = "-",
                            UserName = model.UserName,
                            CreateDate = DateTime.Now,
                            CreatedBy = model.UserName,
                            ModifyDate = DateTime.Now,
                            ModifiedBy = model.UserName,
                            Status = (short)Model.InfraStructure.Enums.StatusType.Inactive,
                            ActivateKey = Guid.NewGuid()
                        };
                        Services.ServiceContext.CustomerService.Insert(customer);
                        FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                        this.SavePreferences(customer);
                        MembershipUser user = Membership.GetUser(model.UserName, true);
                        user.IsApproved = false;
                        Membership.UpdateUser(user);
                        FormsService.SignOut();
                        this.SendEmailRegisterCode(customer);
                        this.TempData["Message"] = Internationalization.Message.User_Register_Code_Has_Been_Sent;
                        return RedirectToAction("/enable", "account");
                    }
                    else
                        ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
                else
                {
                    ModelState.AddModelError("", Internationalization.Message.Choose_Another_Username);
                }
            }
            // If we got this far, something failed, redisplay form
            this.ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            this.AddVinculatedAndAvailableRoles(model.UserName);
            return View();
        }

        [AllowAnonymous]
        public ActionResult Create(string id)
        {
            this.AddVinculatedAndAvailableRoles(id);
            if (id == null)
                return View();
            else
                return View(this.MembershipService.GetUserByName(id, 0, SiteContext.MaximumRows).First());
        }

        [AllowAnonymous]
        public void ChangeLanguage(string culture, string page)
        {
            SpongeSolutions.ServicoDireto.Services.ServiceContext.ActiveLanguage = culture;
            Response.Redirect(page);
        }

        [AllowAnonymous]
        public ActionResult Enable()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Enable(string code)
        {
            var customer = Services.ServiceContext.CustomerService.GetAll().Where(p => p.ActivateKey == new Guid(code)).FirstOrDefault();
            if (customer != null)
            {
                MembershipUser user = Membership.GetUser(customer.UserName, true);
                user.IsApproved = true;
                Membership.UpdateUser(user);
                customer.ModifyDate = DateTime.Now;
                customer.ModifiedBy = customer.UserName;
                customer.Status = (short)Model.InfraStructure.Enums.StatusType.Active;
                Services.ServiceContext.CustomerService.Update(customer);
                FormsService.SignIn(customer.UserName, true);
                this.TempData["Message"] = Internationalization.Label.User_Activated_Successfully;
                return RedirectToAction("/create", "customer", new { id = customer.IDCustomer });
            }
            else
            {
                this.TempData["Message"] = Internationalization.Label.User_ActivateCode_Not_Exists;
                return View();
            }
        }

        #region [Auxiliary Methods]

        public void SavePreferences(Customer customer)
        {
            var profile = CustomProfile.GetProfile(customer.UserName);
            profile.Preferences.IDCustomer = customer.IDCustomer.Value;
            profile.Save();
        }

        private void AddVinculatedAndAvailableRoles(string userName)
        {
            this.ViewData["roles"] = MembershipService.ListRoles(userName);
        }

        public void SendEmailUserNameRecover(RegisterModel user)
        {
            XDocument xml = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
            new XElement("Root",
                new XElement("Data",
                    new XElement("UserName", user.UserName),
                    new XElement("SiteName", SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SiteName),
                    new XElement("Password", user.Password),
                    new XElement("Site", SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SitePath))));

            EmailHelper.Send(user.Email, SpongeSolutions.ServicoDireto.Internationalization.Label.Recover_User_Title, Request.MapPath(String.Format("/templates/{0}/UserNameRecover.xsl", SpongeSolutions.ServicoDireto.Services.ServiceContext.ActiveLanguage)), xml);
        }

        public void SendEmailRegisterCode(Customer customer)
        {
            XDocument xml = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
               new XElement("Root",
               new XElement("Data",
                   new XElement("UserName", customer.UserName),
                   new XElement("SiteName", SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SiteName),
                   new XElement("Site", SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SitePath),
                   new XElement("ActivateKey", customer.ActivateKey))));

            //FormsAuthentication.SignOut();
            EmailHelper.Send(customer.Email, SpongeSolutions.ServicoDireto.Internationalization.Label.User_Activation, Request.MapPath(String.Format("/templates/{0}/register.xsl", SpongeSolutions.ServicoDireto.Services.ServiceContext.ActiveLanguage)), xml);
        }

        public void SendEmailPasswordRecover(Customer customer)
        {
            XDocument xml = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
               new XElement("Root",
               new XElement("Data",
                   new XElement("SiteName", SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SiteName),
                   new XElement("UserName", customer.UserName),
                   new XElement("Site", SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SitePath),
                   new XElement("ActivateKey", customer.ActivateKey))));

            EmailHelper.Send(customer.Email, SpongeSolutions.ServicoDireto.Internationalization.Label.Recover_Password_Title, Request.MapPath(String.Format("/templates/{0}/PasswordRecover.xsl", SpongeSolutions.ServicoDireto.Services.ServiceContext.ActiveLanguage)), xml);
        }

        #endregion
    }
}

