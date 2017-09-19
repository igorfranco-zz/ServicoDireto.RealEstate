using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Services;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.Core.RestFul;
using System.IO;
using SpongeSolutions.Core.Helpers;
using SpongeSolutions.ServicoDireto.Model.AccountSystem;
using System.Web.Security;
using SpongeSolutions.Core.Exceptions;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    public class CustomerController : BaseController
    {
        public ActionResult Index(int page = 1, bool windowMode = false, short? status = null)
        {
            if (Roles.IsUserInRole("Administrator"))
            {
                this.ViewBag.WindowMode = windowMode;
                if (status.HasValue)
                    return View(ServiceContext.CustomerService.GetByStatus(status.Value).ToList());
                else
                    return View(ServiceContext.CustomerService.GetAll().ToList());
            }
            else
            {
                return RedirectToAction("create", new { id = SiteContext.GetActiveProfile.Preferences.IDCustomer });
            }
            //this.ViewBag.WindowMode = windowMode;
            //if (status.HasValue)
            //    return View(Context.CustomerService.GetByStatus(status.Value).ToList());
            //else
            //return View(Context.CustomerService.GetAll().ToList());
        }

        public ActionResult Create(int? id)
        {
            if (id.HasValue)
            {
                if (id.Value == SiteContext.GetActiveProfile.Preferences.IDCustomer || Roles.IsUserInRole("Administrator"))
                {
                    var customer = ServiceContext.CustomerService.GetById(id.Value);
                    this.LoadStateAndCities(customer);
                    return View(customer);
                }
                else
                {
                    return View("Unauthorized");
                }
            }
            else
            {
                if (Roles.IsUserInRole("Administrator"))
                {
                    this.LoadStateAndCities();
                    return View();
                }
                else
                {
                    return RedirectToAction("create", new { id = SiteContext.GetActiveProfile.Preferences.IDCustomer });
                }
            }
        }

        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                this.LoadStateAndCities(customer);
                return View(customer);
            }
            else
            {
                //Verifica se o já existe o mesmo nome de site
                if (!string.IsNullOrEmpty(customer.SiteName))
                {
                    var customerBase = Services.ServiceContext.CustomerService.GetBySiteName(customer.SiteName);
                    if (customerBase != null && customer.IDCustomer.HasValue && customerBase.IDCustomer != customer.IDCustomer.Value)
                    {
                        ModelState.AddModelError("", SpongeSolutions.ServicoDireto.Internationalization.Message.SiteName_Already_Exists);
                        this.LoadStateAndCities(customer);
                        return View(customer);
                    }
                }

                if (!customer.IDCustomer.HasValue)
                {
                    customer.UserName = SiteContext.ActiveUserName;
                    customer.CreateDate = DateTime.Now;
                    customer.CreatedBy = SiteContext.ActiveUserName;
                    customer.ModifyDate = DateTime.Now;
                    customer.ModifiedBy = SiteContext.ActiveUserName;
                    ServiceContext.CustomerService.Insert(customer);
                    this.TempData["Message"] = Internationalization.Message.Record_Inserted_Successfully;
                }
                else
                {
                    customer.ModifyDate = DateTime.Now;
                    customer.ModifiedBy = SiteContext.ActiveUserName;
                    ServiceContext.CustomerService.Update(customer);
                    this.TempData["Message"] = Internationalization.Message.Record_Updated_Successfully;
                }
                this.SavePreferences(customer);
                this.SaveUploadedFiles(customer);
                this.LoadStateAndCities(customer);
                return RedirectToAction("create", new { id = customer.IDCustomer });
            }
        }

        [HttpPost]
        public JsonResult Delete(int[] ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    ServiceContext.CustomerService.Inactivate(ids);
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

        public ActionResult Search(bool windowMode = false)
        {
            this.ViewBag.WindowMode = windowMode;
            ViewBag.RecordCount = 0;
            return View();
        }

        [HttpPost]
        public ActionResult Search(string name, int page = 1, bool windowMode = false)
        {
            page--;
            windowMode = true;
            int recordCount = 0;
            var result = (!string.IsNullOrEmpty(name)) ? ServiceContext.CustomerService.GetAll().Where(p => p.Name.Contains(name)).OrderBy(p => p.Name).ToList() : null;
            ViewBag.RecordCount = recordCount;
            ViewBag.WindowMode = windowMode;
            return View(result);
        }

        [HttpGet]
        public JsonResult AutoComplete(string name_startsWith)
        {
            List<dynamic> listResult = new List<dynamic>();
            return Json(Response<dynamic>.WrapResponse(Services.ServiceContext.CustomerService.AutoComplete(name_startsWith), Enums.ResponseStatus.OK), JsonRequestBehavior.AllowGet);
        }

        #region [Auxiliar Methods]

        private void SaveUploadedFiles(Customer customer)
        {
            string imgBasePath = String.Format("{0}/{1}", SiteContext.UploadPath, customer.IDCustomer);
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase postedFile = Request.Files[file];
                if (postedFile.ContentLength > 0)
                {
                    string newFileName = String.Format("{0}{1}", Guid.NewGuid().ToString(), System.IO.Path.GetExtension(postedFile.FileName));
                    string imagePath = Server.MapPath(String.Format(@"{0}/{1}", imgBasePath, newFileName));
                    string thumbImagePath = Server.MapPath(String.Format(@"{0}/Thumb/{1}", imgBasePath, newFileName));
                    IOHelper.CreateFile(postedFile.InputStream, imagePath);

                    //TODO:Salvar o thumb já redimensionado
                    IOHelper.CreateFile(postedFile.InputStream, thumbImagePath);
                    ImageHelper.ResizeImage(imagePath, thumbImagePath, 100, 100, true);

                    postedFile.InputStream.Close();
                    postedFile.InputStream.Dispose();
                    customer.Logo = newFileName;
                    ServiceContext.CustomerService.Update(customer);
                }
            }
        }

        public void SavePreferences(Customer customer)
        {
            var profile = CustomProfile.GetProfile(customer.UserName);
            profile.Preferences.IDCustomer = customer.IDCustomer.Value;
            //profile.Preferences.AllowShowAddress = customer.Preferences.AllowShowAddress;
            //profile.Preferences.ShowRadiusCircle = customer.Preferences.ShowRadiusCircle;
            //profile.Preferences.Radius = customer.Preferences.Radius;
            profile.Save();
        }

        private void LoadStateAndCities(Customer customer = null)
        {
            if (customer == null)
            {
                ViewBag.States = SpongeSolutions.ServicoDireto.Services.ServiceContext.StateProvinceService.GetAllActiveAsSelectList(SiteContext.DefaultCountry);
                ViewBag.Cities = SpongeSolutions.ServicoDireto.Services.ServiceContext.CityService.GetAllActiveAsSelectList(SiteContext.DefaultCountry, 0);
            }
            else
            {
                ViewBag.States = SpongeSolutions.ServicoDireto.Services.ServiceContext.StateProvinceService.GetAllActiveAsSelectList(customer.IDCountry);
                ViewBag.Cities = SpongeSolutions.ServicoDireto.Services.ServiceContext.CityService.GetAllActiveAsSelectList(customer.IDCountry, customer.IDStateProvince.Value);
            }
        }

        #endregion

    }
}
