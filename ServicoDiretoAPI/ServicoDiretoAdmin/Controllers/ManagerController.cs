using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.Core.Helpers;
using System.Web.Management;

namespace SpongeSolutions.ServicoDireto.Controllers
{
    public class ManagerController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            AssemblyInfoHelper assemblyInfoHelper = new AssemblyInfoHelper(this.GetType());
            assemblyInfoHelper.ReleaseNotes = IOHelper.GetFileContent(Server.MapPath("Release_Notes.txt"));
            return View(assemblyInfoHelper);
        }

        public ActionResult LoadingInterface(string message)
        {
            this.ViewBag.Message = message;
            return View();
        }

        public ActionResult InstallPrequisites(string db)
        {
            System.Web.Management.SqlServices.Install(db, SqlFeatures.All, "Password=sa;Persist Security Info=True;User ID=sa;Initial Catalog=master;Data Source=(local)");
            return View();
        }
    }
}
