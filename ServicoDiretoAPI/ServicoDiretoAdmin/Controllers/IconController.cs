using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.Core.RestFul;
using SpongeSolutions.ServicoDireto.Services;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    public class IconController : BaseController
    {
        public ActionResult Index(int page = 1, bool windowMode = false, short? status = null)
        {
            page--;
            IList<Model.Icon> viewData;
            this.ViewBag.WindowMode = windowMode;

            if (status.HasValue)
                viewData = ServiceContext.IconService.GetByStatus(status.Value).ToList();
            else
                viewData = ServiceContext.IconService.GetAll().ToList();
            this.ViewBag.RowCount = viewData.Count;

            return View(viewData);
        }

        public ActionResult Create(short? id)
        {
            if (id.HasValue)
                return View(ServiceContext.IconService.GetById(id.Value));
            else
                return View();
        }

        [HttpPost]
        public ActionResult Create(Icon icon)
        {
            if (!ModelState.IsValid)
                return View();

            if (!icon.IDIcon.HasValue)
            {
                icon.CreateDate = DateTime.Now;
                icon.CreatedBy = SiteContext.ActiveUserName;
                icon.ModifyDate = DateTime.Now;
                icon.ModifiedBy = SiteContext.ActiveUserName;
                this.TempData["Message"] = Internationalization.Message.Record_Inserted_Successfully;
                ServiceContext.IconService.Insert(icon);
            }
            else
            {
                icon.ModifyDate = DateTime.Now;
                icon.ModifiedBy = SiteContext.ActiveUserName;
                ServiceContext.IconService.Update(icon);
                this.TempData["Message"] = Internationalization.Message.Record_Updated_Successfully;
            }
            return RedirectToAction("create", new { id = icon.IDIcon });
        }

        [HttpPost]
        public JsonResult Delete(int[] ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    ServiceContext.IconService.Inactivate(ids);
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

        [HttpGet]
        public JsonResult AutoComplete(string name_startsWith)
        {
            List<dynamic> listResult = new List<dynamic>();
            return Json(Response<dynamic>.WrapResponse(Services.ServiceContext.IconService.AutoComplete(name_startsWith), Enums.ResponseStatus.OK), JsonRequestBehavior.AllowGet);
        }
    }
}
