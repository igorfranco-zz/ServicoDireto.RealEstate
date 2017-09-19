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
    public class PurposeController : BaseController
    {

        public ActionResult Index(int page = 1, bool windowMode = false, short? status = null)
        {
            this.ViewBag.WindowMode = windowMode;
            if (status.HasValue)
                return View(ServiceContext.PurposeService.GetByStatus(status.Value).ToList());
            else
                return View(ServiceContext.PurposeService.GetAll().ToList());
        }

        public ActionResult Create(short? id)
        {
            if (id.HasValue)
            {
                var purpose = ServiceContext.PurposeService.GetById(id.Value);
                this.LoadViewBagData(purpose);
                return View(purpose);
            }
            else
            {
                this.LoadViewBagData();
                return View();
            }
        }

        [HttpPost]
        public ActionResult Create(Purpose purpose, ICollection<PurposeCulture> purposeCulture, ICollection<SpongeSolutions.ServicoDireto.Model.HierarchyStructure> hierarchyStructure, FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                this.LoadViewBagData(purpose);
                return View(purpose);
            }

            int[] vinculatedStrucuture = new int[] { };
            if (collection["IDHierachyStructureVinculated"] != null)
                vinculatedStrucuture = collection["IDHierachyStructureVinculated"].Split(',').Select(n => int.Parse(n)).ToArray();

            if (!purpose.IDPurpose.HasValue)
            {
                purpose.CreateDate = DateTime.Now;
                purpose.CreatedBy = SiteContext.ActiveUserName;
                purpose.ModifyDate = DateTime.Now;
                purpose.ModifiedBy = SiteContext.ActiveUserName;
                ServiceContext.PurposeService.Insert(purpose, purposeCulture, vinculatedStrucuture);
                this.TempData["Message"] = Internationalization.Message.Record_Inserted_Successfully;
            }
            else
            {
                purpose.ModifyDate = DateTime.Now;
                purpose.ModifiedBy = SiteContext.ActiveUserName;
                ServiceContext.PurposeService.Update(purpose, purposeCulture, vinculatedStrucuture);
                this.TempData["Message"] = Internationalization.Message.Record_Updated_Successfully;
            }
            this.LoadViewBagData(purpose);
            return RedirectToAction("create", new { id = purpose.IDPurpose.Value });
        }

        private void LoadViewBagData(Purpose purpose = null)
        {
            ViewBag.Cultures = ServiceContext.CultureService.GetAllActive().ToList();
            if (purpose != null)
            {
                ViewBag.HierachyStructureAvailable = ServiceContext.PurposeService.ListAvailableCategoryAsSelectList(new int[] { purpose.IDPurpose.Value });
                ViewBag.HierachyStructureVinculated = ServiceContext.PurposeService.ListVinculatedCategoryAsSelectList(new int[] { purpose.IDPurpose.Value });
            }
            else
            {
                ViewBag.HierachyStructureAvailable = ServiceContext.PurposeService.ListAvailableCategoryAsSelectList(new int[] { });
                ViewBag.HierachyStructureVinculated = ServiceContext.PurposeService.ListVinculatedCategoryAsSelectList(new int[] { });
            }
        }

        [HttpPost]
        public JsonResult Delete(int[] ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    ServiceContext.PurposeService.Inactivate(ids);
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
