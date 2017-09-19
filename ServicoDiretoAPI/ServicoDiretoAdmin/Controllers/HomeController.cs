using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Admin;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    [AllowAnonymous]
    public class HomeController : SiteBaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}
