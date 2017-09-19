using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Services;
using SpongeSolutions.ServicoDireto.Model;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    public class HierarchyStructureCultureController : BaseController
    {
        public ActionResult Create(int index, int? idHierarchyStructure, string idCulture)
        {
            this.ViewBag.Index = index;
            if (idHierarchyStructure.HasValue)
                return View(ServiceContext.HierarchyStructureCultureService.GetById(idHierarchyStructure.Value, idCulture));
            else
            {
                var culture = ServiceContext.CultureService.GetById(idCulture);
                return View(new HierarchyStructureCulture() { IDCulture = idCulture, CultureName = culture.Name, IconPath = culture.IconPath });
            }
        }
    }
}
