using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Services;
using SpongeSolutions.ServicoDireto.Model;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    public class AttributeTypeCultureController : BaseController
    {
        //
        // GET: /Purpose/
        public ActionResult Create(int index, int? idAttributeType, string idCulture)
        {
            this.ViewBag.Index = index;
            if (idAttributeType.HasValue)
                return View(ServiceContext.AttributeTypeCultureService.GetById(idAttributeType.Value, idCulture));
            else
            {
                var culture = ServiceContext.CultureService.GetById(idCulture);
                return View(new AttributeTypeCultureExtended() { IDCulture = idCulture, CultureName = culture.Name, IconPath = culture.IconPath });
            }
        }
    }
}
