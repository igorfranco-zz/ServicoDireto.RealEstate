using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using SpongeSolutions.Core.Attributes;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    [AllowAnonymous]
    public class ScriptController : Controller
    {
        //
        // GET: /Script/

        public ActionResult ImobeNet()
        {
            return View();
        }

    }
}
