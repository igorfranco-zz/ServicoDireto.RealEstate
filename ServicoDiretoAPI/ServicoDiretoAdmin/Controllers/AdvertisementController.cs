using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Services;
using SpongeSolutions.ServicoDireto.Model.Advertisement;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using SpongeSolutions.Core.RestFul;
using System.IO;
//using SpongeSolutions.Core.Attributes;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    public class AdvertisementController : BaseController
    {
        //
        // GET: /Advertisement/
        public ActionResult Index(int page = 1, bool windowMode = false, short? status = null)
        {
            this.ViewBag.WindowMode = windowMode;
            return View(ServiceContext.AdsCategoryService.GetAllExtended());
        }

        public ActionResult Create(int? id)
        {
            this.LoadViewBagData(id);
            if (id.HasValue)
            {
                var category = ServiceContext.AdsCategoryService.GetById(id.Value);
                var categoryRelation = ServiceContext.AdsCategoryService.ListRelation(id.Value).Where(p => p.IDCustomer.HasValue).FirstOrDefault();
                if (categoryRelation != null && categoryRelation.IDCustomer.HasValue)
                {
                    var customer = ServiceContext.CustomerService.GetById(categoryRelation.IDCustomer.Value);
                    if (customer != null)
                    {
                        category.IDCustomer = customer.IDCustomer.Value;
                        category.CustomerName = customer.Name;
                    }
                }
                return View(category);
            }
            else
                return View();
        }

        [HttpPost]
        public ActionResult Create(AdsCategory entity, ICollection<AdsCategoryCulture> adsCategoryCulture)
        {
            if (!ModelState.IsValid)
            {
                this.LoadViewBagData();
                return View();
            }
            else
            {
                var purposeVinculated = (Request.Form["IDPurposeVinculated"] != null) ? Request.Form["IDPurposeVinculated"].Split(',').Select(n => int.Parse(n)).ToArray() : new int[] { };
                var categoryVinculated = (Request.Form["IDHierarchyStructureCategoryVinculated"] != null) ? Request.Form["IDHierarchyStructureCategoryVinculated"].Split(',').Select(n => int.Parse(n)).ToArray() : new int[] { };
                var typeVinculated = (Request.Form["IDHierarchyStructureTypeVinculated"] != null) ? Request.Form["IDHierarchyStructureTypeVinculated"].Split(',').Select(n => int.Parse(n)).ToArray() : new int[] { };

                if (!entity.IDAdsCategory.HasValue)
                {
                    entity.CreateDate = DateTime.Now;
                    entity.CreatedBy = SiteContext.ActiveUserName;
                    entity.ModifyDate = DateTime.Now;
                    entity.ModifiedBy = SiteContext.ActiveUserName;
                    entity.Status = (short)SpongeSolutions.ServicoDireto.Model.InfraStructure.Enums.StatusType.Active;
                    ServiceContext.AdsCategoryService.Insert(entity, adsCategoryCulture, purposeVinculated, categoryVinculated, typeVinculated);
                    this.TempData["Message"] = Internationalization.Message.Record_Inserted_Successfully;
                }
                else
                {
                    entity.ModifyDate = DateTime.Now;
                    entity.ModifiedBy = SiteContext.ActiveUserName;
                    ServiceContext.AdsCategoryService.Update(entity, adsCategoryCulture, purposeVinculated, categoryVinculated, typeVinculated);
                    this.TempData["Message"] = Internationalization.Message.Record_Updated_Successfully;
                }
                this.LoadViewBagData();
                return RedirectToAction("create", new { id = entity.IDAdsCategory.Value });
            }
        }

        public ActionResult Culture(int index, int? idAdsCategory, string idCulture)
        {
            this.ViewBag.Index = index;
            if (idAdsCategory.HasValue)
            {
                var adsCategoryCulture = ServiceContext.AdsCategoryCultureService.GetById(idAdsCategory.Value, idCulture);
                if (adsCategoryCulture == null)
                {
                    var culture = ServiceContext.CultureService.GetById(idCulture);
                    adsCategoryCulture = new AdsCategoryCulture() { IDAdsCategory = idAdsCategory, IDCulture = idCulture, CultureName = culture.Name, IconPath = culture.IconPath };
                }
                return View(adsCategoryCulture);
            }
            else
            {
                var culture = ServiceContext.CultureService.GetById(idCulture);
                return View(new AdsCategoryCulture() { IDAdsCategory = idAdsCategory, IDCulture = idCulture, CultureName = culture.Name, IconPath = culture.IconPath });
            }
        }

        [AllowAnonymous]
        public ActionResult CategoryItems(int idAdsCategory)
        {
            //var idCustomer = Session["IDCustomer"] == null ? 0 : (int)Session["IDCustomer"];
            //var result = Services.ServiceContext.ElementService.ListByAdsCategory(idAdsCategory, idCustomer);
            //foreach (var item in result)
            //    item.DefaultPicturePath = this.DefaultPicturePath(item.IDCustomer, item.IDElement.Value);

            //return View(result);
            return View();
        }

        private string DefaultPicturePath(int idCustomer, long idElement)
        {
            string file = "";
            string dirImages = Server.MapPath(String.Format("{0}/{1}/{2}", SiteContext.UploadPath, idCustomer, idElement));

            if (Directory.Exists(dirImages))
            {
                if (string.IsNullOrEmpty(file))
                {
                    var files = Directory.GetFiles(dirImages);
                    if (files != null && files.Count() > 0)
                    {
                        file = files.Where(p => System.IO.Path.GetFileName(p).StartsWith("_default_")).FirstOrDefault();
                        if (string.IsNullOrEmpty(file))
                            file = System.IO.Path.GetFileName(files[0]); //Primeiro encontrado
                        else
                            file = System.IO.Path.GetFileName(file);
                    }
                }
            }

            if (string.IsNullOrEmpty(file))
                return String.Format("{0}/_images/blank.png", SiteContext.LayoutPath);
            else
                return String.Format("{0}/{1}/{2}/thumb/{3}", SiteContext.UploadPath, idCustomer, idElement, file);
        }

        private void LoadViewBagData(int? id = null)
        {
            ViewBag.Cultures = ServiceContext.CultureService.GetAllActive().ToList();
            if (id.HasValue)
            {
                //ViewBag.AvailablePurpose = ServiceContext.AdsCategoryService.ListAvailablePurpose(id.Value);
                ViewBag.AvailableCategory = ServiceContext.AdsCategoryService.ListAvailableCategory(id.Value);
                ViewBag.AvailableType = ServiceContext.AdsCategoryService.ListAvailableType(id.Value);

                //ViewBag.VinculatedPurpose = ServiceContext.AdsCategoryService.ListVinculatedPurpose(id.Value);
                ViewBag.VinculatedCategory = ServiceContext.AdsCategoryService.ListVinculatedCategory(id.Value);
                ViewBag.VinculatedType = ServiceContext.AdsCategoryService.ListVinculatedType(id.Value);
            }
            else
            {
                //ViewBag.AvailablePurpose = ServiceContext.AdsCategoryService.ListAvailablePurpose(-1);
                ViewBag.AvailableCategory = ServiceContext.AdsCategoryService.ListAvailableCategory(-1);
                ViewBag.AvailableType = ServiceContext.AdsCategoryService.ListAvailableType(-1);

                //ViewBag.VinculatedPurpose = new List<PurposeBasic>();
                ViewBag.VinculatedCategory = new List<HierarchyStructureBasic>();
                ViewBag.VinculatedType = new List<HierarchyStructureBasic>();
            }
        }

        [HttpPost]
        public JsonResult Delete(int[] ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    ServiceContext.AdsCategoryService.Inactivate(ids);
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
