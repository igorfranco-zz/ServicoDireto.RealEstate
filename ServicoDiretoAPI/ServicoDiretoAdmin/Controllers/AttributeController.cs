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
    public class AttributeController : BaseController
    {
        public ActionResult Index(int page = 1, bool windowMode = false, short? status = null)
        {
            page--;
            int recordCount = 0;
            this.ViewBag.WindowMode = windowMode;
            IList<Model.AttributeExtended> viewData;
                
            if (status.HasValue)
                viewData = ServiceContext.AttributeService.GetByStatus(status.Value, out recordCount, null, SiteContext.MaximumRows * page, SiteContext.MaximumRows).ToList();
            else
                viewData = ServiceContext.AttributeService.GetAll(out recordCount, null, SiteContext.MaximumRows * page, SiteContext.MaximumRows).ToList();

            this.ViewBag.RowCount = recordCount;
            return View(viewData);
        }
        
        public ActionResult Create(short? id)
        {
            this.ViewData["cultures"] = ServiceContext.CultureService.GetAllActive().ToList();
            if (id.HasValue)
                return View(ServiceContext.AttributeService.GetByExId(id.Value));
            else
                return View();
        }

        [HttpPost]
        public ActionResult Create(SpongeSolutions.ServicoDireto.Model.Attribute attribute, ICollection<AttributeCulture> attributeCulture)
        {
            if (!ModelState.IsValid)
            {
                this.ViewData["cultures"] = ServiceContext.CultureService.GetAllActive().ToList();
                return View(attribute);
            }
            else
            {
                if (!attribute.IDAttribute.HasValue)
                {
                    attribute.CreateDate = DateTime.Now;
                    attribute.CreatedBy = SiteContext.ActiveUserName;
                    attribute.ModifyDate = DateTime.Now;
                    attribute.ModifiedBy = SiteContext.ActiveUserName;
                    ServiceContext.AttributeService.Insert(attribute, attributeCulture);
                    this.TempData["Message"] = Internationalization.Message.Record_Inserted_Successfully;
                }
                else
                {
                    attribute.ModifyDate = DateTime.Now;
                    attribute.ModifiedBy = SiteContext.ActiveUserName;
                    ServiceContext.AttributeService.Update(attribute, attributeCulture);
                    this.TempData["Message"] = Internationalization.Message.Record_Updated_Successfully;
                }
                return RedirectToAction("create", new { id = attribute.IDAttribute.Value });
            }
        }

        [HttpPost]
        public JsonResult Delete(int[] ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    ServiceContext.AttributeService.Inactivate(ids);
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
            return Json(Response<dynamic>.WrapResponse(Services.ServiceContext.AttributeService.AutoComplete(name_startsWith), Enums.ResponseStatus.OK), JsonRequestBehavior.AllowGet);
        }
    }
}
