using SpongeSolutions.Core.Helpers;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Model.AccountSystem;
using SpongeSolutions.ServicoDireto.Services;
using SpongeSolutions.ServicoDireto.Services.AccountSystem.Contracts;
using SpongeSolutions.ServicoDireto.Services.AccountSystem.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using System.Xml.Linq;
using System.Web.Http.Controllers;
using System.Web.Security;
using System.Web;
using SpongeSolutions.Core.RestFul;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    [AllowAnonymous]
    public class ApiAccountController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult CreateAccount(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (!ServiceContext.AccountMembershipService.VerifyUserExistence(model.UserName))
                {
                    //Verificar se existem usuários com o mesmo email informado, caso afirmativo desabilita-los
                    Services.ServiceContext.CustomerService.DisableUser(model.Email);
                    foreach (var user in ServiceContext.AccountMembershipService.GetUserByEmail(model.Email, 0, 100))
                    {
                        MembershipUser membershipUser = Membership.GetUser(user.UserName, true);
                        membershipUser.IsApproved = false;
                        Membership.UpdateUser(membershipUser);
                    }

                    // Attempt to register the user
                    MembershipCreateStatus createStatus = ServiceContext.AccountMembershipService.CreateUser(model);
                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        //We must create a record on customer table
                        var customer = new Customer()
                        {
                            Email = model.Email,
                            //IDCity = 1,
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

                        //todo:
                        //FormsService.SignIn(model.UserName, false /* createPersistentCookie */);

                        this.SavePreferences(customer, model.AllowNewsletter);
                        MembershipUser user = Membership.GetUser(model.UserName, true);
                        user.IsApproved = false;
                        Membership.UpdateUser(user);
                        ServiceContext.FormsAuthenticationService.SignOut();
                        this.SendEmailRegisterCode(model.IDCulture, customer);
                        //return this.Request.CreateResponse(HttpStatusCode.OK, new ResponseMessage(Internationalization.Message.User_Register_Code_Has_Been_Sent));
                        return Ok(new { Message = Internationalization.Message.User_Register_Code_Has_Been_Sent });
                    }
                    else
                        return BadRequest(AccountValidation.ErrorCodeToString(createStatus));
                    //return this.Request.CreateResponse(HttpStatusCode.NotAcceptable, AccountValidation.ErrorCodeToString(createStatus));
                }
                else
                {
                    return BadRequest(Internationalization.Message.Choose_Another_Username);
                    //return this.Request.CreateResponse(HttpStatusCode.NotAcceptable, Internationalization.Message.Choose_Another_Username);
                }
            }
            else
            {
                return BadRequest(ModelState);
                //return this.Request.CreateResponse(HttpStatusCode.NotAcceptable, Internationalization.Message.Error);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult ActivateAccount(string code)
        {
            var customer = Services.ServiceContext.CustomerService.GetByActivateKey(new Guid(code));
            if (customer != null)
            {
                MembershipUser user = Membership.GetUser(customer.UserName, true);
                if (user == null)
                    return BadRequest(Internationalization.Label.User_ActivateCode_Not_Exists);

                user.IsApproved = true;
                Membership.UpdateUser(user);
                customer.ModifyDate = DateTime.Now;
                customer.ModifiedBy = customer.UserName;
                customer.Status = (short)Model.InfraStructure.Enums.StatusType.Active;
                Services.ServiceContext.CustomerService.Update(customer);
                ServiceContext.FormsAuthenticationService.SignIn(customer.UserName, true);
                return Ok(new { Message = Internationalization.Label.User_Activated_Successfully });
            }
            else
            {
                return BadRequest(Internationalization.Label.User_ActivateCode_Not_Exists);
            }
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool passwordChanged = false;
            Services.ServiceContext.FormsAuthenticationService.SignOut();
            if (model.ActivateCode != Guid.Empty)
            {
                var customer = Services.ServiceContext.CustomerService.GetByActivateKey(model.ActivateCode);
                passwordChanged = (Services.ServiceContext.AccountMembershipService.ChangePassword(customer.UserName, model.NewPassword));
            }
            else
            {
                passwordChanged = (Services.ServiceContext.AccountMembershipService.ChangePassword(SiteContext.ActiveUserName, model.OldPassword, model.NewPassword));
            }

            if (passwordChanged)
                return Ok(new { Message = Internationalization.Message.Password_Changed_Successfully });
            else
                return BadRequest(Internationalization.Message.Password_Invalid);

        }

        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult Recover(PasswordRecoverModel passwordRecoverModel)
        {
            if (string.IsNullOrEmpty(passwordRecoverModel.Email) && string.IsNullOrEmpty(passwordRecoverModel.UserName))
                return BadRequest(SpongeSolutions.ServicoDireto.Internationalization.Message.Email_or_UserName_Empty);

            RegisterModel user = null;
            if (!string.IsNullOrEmpty(passwordRecoverModel.UserName))
                user = Services.ServiceContext.AccountMembershipService.GetUserByName(passwordRecoverModel.UserName, 0, 1).FirstOrDefault();
            else
                user = Services.ServiceContext.AccountMembershipService.GetUserByEmail(passwordRecoverModel.Email, 0, 1).FirstOrDefault();

            if (user == null)
                return BadRequest(SpongeSolutions.ServicoDireto.Internationalization.Message.User_Not_Found);

            if (passwordRecoverModel.Recovery == RecoveryType.Username)
            {
                this.SendEmailUserNameRecover(passwordRecoverModel.IDCulture, user);
                return Ok(new { Message = Internationalization.Label.Access_Data_Recovered });
            }
            else
            {
                this.SendEmailPasswordRecover(passwordRecoverModel.IDCulture, Services.ServiceContext.CustomerService.GetByUserEmail(user.Email));
                return Ok(new { Message = Internationalization.Message.Email_Password_Recover_Sent });
            }
        }

        #region [Auxiliary Methods]

        public void SavePreferences(Customer customer, bool allowNewsletter)
        {
            var profile = CustomProfile.GetProfile(customer.UserName);
            profile.Preferences.IDCustomer = customer.IDCustomer.Value;
            profile.Preferences.AllowNewsletter = allowNewsletter;
            profile.Save();
        }

        public void SendEmailUserNameRecover(string idCulture, RegisterModel user)
        {
            XDocument xml = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
            new XElement("Root",
                new XElement("Data",
                    new XElement("UserName", user.UserName),
                    new XElement("SiteName", SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SiteName),
                    new XElement("Password", user.Password),
                    new XElement("Site", SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SitePath))));

            EmailHelper.Send(user.Email, SpongeSolutions.ServicoDireto.Internationalization.Label.Recover_User_Title, HttpContext.Current.Request.MapPath(String.Format("/templates/{0}/UserNameRecover.xsl", idCulture)), xml);
        }

        public void SendEmailRegisterCode(string idCulture, Customer customer)
        {
            XDocument xml = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
               new XElement("Root",
               new XElement("Data",
                   new XElement("UserName", customer.UserName),
                   new XElement("SiteName", SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SiteName),
                   new XElement("Site", SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SitePath),
                   new XElement("ActivateKey", customer.ActivateKey))));

            //FormsAuthentication.SignOut();
            EmailHelper.Send(customer.Email, SpongeSolutions.ServicoDireto.Internationalization.Label.User_Activation, HttpContext.Current.Request.MapPath(String.Format("/templates/{0}/register.xsl", idCulture)), xml);
        }

        public void SendEmailPasswordRecover(string idCulture, Customer customer)
        {
            XDocument xml = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
               new XElement("Root",
               new XElement("Data",
                   new XElement("SiteName", SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SiteName),
                   new XElement("UserName", customer.UserName),
                   new XElement("Site", SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SitePath),
                   new XElement("ActivateKey", customer.ActivateKey))));

            EmailHelper.Send(customer.Email, SpongeSolutions.ServicoDireto.Internationalization.Label.Recover_Password_Title, HttpContext.Current.Request.MapPath(String.Format("/templates/{0}/PasswordRecover.xsl", idCulture)), xml);
        }

        #endregion
    }
}
