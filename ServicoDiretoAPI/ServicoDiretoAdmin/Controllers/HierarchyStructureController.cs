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
    public class HierarchyStructureController : BaseController
    {
        public ActionResult Index(int page = 1, bool windowMode = false, short? status = null)
        {
            this.ViewBag.WindowMode = windowMode;
            if (status.HasValue)
                return View(ServiceContext.HierarchyStructureService.GetByStatus(status.Value).ToList());
            else
                return View(ServiceContext.HierarchyStructureService.GetAll(idCulture: ServiceContext.ActiveLanguage));
        }

        public ActionResult Create(int? id)
        {
            this.LoadViewData((id.HasValue) ? id.Value : 0);
            if (id.HasValue)
                return View(ServiceContext.HierarchyStructureService.GetById(id.Value));
            else
                return View();
        }

        [HttpPost]
        public ActionResult Create(HierarchyStructure hierarchyStructure, ICollection<HierarchyStructureCulture> hierarchyStructureCulture, FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                this.LoadViewData((hierarchyStructure.IDHierarchyStructureParent.HasValue) ? hierarchyStructure.IDHierarchyStructureParent.Value : 0);
                return View();
            }
            else
            {

                int[] vinculatedStrucuture = new int[] { };
                if (collection["IDHierarchyStructureVinculated"] != null)
                    vinculatedStrucuture = collection["IDHierarchyStructureVinculated"].Split(',').Select(n => int.Parse(n)).ToArray();


                if (!hierarchyStructure.IDHierarchyStructure.HasValue)
                {
                    hierarchyStructure.CreateDate = DateTime.Now;
                    hierarchyStructure.CreatedBy = SiteContext.ActiveUserName;
                    hierarchyStructure.ModifyDate = DateTime.Now;
                    hierarchyStructure.ModifiedBy = SiteContext.ActiveUserName;
                    ServiceContext.HierarchyStructureService.Insert(hierarchyStructure, hierarchyStructureCulture, vinculatedStrucuture);
                    this.TempData["Message"] = Internationalization.Message.Record_Inserted_Successfully;
                }
                else
                {
                    hierarchyStructure.ModifyDate = DateTime.Now;
                    hierarchyStructure.ModifiedBy = SiteContext.ActiveUserName;
                    ServiceContext.HierarchyStructureService.Update(hierarchyStructure, hierarchyStructureCulture, vinculatedStrucuture);
                    this.TempData["Message"] = Internationalization.Message.Record_Updated_Successfully;
                }
                return RedirectToAction("create", new { id = hierarchyStructure.IDHierarchyStructure.Value });
            }
        }

        [HttpPost]
        public JsonResult Delete(int[] ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    ServiceContext.HierarchyStructureService.Inactivate(ids);
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

        public void LoadViewData(int idHierarchyStrututureParent)
        {
            this.ViewData["cultures"] = ServiceContext.CultureService.GetAllActive().ToList();
            this.ViewData["vinculatedItems"] = ServiceContext.HierarchyStructureService.ListVinculatedAsSelectList(idHierarchyStrututureParent);
            this.ViewData["availableItems"] = ServiceContext.HierarchyStructureService.ListAvailableAsSelectList(idHierarchyStrututureParent);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult ListByPurpose(int[] idPurpose)
        {
            try
            {
                return Json(Response<dynamic>.WrapResponse(ServiceContext.PurposeService.ListCategoryAsSelectList(idPurpose), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
            }
            catch (Exception ex)
            {
                return Json(Response<string>.WrapResponse(null, ex.ToString(), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.Error));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult ListByCategory(int idHierarchyStructureParent)
        {
            try
            {
                return Json(Response<dynamic>.WrapResponse(ServiceContext.PurposeService.ListTypeAsSelectList(idHierarchyStructureParent), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
            }
            catch (Exception ex)
            {
                return Json(Response<string>.WrapResponse(null, ex.ToString(), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.Error));
            }
        }

    }
}
