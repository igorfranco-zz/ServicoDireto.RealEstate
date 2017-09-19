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
    public class FilterController : BaseController
    {
        public ActionResult Index(int page = 1, bool windowMode = false, short? status = null)
        {
            this.ViewBag.WindowMode = windowMode;
            if (status.HasValue)
                return View(ServiceContext.FilterService.GetByStatus(status.Value).ToList());
            else
                return View(ServiceContext.FilterService.GetAll().ToList());
        }

        private void LoadViewData(int? id)
        {
            this.ViewData["availablePurposes"] = Services.ServiceContext.PurposeService.ListAvailablePurposeAsSelectList((id.HasValue) ? id.Value : 0);
            this.ViewData["vinculatedPurposes"] = Services.ServiceContext.PurposeService.ListVinculatedPurposeAsSelectList((id.HasValue) ? id.Value : 0);
        }

        public ActionResult Create(int? id)
        {
            this.LoadViewData(id);
            if (id.HasValue)
                return View(ServiceContext.FilterService.GetById(id.Value));
            else
                return View();
        }

        [HttpPost]
        public ActionResult Create(SpongeSolutions.ServicoDireto.Model.Filter entity, FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                this.LoadViewData(entity.IDFilter);
                return View();
            }
            else
            {
                int[] vinculatedPurpose = new int[] { };
                if (collection["IDPurpose"] != null)
                    vinculatedPurpose = collection["IDPurpose"].Split(',').Select(n => int.Parse(n)).ToArray();

                if (!entity.IDFilter.HasValue)
                {
                    entity.CreateDate = DateTime.Now;
                    entity.ModifyDate = DateTime.Now;
                    entity.CreatedBy = SiteContext.ActiveUserName;
                    entity.ModifiedBy = SiteContext.ActiveUserName;
                    this.TempData["Message"] = Internationalization.Message.Record_Inserted_Successfully;
                    ServiceContext.FilterService.Insert(entity, vinculatedPurpose);
                }
                else
                {
                    entity.ModifyDate = DateTime.Now;
                    entity.ModifiedBy = SiteContext.ActiveUserName;
                    ServiceContext.FilterService.Update(entity, vinculatedPurpose);
                    this.TempData["Message"] = Internationalization.Message.Record_Updated_Successfully;
                }
                return RedirectToAction("create", new { id = entity.IDFilter });
            }
        }

        [HttpPost]
        public JsonResult Delete(int[] ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    ServiceContext.FilterService.Inactivate(ids);
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
