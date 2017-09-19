using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Services;
using SpongeSolutions.ServicoDireto.Model;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    public class PurposeCultureController : BaseController
    {
        //
        // GET: /Purpose/
        public ActionResult Create(int index, int? idPurpose, string idCulture)
        {
            this.ViewBag.Index = index;
            if (idPurpose.HasValue)
            {
                var item = ServiceContext.PurposeCultureService.GetById(idPurpose.Value, idCulture);
                if (item == null)
                    return View(CreatePurposeCulture(idCulture));
                else
                    return View(item);
            }
            else
            {
                return View(CreatePurposeCulture(idCulture));
            }
        }

        private PurposeCulture CreatePurposeCulture(string idCulture)
        {
            var culture = ServiceContext.CultureService.GetById(idCulture);
            return new PurposeCulture() { IDCulture = idCulture, CultureName = culture.Name, IconPath = culture.IconPath };
        }
    }
}
