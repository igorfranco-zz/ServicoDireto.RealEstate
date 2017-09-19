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
    public class StateProvinceController : BaseController
    {
        public ActionResult Index(int page = 1, bool windowMode = false, short? status = null)
        {
            this.ViewBag.WindowMode = windowMode;
            if (status.HasValue)
                return View(ServiceContext.StateProvinceService.GetByStatus(status.Value).ToList());
            else
                return View(ServiceContext.StateProvinceService.GetAll().ToList());
        }

        public ActionResult Create(int? id)
        {
            if (id.HasValue)
                return View(ServiceContext.StateProvinceService.GetById(id.Value));
            else
                return View();
        }

        [HttpPost]
        public ActionResult Create(StateProvince stateProvince)
        {
            if (!ModelState.IsValid)
                return View();

            if (!stateProvince.IDStateProvince.HasValue)
            {
                stateProvince.CreateDate = DateTime.Now;
                stateProvince.CreatedBy = SiteContext.ActiveUserName;
                stateProvince.ModifyDate = DateTime.Now;
                stateProvince.ModifiedBy = SiteContext.ActiveUserName;
                this.TempData["Message"] = Internationalization.Message.Record_Inserted_Successfully;
                ServiceContext.StateProvinceService.Insert(stateProvince);
            }
            else
            {
                stateProvince.ModifyDate = DateTime.Now;
                stateProvince.ModifiedBy = SiteContext.ActiveUserName;
                ServiceContext.StateProvinceService.Update(stateProvince);
                this.TempData["Message"] = Internationalization.Message.Record_Updated_Successfully;
            }
            return RedirectToAction("create", new { id = stateProvince.IDStateProvince });
        }

        [HttpPost]
        public JsonResult Delete(int[] ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    ServiceContext.StateProvinceService.Inactivate(ids);
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
        public JsonResult ListByCountry(string idCountry)
        {
            try
            {
                return Json(Response<dynamic>.WrapResponse(ServiceContext.StateProvinceService.GetAllActiveAsSelectList(idCountry), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
            }
            catch (Exception ex)
            {
                return Json(Response<string>.WrapResponse(null, ex.ToString(), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.Error));
            }
        }
    }
}
