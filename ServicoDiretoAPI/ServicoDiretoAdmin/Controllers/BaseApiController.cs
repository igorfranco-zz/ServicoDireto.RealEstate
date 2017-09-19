using Newtonsoft.Json;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    public class BaseApiController : ApiController
    {
        public Customer GetAuthCustomer()
        {
            Customer customer = null;
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var claimsIdentity = ((System.Security.Claims.ClaimsIdentity)HttpContext.Current.User.Identity);
                var customerClaim = claimsIdentity.Claims.Where(p => p.Type == "id_customer").FirstOrDefault();
                if (customerClaim != null)
                {
                    var item = ServiceContext.CustomerService.GetById(int.Parse(customerClaim.Value));
                    if (item != null)
                        customer = item;
                }
            }

            return customer;
        }
    }
}
