using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Services;
using SpongeSolutions.ServicoDireto.Model;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    public class AttributeCultureController : BaseController
    {
        //
        // GET: /Purpose/
        public ActionResult Create(int index, int? idAttribute, string idCulture)
        {
            this.ViewBag.Index = index;
            if (idAttribute.HasValue)
            {
                var attrCulture = ServiceContext.AttributeCultureService.GetById(idAttribute.Value, idCulture);
                if (attrCulture == null)
                {
                    var culture = ServiceContext.CultureService.GetById(idCulture);
                    attrCulture = new AttributeCultureExtended() { IDAttribute = idAttribute.Value, IDCulture = idCulture, CultureName = culture.Name, IconPath = culture.IconPath };
                }
                return View(attrCulture);
            }
            else
            {
                var culture = ServiceContext.CultureService.GetById(idCulture);
                return View(new AttributeCultureExtended() { IDCulture = idCulture, CultureName = culture.Name, IconPath = culture.IconPath });
            }
        }
    }
}
