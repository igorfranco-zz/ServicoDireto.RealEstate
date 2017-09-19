using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSolutions.ServicoDireto.Services.AccountSystem.Contracts
{
    public interface IFormsAuthenticationContract
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }
}
