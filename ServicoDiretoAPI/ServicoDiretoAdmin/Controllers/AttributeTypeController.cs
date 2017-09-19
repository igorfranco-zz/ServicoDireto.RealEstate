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
    public class AttributeTypeController : BaseController
    {
        public ActionResult Index(int page = 1, bool windowMode = false, short? status = null)
        {
            this.ViewBag.WindowMode = windowMode;
            if (status.HasValue)
                return View(ServiceContext.AttributeTypeService.GetByStatus(status.Value).ToList());
            else
                return View(ServiceContext.AttributeTypeService.GetAll().ToList());
        }
        public ActionResult Create(short? id)
        {
            this.LoadViewData(id);
            if (id.HasValue)
                return View(ServiceContext.AttributeTypeService.GetById(id.Value));
            else
                return View();
        }

        [HttpPost]
        public ActionResult Create(SpongeSolutions.ServicoDireto.Model.AttributeType attributeType, ICollection<AttributeTypeCulture> attributeTypeCulture, ICollection<int> IDAttribute)
        {
            if (!ModelState.IsValid)
            {
                this.LoadViewData(attributeType.IDAttributeType);
                return View();
            }
            else
            {
                //int[] attribute = new int[] { };
                //if (collection["IDAttribute"] != null)
                //    attribute = collection["IDAttribute"].Split(',').Select(n => int.Parse(n)).ToArray();

                if (!attributeType.IDAttributeType.HasValue)
                {
                    attributeType.CreateDate = DateTime.Now;
                    attributeType.CreatedBy = SiteContext.ActiveUserName;
                    attributeType.ModifyDate = DateTime.Now;
                    attributeType.ModifiedBy = SiteContext.ActiveUserName;
                    ServiceContext.AttributeTypeService.Insert(attributeType, attributeTypeCulture, IDAttribute.ToArray());
                    this.TempData["Message"] = Internationalization.Message.Record_Inserted_Successfully;
                }
                else
                {
                    attributeType.ModifyDate = DateTime.Now;
                    attributeType.ModifiedBy = SiteContext.ActiveUserName;
                    ServiceContext.AttributeTypeService.Update(attributeType, attributeTypeCulture, IDAttribute.ToArray());
                    this.TempData["Message"] = Internationalization.Message.Record_Updated_Successfully;
                }
                return RedirectToAction("create", new { id = attributeType.IDAttributeType.Value });
            }
        }

        [HttpPost]
        public JsonResult Delete(int[] ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    ServiceContext.AttributeTypeService.Inactivate(ids);
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

        public void LoadViewData(int? idAttributeType = null)
        {
            this.ViewData["cultures"] = ServiceContext.CultureService.GetAllActive().ToList();
            this.ViewData["vinculatedAttributes"] = ServiceContext.AttributeService.ListVinculatedAttributeAsSelectList(idAttributeType);
            this.ViewData["availableAttributes"] = ServiceContext.AttributeService.ListAvailableAttributeAsSelectList(idAttributeType);
        }
    }
}
