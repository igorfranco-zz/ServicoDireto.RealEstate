using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SpongeSolutions.ServicoDireto.Internationalization;

namespace SpongeSolutions.ServicoDireto.Model.AccountSystem
{
    public enum RecoveryType : short
    {
        Password = 1,
        Username = 2        
    }

    public class PasswordRecoverModel
    {
        public string IDCulture { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "UserName")]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Email")]
        public string Email { get; set; }

        public RecoveryType Recovery { get; set; }
    }
}
