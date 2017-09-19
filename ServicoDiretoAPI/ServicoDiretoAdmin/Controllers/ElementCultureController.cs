using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Services;
using SpongeSolutions.ServicoDireto.Model;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    public class ElementCultureController : BaseController
    {
        public ActionResult Create(int index, int? idObject, string idCulture)
        {
            ElementCultureExtended result = null;
            this.ViewBag.Index = index;

            if (idObject.HasValue)
                result = ServiceContext.ElementCultureService.GetById(idObject.Value, idCulture);

            if (result == null)
            {
                var culture = ServiceContext.CultureService.GetById(idCulture);
                result = new ElementCultureExtended() { IDCulture = idCulture, CultureName = culture.Name, IconPath = culture.IconPath };
            }
            return View(result);
        }
    }
}
