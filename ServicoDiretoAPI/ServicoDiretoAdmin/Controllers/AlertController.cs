using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services;
using SpongeSolutions.Core.RestFul;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    public class AlertController : BaseController
    {
        public ActionResult Index(bool windowMode = false)
        {
            this.ViewBag.WindowMode = windowMode;
            return View(Services.ServiceContext.AlertService.List(idCustomer: SiteContext.GetActiveProfile.Preferences.IDCustomer));
        }

        public ActionResult Create(int? id)
        {
            if (id.HasValue)
            {
                var alert = Services.ServiceContext.AlertService.GetByIdExt(id.Value);
                this.LoadViewBagData(alert);
                return View(alert);
            }
            else
            {
                this.LoadViewBagData();
                return View();
            }
        }

        [HttpPost]
        public ActionResult Create(Alert alert)
        {
            List<AlertAttribute> listAttr = new List<AlertAttribute>();
            if (!ModelState.IsValid)
                return View();

            //Atributos
            var attributes = Request.Form["IDAttribute"].Split(',');
            if (attributes != null && attributes.Count() > 0)
            {
                foreach (var item in attributes)
                {
                    var values = item.Split('|');//.Select(n => decimal.Parse(n)).ToArray();
                    listAttr.Add(new AlertAttribute() { IDAttribute = Convert.ToInt32(values[0]), InitialValue = values[1], FinalValue = values[2] });
                }
            }

            alert.IDCustomer = SiteContext.GetActiveProfile.Preferences.IDCustomer;
            if (!alert.IDAlert.HasValue)
            {                
                alert.CreateDate = DateTime.Now;
                alert.CreatedBy = SiteContext.ActiveUserName;
                alert.ModifyDate = DateTime.Now;
                alert.ModifiedBy = SiteContext.ActiveUserName;
                ServiceContext.AlertService.Insert(alert, listAttr);
                this.TempData["Message"] = Internationalization.Message.Record_Inserted_Successfully;
            }
            else
            {
                alert.ModifyDate = DateTime.Now;
                alert.ModifiedBy = SiteContext.ActiveUserName;
                ServiceContext.AlertService.Update(alert, listAttr);
                this.TempData["Message"] = Internationalization.Message.Record_Updated_Successfully;
            }
            return RedirectToAction("create", new { id = alert.IDAlert });
        }

        private void LoadViewBagData(Alert entity = null)
        {
            SelectList category = null;
            SelectList type = null;
            SelectList purpose = SpongeSolutions.ServicoDireto.Services.ServiceContext.PurposeService.GetAllActiveAsSelectList();
            var filters = Services.ServiceContext.FilterService.GetAllActive();
            if (entity == null)
            {
                ViewBag.States = SpongeSolutions.ServicoDireto.Services.ServiceContext.StateProvinceService.GetAllActiveAsSelectList(SiteContext.DefaultCountry);
                ViewBag.Cities = SpongeSolutions.ServicoDireto.Services.ServiceContext.CityService.GetAllActive(SiteContext.DefaultCountry, 0);
                category = SpongeSolutions.ServicoDireto.Services.ServiceContext.PurposeService.ListCategoryAsSelectList(new int[] { });
                type = SpongeSolutions.ServicoDireto.Services.ServiceContext.PurposeService.ListTypeAsSelectList(-1);
            }
            else
            {
                var city = ServiceContext.CityService.GetById(entity.IDCity.Value);
                var attributes = ServiceContext.AlertService.ListAttribute(entity.IDAlert.Value);
                foreach (var item in filters)
                {
                    var result = attributes.Where(p => p.IDAttribute == item.IDAttribute).FirstOrDefault();
                    if (result != null)
                    {
                        item.InitialValue = result.InitialValue.ToString().Replace(',', '.');
                        item.FinalValue = result.FinalValue.ToString().Replace(',', '.');
                    }
                }
                category = SpongeSolutions.ServicoDireto.Services.ServiceContext.PurposeService.ListCategoryAsSelectList(new int[] { entity.IDPurpose.Value });
                type = SpongeSolutions.ServicoDireto.Services.ServiceContext.PurposeService.ListTypeAsSelectList(entity.IDHierarchyStructureParent.Value);
                //ViewBag.States = SpongeSolutions.ServicoDireto.Services.ServiceContext.StateProvinceService.GetAllActiveAsSelectList(city.IDCountry);
                //ViewBag.Cities = SpongeSolutions.ServicoDireto.Services.ServiceContext.CityService.GetAllActive(city.IDCountry, city.IDStateProvince);
            }
            
            ViewBag.Purposes = purpose;
            ViewBag.Types = type;
            ViewBag.Categories = category;
            ViewBag.Filters = filters;
        }

        [HttpPost]
        public JsonResult Delete(long[] ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    ServiceContext.AlertService.Deactivate(ids);
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
