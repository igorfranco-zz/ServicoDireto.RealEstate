using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
namespace SpongeSolutions.ServicoDireto.Model.AccountSystem
{
    public class RegisterModel
    {
        public string IDCulture { get; set; }

        public string OldPassword { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "UserName")]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = null, ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Message), ErrorMessageResourceName = "InvalidEmail")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Email")]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        public string Email { get; set; }

        //[ValidatePasswordLength]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "ConfirmPassword")]
        //[Compare("Password", ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Compare_ConfirmPassword")]
        public string ConfirmPassword { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Comment")]
        public string Comment { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "CreationDate")]
        public DateTime CreationDate { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IsApproved")]
        public bool IsApproved { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IsLockedOut")]
        public bool IsLockedOut { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IsOnline")]
        public bool IsOnline { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "LastActivityDate")]
        public DateTime LastActivityDate { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "LastLockoutDate")]
        public DateTime LastLockoutDate { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "LastLoginDate")]
        public DateTime LastLoginDate { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "LastPasswordChangedDate")]
        public DateTime LastPasswordChangedDate { get; set; }

        public string[] Roles { get; set; }

        public bool AllowNewsletter { get; set; }
    }
}
