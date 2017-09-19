using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using SpongeSolutions.ServicoDireto.Services;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    public class ChartController : Controller
    {
        //
        // GET: /Chart/
        public ActionResult PageViewXDetailView(Enums.ChartType chartType)
        {            
            this.ViewBag.ChartType = chartType;
            return View(ServiceContext.ElementService.ListPageViewXDetailView(SpongeSolutions.ServicoDireto.Admin.SiteContext.GetActiveProfile.Preferences.IDCustomer));
        }
    }
}
