using Microsoft.Owin.Security.OAuth;
using SpongeSolutions.ServicoDireto.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace SpongeSolutions.ServicoDireto.Admin.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            if (!ServiceContext.AccountMembershipService.ValidateUser(context.UserName, context.Password))
            {
                context.SetError("invalid_authentication", Internationalization.Message.Invalid_Authentication);
                return;
            }

            var customer = ServiceContext.CustomerService.GetByUserName(context.UserName);
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            identity.AddClaim(new Claim("id_customer", customer.IDCustomer.ToString()));
            identity.AddClaim(new Claim("id_customer_parent", customer.IDCustomerParent.ToString()));
            identity.AddClaim(new Claim("sub", context.UserName));

            IList roles = Roles.GetRolesForUser(context.UserName);
            foreach (string role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            //identity.AddClaim(new Claim("role", "user"));
            //identity.AddClaim(new Claim(ClaimTypes.Role, "Administrador"));
            context.Validated(identity);
        }
    }
}