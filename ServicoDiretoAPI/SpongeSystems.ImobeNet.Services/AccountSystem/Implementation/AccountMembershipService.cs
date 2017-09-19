using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpongeSolutions.ServicoDireto.Services.AccountSystem.Contracts;
using System.Web.Security;
using SpongeSolutions.Core.Helpers;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Model.AccountSystem;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using SpongeSolutions.ServicoDireto.Database;
using SpongeSolutions.Core.Model;

namespace SpongeSolutions.ServicoDireto.Services.AccountSystem.Implementation
{
    public class AccountMembershipService : IMembershipContract
    {

        #region [Attributes]
        //private readonly MembershipProvider Membership;
        //private readonly RoleProvider Roles;
        #endregion

        #region [Auxiliary Methods]
        private RegisterModel CreateRegisterModel(MembershipUser user)
        {
            string password = string.Empty;
            //if (!user.IsLockedOut)
            //    password = user.GetPassword();

            return new RegisterModel()
            {
                Comment = user.Comment,
                ConfirmPassword = password,
                OldPassword = password,
                Password = password,
                CreationDate = user.CreationDate,
                Email = user.Email,
                IsApproved = user.IsApproved,
                IsLockedOut = user.IsLockedOut,
                IsOnline = user.IsOnline,
                LastActivityDate = user.LastActivityDate,
                LastLockoutDate = user.LastLockoutDate,
                LastLoginDate = user.LastLoginDate,
                LastPasswordChangedDate = user.LastPasswordChangedDate,
                UserName = user.UserName,
                Roles = Roles.GetRolesForUser(user.UserName)
            };
        }
        #endregion

        public AccountMembershipService()
           // : this(null)
        {
        }

        //public AccountMembershipService(MembershipProvider provider)
        //{
        //    //Membership = provider ?? Membership.Provider;
        //    //Roles = Roles.Provider;
        //}

        public int MinPasswordLength
        {
            get
            {
                return Membership.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

            return Membership.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(RegisterModel register)
        {
            if (String.IsNullOrEmpty(register.UserName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(register.Password)) throw new ArgumentException("Value cannot be null or empty.", "password");
            if (String.IsNullOrEmpty(register.Email)) throw new ArgumentException("Value cannot be null or empty.", "email");

            MembershipCreateStatus status;
            Membership.CreateUser(register.UserName, register.Password, register.Email, null, null, true, null, out status);
            //Vinculando o usuario a roles selecionadas
            if (register.Roles != null)
            {
                foreach (var role in register.Roles)
                    Roles.AddUsersToRoles(new string[] { register.UserName }, new string[] { role });
            }

            return status;
        }

        public void ChangeUser(RegisterModel register)
        {
            //http://maanehunden.wordpress.com/2009/12/17/things-to-remember-when-using-membership-updateusermembershipuser-user/
            MembershipUser user = Membership.GetUser(register.UserName, register.IsOnline);
            string[] relatedRoles = Roles.GetRolesForUser(register.UserName);
            //if (Membership.ValidateUser(register.UserName, register.Password))
            //{
            user.Comment = register.Comment;
            Membership.UpdateUser(user);
            user.Email = register.Email;
            Membership.UpdateUser(user);
            user.IsApproved = true;//register.IsApproved;
            Membership.UpdateUser(user);
            //if (register.IsLockedOut)
            user.UnlockUser();

            if (register.Password != null && register.Password.Length > 0 && register.OldPassword != null)
                user.ChangePassword(register.OldPassword, register.Password);

            if (relatedRoles != null && relatedRoles.Count() > 0)
                //Removendo o usuario das roles atreladas a ele.
                Roles.RemoveUsersFromRoles(new string[] { register.UserName }, relatedRoles);

            if (register.Roles != null && register.Roles.Count() > 0)
                //Adicionando os papéis novamente
                foreach (var role in register.Roles)
                    Roles.AddUsersToRoles(new string[] { register.UserName }, new string[] { role });
            //Membership.UpdateUser(user);
            //}
        }

        public IEnumerable<RegisterModel> ListUsers(int startRowIndex = -1, int maximumRows = -1, string roleName = null)
        {
            int recordCount = 0;
            foreach (MembershipUser user in Membership.GetAllUsers(startRowIndex, maximumRows, out recordCount))
            {
                if ((roleName != null && Roles.IsUserInRole(user.UserName, roleName)) || roleName == null)
                    yield return this.CreateRegisterModel(user);
            }
        }

        public IEnumerable<SelectListItem> ListAllUsers()
        {
            foreach (var user in this.ListUsers(0, 9999).OrderBy(p => p.UserName))
            {
                yield return new SelectListItem() { Text = user.UserName, Value = user.UserName };
            }
        }

        public IEnumerable<RegisterModel> GetUserByName(string userName, int startRowIndex = -1, int maximumRows = -1)
        {
            int recordCount = 0;
            foreach (MembershipUser user in Membership.FindUsersByName(userName, startRowIndex, maximumRows, out recordCount))
                yield return this.CreateRegisterModel(user);
        }

        public IEnumerable<RegisterModel> GetUserByEmail(string email, int startRowIndex = -1, int maximumRows = -1)
        {
            int recordCount = 0;
            foreach (MembershipUser user in Membership.FindUsersByEmail(email, startRowIndex, maximumRows, out recordCount))
                yield return this.CreateRegisterModel(user);
        }

        public void AddUserToRoles(string[] roles, string userName)
        {
            Roles.AddUserToRoles(userName, roles);
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = Membership.GetUser(userName, true /* userIsOnline */);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

        public bool ChangePassword(string userName, string newPassword)
        {
            MembershipUser mu = Membership.GetUser(userName);
            return mu.ChangePassword(mu.ResetPassword(), newPassword);
        }

        public string[] ListRoles()
        {
            return Roles.GetAllRoles();
        }

        public bool VerifyUserExistence(string userName)
        {
            int recordCount = 0;
            return (Membership.FindUsersByName(userName, 0, 10, out recordCount).Count == 0) ? false : true;
        }

        public IEnumerable<SelectListItem> ListRelatedRoles(string userName)
        {
            if (userName != null)
                foreach (string item in Roles.GetRolesForUser(userName))
                    yield return new SelectListItem() { Text = item, Value = item };
        }

        public IEnumerable<SelectListItem> ListAvailableRoles(string userName)
        {
            foreach (string item in Roles.GetAllRoles())
                if (userName == null || !Roles.IsUserInRole(userName, item))
                    yield return new SelectListItem() { Text = item, Value = item };
        }

        public IEnumerable<CustomSelectListItem> ListRoles(string userName)
        {
            using (var context = new AspnetDBContext())
            {
                foreach (var item in from rec in context.aspnet_Roles
                                     where rec.Description == null || rec.Description.Length == 0
                                     orderby rec.RoleName
                                     select rec)
                {
                    CustomSelectListItem selectItem = new CustomSelectListItem() { Text = item.RoleName, Value = item.RoleName, Selected = (userName != null && Roles.IsUserInRole(userName, item.RoleName)) };
                    selectItem.Children = new MultiSelectList(
                        (from rec in context.aspnet_Roles
                         where rec.Description == item.RoleName
                         orderby rec.RoleName
                         select rec),
                        "RoleName",
                        "RoleName",
                        null);

                    //foreach (var child in from rec in context.aspnet_Roles
                    //                      where rec.Description == item.RoleName
                    //                      orderby rec.RoleName
                    //                      select rec)
                    //{
                    //    selectItem.Child.Add(new CustomSelectListItem() { Text = child.RoleName, Value = child.RoleName, Selected = (userName != null && Roles.IsUserInRole(userName, child.RoleName)) });
                    //}
                    yield return selectItem;
                }
            }
        }

        public void DeleteUser(string[] userName)
        {
            foreach (var item in userName)
                Membership.DeleteUser(item, true);
        }
    }
}
