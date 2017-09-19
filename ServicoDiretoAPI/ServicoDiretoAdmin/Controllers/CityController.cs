using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Services;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.Core.RestFul;
//using SpongeSolutions.Core.Attributes;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    public class CityController : BaseController
    {
        public ActionResult Index(int page = 1, bool windowMode = false, short? status = null)
        {
            this.ViewBag.WindowMode = windowMode;
            if (status.HasValue)
                return View(ServiceContext.CityService.GetByStatus(status.Value).ToList());
            else
                return View(ServiceContext.CityService.GetAll().ToList());
        }

        public ActionResult Create(int? id)
        {
            ViewBag.Countries = SpongeSolutions.ServicoDireto.Services.ServiceContext.CountryService.GetAllActive();        
            if (id.HasValue)
            {
                var city = ServiceContext.CityService.GetById(id.Value);
                //ViewBag.States = SpongeSolutions.ServicoDireto.Services.ServiceContext.StateProvinceService.GetAllActiveAsSelectList(city.IDCountry);
                return View(city);
            }
            else
            {
                ViewBag.States = SpongeSolutions.ServicoDireto.Services.ServiceContext.StateProvinceService.GetAllActiveAsSelectList(SiteContext.DefaultCountry); 
                return View();
            }
        }

        [HttpPost]
        public ActionResult Create(City city)
        {
            if (!ModelState.IsValid)
                return View();

            if (!city.IDCity.HasValue)
            {
                city.CreateDate = DateTime.Now;
                city.CreatedBy = SiteContext.ActiveUserName;
                city.ModifyDate = DateTime.Now;
                city.ModifiedBy = SiteContext.ActiveUserName;
                this.TempData["Message"] = Internationalization.Message.Record_Inserted_Successfully;
                ServiceContext.CityService.Insert(city);
            }
            else
            {
                city.ModifyDate = DateTime.Now;
                city.ModifiedBy = SiteContext.ActiveUserName;
                ServiceContext.CityService.Update(city);
                this.TempData["Message"] = Internationalization.Message.Record_Updated_Successfully;
            }
            return RedirectToAction("create", new { id = city.IDCity });
        }

        [HttpPost]
        public JsonResult Delete(int[] ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    ServiceContext.CityService.Inactivate(ids);
                    return Json(Response<dynamic>.WrapResponse(new { Deleted = true }, SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
                }
                else
                    return Json(Response<dynamic>.WrapResponse(new { Message = SpongeSolutions.ServicoDireto.Internationalization.Message.Select_Items_To_Be_Deleted }, SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
            }
            catch (Exception ex)
            {
                return Json(Response<string>.WrapResponse(null, ex.ToString(), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.Error));
            }
        }
        
        [AllowAnonymous]
        [HttpPost]
        public JsonResult ListByStateProvince(string idCountry, int idStateProvince)
        {
            try
            {
                return Json(Response<dynamic>.WrapResponse(ServiceContext.CityService.GetAllActiveAsSelectList(idCountry, idStateProvince), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
            }
            catch (Exception ex)
            {
                return Json(Response<string>.WrapResponse(null, ex.ToString(), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.Error));
            }
        }
    }
}
