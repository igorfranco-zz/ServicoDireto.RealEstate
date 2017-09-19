using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Services;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.Core.RestFul;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    public class CountryController : BaseController
    {
        public ActionResult Index(int page = 1, bool windowMode = false, short? status = null)
        {
            page--;
            this.ViewBag.WindowMode = windowMode;
            if (status.HasValue)
                return View(ServiceContext.CountryService.GetByStatus(status.Value).ToList());
            else
                return View(ServiceContext.CountryService.GetAll().ToList());
        }

        public ActionResult Create(string id)
        {
            if (!string.IsNullOrEmpty(id))
                return View(ServiceContext.CountryService.GetById(id));
            else
                return View();
        }

        [HttpPost]
        public ActionResult Create(Country Country)
        {
            if (!ModelState.IsValid)
                return View();

            if (!ServiceContext.CountryService.Exists(Country.IDCountry))
            {
                Country.CreateDate = DateTime.Now;
                Country.CreatedBy = SiteContext.ActiveUserName;
                Country.ModifyDate = DateTime.Now;
                Country.ModifiedBy = SiteContext.ActiveUserName;
                this.TempData["Message"] = Internationalization.Message.Record_Inserted_Successfully;
                ServiceContext.CountryService.Insert(Country);
            }
            else
            {
                Country.ModifyDate = DateTime.Now;
                Country.ModifiedBy = SiteContext.ActiveUserName;
                ServiceContext.CountryService.Update(Country);
                this.TempData["Message"] = Internationalization.Message.Record_Updated_Successfully;
            }
            return RedirectToAction("create", new { id = Country.IDCountry });
        }

        [HttpPost]
        public JsonResult Delete(string[] ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    ServiceContext.CountryService.Inactivate(ids);
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
    }
}
