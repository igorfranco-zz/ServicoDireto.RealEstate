using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    public class SiteBaseController : BaseController
    {
        public SiteBaseController()
        {
            //ViewBag.Purposes = SpongeSolutions.Core.Cache.CacheManager.GetInsert("Purposes", () => SpongeSolutions.ServicoDireto.Services.ServiceContext.PurposeService.GetAllActiveAsSelectList());
            //ViewBag.Filters = SpongeSolutions.Core.Cache.CacheManager.GetInsert("Filters", () => SpongeSolutions.ServicoDireto.Services.ServiceContext.FilterService.GetAllActive());
            //ViewBag.Countries = SpongeSolutions.Core.Cache.CacheManager.GetInsert("Countries", () => SpongeSolutions.ServicoDireto.Services.ServiceContext.CountryService.GetAllActive());
            //ViewBag.States = SpongeSolutions.Core.Cache.CacheManager.GetInsert("States", () => SpongeSolutions.ServicoDireto.Services.ServiceContext.StateProvinceService.GetAllActiveAsSelectList("pt-BR"));
            //ViewBag.Cities = SpongeSolutions.Core.Cache.CacheManager.GetInsert("Cities", () => SpongeSolutions.ServicoDireto.Services.ServiceContext.CityService.GetAllActiveAsSelectList("pt-BR", 1));
            //ViewBag.Categories = SpongeSolutions.Core.Cache.CacheManager.GetInsert("Categories", () => SpongeSolutions.ServicoDireto.Services.ServiceContext.PurposeService.ListCategoryAsSelectList(new int[] { }));
            //ViewBag.Types = SpongeSolutions.Core.Cache.CacheManager.GetInsert("Types", () => SpongeSolutions.ServicoDireto.Services.ServiceContext.PurposeService.ListTypeAsSelectList(-1));
        }
    }
}
