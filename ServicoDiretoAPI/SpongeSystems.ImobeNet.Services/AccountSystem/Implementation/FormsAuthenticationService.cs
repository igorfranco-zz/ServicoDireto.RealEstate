using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpongeSolutions.ServicoDireto.Services.AccountSystem.Contracts;
using System.Web.Security;

namespace SpongeSolutions.ServicoDireto.Services.AccountSystem.Implementation
{
    public class FormsAuthenticationService : IFormsAuthenticationContract
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

            
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}
