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
    public class CultureController : BaseController
    {
        public ActionResult Index(int page = 1, bool windowMode = false, short? status = null)
        {            
            page--;
            this.ViewBag.WindowMode = windowMode;
            
            var result = ServiceContext.CultureService.GetAll().ToList();
            return View(result);
        }

        public ActionResult Create(string id)
        {
            if (!string.IsNullOrEmpty(id))
                return View(ServiceContext.CultureService.GetById(id));
            else
                return View();
        }

        [HttpPost]
        public ActionResult Create(Culture culture)
        {
            if (!ModelState.IsValid)
                return View();

            if (!ServiceContext.CultureService.Exists(culture.IDCulture))
            {
                culture.CreateDate = DateTime.Now;
                culture.CreatedBy = SiteContext.ActiveUserName;
                culture.ModifyDate = DateTime.Now;
                culture.ModifiedBy = SiteContext.ActiveUserName;
                this.TempData["Message"] = Internationalization.Message.Record_Inserted_Successfully;
                ServiceContext.CultureService.Insert(culture);
            }
            else
            {
                culture.ModifyDate = DateTime.Now;
                culture.ModifiedBy = SiteContext.ActiveUserName;
                ServiceContext.CultureService.Update(culture);
                this.TempData["Message"] = Internationalization.Message.Record_Updated_Successfully;
            }
            return RedirectToAction("create", new { id = culture.IDCulture });
        }

        [HttpPost]
        public JsonResult Delete(string[] ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    ServiceContext.CultureService.Inactivate(ids);
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
