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
    #region Validation
    public static class AccountValidation
    {
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return Message.DuplicateUserName;

                case MembershipCreateStatus.DuplicateEmail:
                    return Message.DuplicateEmail;

                case MembershipCreateStatus.InvalidPassword:
                    return Message.InvalidPassword;

                case MembershipCreateStatus.InvalidEmail:
                    return Message.InvalidEmail;

                case MembershipCreateStatus.InvalidAnswer:
                    return Message.InvalidAnswer;

                case MembershipCreateStatus.InvalidQuestion:
                    return Message.InvalidQuestion;

                case MembershipCreateStatus.InvalidUserName:
                    return Message.InvalidUserName;

                case MembershipCreateStatus.ProviderError:
                    return Message.ProviderError;

                case MembershipCreateStatus.UserRejected:
                    return Message.UserRejected;

                default:
                    return Message.UnknownError;
            }
        }
    }

    #endregion

}
