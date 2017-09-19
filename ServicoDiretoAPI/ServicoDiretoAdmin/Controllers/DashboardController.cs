using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Services;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/
        public ActionResult Inbox()
        {
            int recordCount = 0;
            var result = ServiceContext.EmailService.GetAll(out recordCount, -1, SpongeSolutions.ServicoDireto.Admin.SiteContext.GetActiveProfile.Preferences.IDCustomer, -1, (short)SpongeSolutions.ServicoDireto.Model.InfraStructure.Enums.EmailType.Sent, null, 0, SiteContext.MaximumRows);
            if (result != null)
                result = result.Where(p => p.Read.HasValue && !p.Read.Value).ToList();

            this.ViewBag.TotalEmails = result.Count;
            return View();
        }

        public ActionResult Alert()
        {
            return View(ServiceContext.AlertService.List(idCustomer: SpongeSolutions.ServicoDireto.Admin.SiteContext.GetActiveProfile.Preferences.IDCustomer));
        }

        public ActionResult AlertElement(long idAlert)
        {
            return View(ServiceContext.ElementService.ListAlert(SpongeSolutions.ServicoDireto.Admin.SiteContext.GetActiveProfile.Preferences.IDCustomer, idAlert));
        }

    }
}
