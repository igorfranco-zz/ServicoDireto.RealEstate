using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Services;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.Core.RestFul;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using SpongeSolutions.Core.Helpers;
using System.IO;
using SpongeSolutions.ServicoDireto.Model.AccountSystem;
using System.Web.Security;
//using SpongeSolutions.Core.Attributes;
using SpongeSolutions.Core.Helpers.Serialization;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    public class ElementController : SiteBaseController
    {
        private string TemporaryImageBasePath()
        {
            return String.Format("{0}/{1}/{2}", SiteContext.UploadPath, SiteContext.GetActiveProfile.Preferences.IDCustomer, Session.SessionID);
        }

        private string ImageBasePath()
        {
            return (Session["IDElement"].ToString() == "0") ? this.TemporaryImageBasePath() : String.Format("{0}/{1}/{2}", SiteContext.UploadPath, SiteContext.GetActiveProfile.Preferences.IDCustomer, Session["IDElement"]);
        }

        public ActionResult Index(int page = 1, bool windowMode = false)
        {
            page--;
            int recordCount = 0;
            this.ViewBag.WindowMode = windowMode;
            IList<Model.ElementExtended> viewData;

            if (Roles.IsUserInRole("Administrator"))
                viewData = ServiceContext.ElementService.GetAll(out recordCount, startRowIndex: SiteContext.MaximumRows * page, maximumRows: SiteContext.MaximumRows).ToList();
            else
            {
                viewData = ServiceContext.ElementService.GetAll(
                    out recordCount,
                    startRowIndex: SiteContext.MaximumRows * page,
                    maximumRows: SiteContext.MaximumRows,
                    idCustomer: SiteContext.GetActiveProfile.Preferences.IDCustomer//,
                                                                                   //status: (short)SpongeSolutions.ServicoDireto.Model.InfraStructure.Enums.StatusType.Active
                ).ToList();
            }
            this.ViewBag.RowCount = recordCount;
            return View(viewData);
        }

        public ActionResult Create(long? id)
        {
            if (id.HasValue)
            {
                var element = ServiceContext.ElementService.GetByIdExtended(id.Value);
                if (element.IDCustomer == SiteContext.GetActiveProfile.Preferences.IDCustomer || Roles.IsUserInRole("Administrator"))
                {
                    Session["IDElement"] = id.Value;
                    this.LoadViewBagData(element);
                    return View(element);
                }
                else
                    return View("Unauthorized");
            }
            else
            {
                Session["IDElement"] = 0;
                this.LoadViewBagData();
                return View();
            }
        }

        [HttpPost]
        public ActionResult Create(Element entity, ICollection<ElementCulture> elementCulture, ICollection<AttributeExtended> elementAttribute)
        {
            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.Values.Where(p => p.Errors.Count > 0).ToList();
            //    this.LoadViewBagData(entity);
            //    return View();
            //}
            //else
            //{
            IList<ElementAttribute> selectedAttributes = null;
            if (elementAttribute != null)
            {
                selectedAttributes = (from ea in elementAttribute
                                      where ea.Checked == true
                                      select new ElementAttribute
                                      {
                                          IDAttribute = ea.IDAttribute.Value,
                                          Value = ea.Value
                                      }).ToList();
            }
            
            if (!entity.IDElement.HasValue)
            {
                entity.IDCustomer = SiteContext.GetActiveProfile.Preferences.IDCustomer;
                entity.CreateDate = DateTime.Now;
                entity.CreatedBy = SiteContext.ActiveUserName;
                entity.ModifyDate = DateTime.Now;
                entity.ModifiedBy = SiteContext.ActiveUserName;                
                ServiceContext.ElementService.Insert(entity, elementCulture, selectedAttributes);
                Session["IDElement"] = entity.IDElement.Value;
                //Copia as imagens do diretorio temporario para o novo com a sessão
                IOHelper.CopyFiles(Server.MapPath(this.TemporaryImageBasePath()), Server.MapPath(this.ImageBasePath()), true);

                //Setando a imagem principal
                Services.ServiceContext.ElementService.ChangeDefaultImage(entity.IDElement.Value, this.DefaultPicturePath(entity.IDCustomer, entity.IDElement.Value));
                this.TempData["Message"] = Internationalization.Message.Record_Inserted_Successfully;
            }
            else
            {
                entity.DefaultPicturePath = this.DefaultPicturePath(entity.IDCustomer, entity.IDElement.Value);
                entity.ModifyDate = DateTime.Now;
                entity.ModifiedBy = SiteContext.ActiveUserName;
                ServiceContext.ElementService.Update(entity, elementCulture, selectedAttributes);
                this.TempData["Message"] = Internationalization.Message.Record_Updated_Successfully;
            }
            this.LoadViewBagData(entity);
            return RedirectToAction("create", new { id = entity.IDElement.Value });
            //}
        }

        [HttpPost]
        public JsonResult Delete(long[] ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    ServiceContext.ElementService.Delete(ids);
                    return Json(Response<dynamic>.WrapResponse(new { Inactivated = true }, SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
                }
                else
                    return Json(Response<dynamic>.WrapResponse(new { Message = SpongeSolutions.ServicoDireto.Internationalization.Message.Select_Items_To_Be_Deleted }, SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
            }
            catch (Exception ex)
            {
                return Json(Response<string>.WrapResponse(null, ex.ToString(), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.Error));
            }
        }

        [HttpPost]
        public JsonResult Inactivate(int[] ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    ServiceContext.ElementService.Inactivate(ids);
                    return Json(Response<dynamic>.WrapResponse(new { Inactivated = true }, SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
                }
                else
                    return Json(Response<dynamic>.WrapResponse(new { Message = SpongeSolutions.ServicoDireto.Internationalization.Message.Select_Items_To_Be_Deleted }, SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
            }
            catch (Exception ex)
            {
                return Json(Response<string>.WrapResponse(null, ex.ToString(), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.Error));
            }
        }

        private void LoadViewBagData(Element element = null)
        {
            SelectList category = null;
            SelectList type = null;
            //SelectList purpose = SpongeSolutions.ServicoDireto.Services.ServiceContext.PurposeService.GetAllActiveAsSelectList();
            if (element == null)
            {
                ViewBag.States = SpongeSolutions.ServicoDireto.Services.ServiceContext.StateProvinceService.GetAllActiveAsSelectList(SiteContext.DefaultCountry);
                ViewBag.Cities = SpongeSolutions.ServicoDireto.Services.ServiceContext.CityService.GetAllActiveAsSelectList(SiteContext.DefaultCountry, 0);
                category = SpongeSolutions.ServicoDireto.Services.ServiceContext.PurposeService.ListCategoryAsSelectList(new int[] { });
                type = SpongeSolutions.ServicoDireto.Services.ServiceContext.PurposeService.ListTypeAsSelectList(-1);
            }
            else
            {
                var city = ServiceContext.CityService.GetById(element.IDCity);
                category = SpongeSolutions.ServicoDireto.Services.ServiceContext.PurposeService.ListCategoryAsSelectList(new int[] { element.IDPurpose });
                type = SpongeSolutions.ServicoDireto.Services.ServiceContext.PurposeService.ListTypeAsSelectList(element.IDHierarchyStructureParent);
                //ViewBag.States = SpongeSolutions.ServicoDireto.Services.ServiceContext.StateProvinceService.GetAllActiveAsSelectList(city.IDCountry);
                //ViewBag.Cities = SpongeSolutions.ServicoDireto.Services.ServiceContext.CityService.GetAllActiveAsSelectList(city.IDCountry, city.IDStateProvince);
            }
            //ViewBag.Purposes = purpose;
            ViewBag.Types = type;
            ViewBag.Categories = category;
            ViewBag.Cultures = ServiceContext.CultureService.GetAllActive().ToList();
        }

        public ActionResult AttributeType()
        {
            return View(ServiceContext.AttributeTypeService.GetAllActive().Where(p => p.Hidden == false).ToList());
        }

        private UploadedFile RetrieveFileFromRequest()
        {
            string filename = null;
            string fileType = null;
            byte[] fileContents = null;

            if (Request.Files.Count > 0)
            { //they're uploading the old way
                var file = Request.Files[0];
                fileContents = new byte[file.ContentLength];
                fileType = file.ContentType;
                filename = file.FileName;
            }
            else if (Request.ContentLength > 0)
            {
                fileContents = new byte[Request.ContentLength];
                Request.InputStream.Read(fileContents, 0, Request.ContentLength);
                filename = Request.Headers["X-File-Name"];
                fileType = Request.Headers["X-File-Type"];
            }

            return new UploadedFile()
            {
                Filename = filename,
                ContentType = fileType,
                FileSize = fileContents != null ? fileContents.Length : 0,
                Contents = fileContents
            };
        }

        public void SaveFile(UploadedFile file)
        {
            string basePath = this.ImageBasePath();
            string newFileName = String.Format("{0}{1}", Guid.NewGuid().ToString(), System.IO.Path.GetExtension(file.Filename));
            string imagePath = Server.MapPath(String.Format(@"{0}/{1}", basePath, newFileName));
            string thumbImagePath = Server.MapPath(String.Format(@"{0}/Thumb/{1}", basePath, newFileName));
            IOHelper.CreateFile(file.Contents, imagePath);

            ////TODO:Salvar o thumb já redimensionado
            if (file.ContentType.Contains("image"))
            {
                IOHelper.CreateFile(file.Contents, thumbImagePath);
                ImageHelper.ResizeImage(imagePath, thumbImagePath, 100, 100, true);
            }
        }

        public string[] ListFilesByPath(string path)
        {
            List<string> result = new List<string>();
            string basePath = Server.MapPath(path);
            if (Directory.Exists(basePath))
            {
                foreach (var filePath in System.IO.Directory.GetFiles(basePath, "*.*").OrderBy(p => p))
                    result.Add(new FileInfo(filePath).Name);
            }
            return result.ToArray();
        }

        public ActionResult UploadFile()
        {
            ViewBag.UploadedFiles = ListFilesByPath(this.ImageBasePath());
            ViewBag.ImageBasePath = this.ImageBasePath();
            return View();
        }

        [HttpPost]
        public JsonResult UploadFiles()
        {
            try
            {
                UploadedFile file = this.RetrieveFileFromRequest();
                SaveFile(file);
                return Json(Response<dynamic>.WrapResponse(new { Saved = true }, SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
            }
            catch (Exception ex)
            {
                return Json(Response<string>.WrapResponse(null, ex.ToString(), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.Error));
            }
        }

        [AllowAnonymous]
        public ActionResult ListItem(ElementExtended item)
        {
            item.DefaultPicturePath = this.DefaultPicturePath(item.IDCustomer, item.IDElement.Value);
            //ViewBag.BasicAttributes = Context.AttributeService.List("BASIC", item.IDElement.Value);
            return View(item);
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

        [AllowAnonymous]
        public ActionResult List(string filterSource, int page = 1)
        {
            Session["LastFilter"] = filterSource;
            page--;
            int recordCount = 0;
            ElementFilter filter = SerializationHelper.DeSerializeJSON<ElementFilter>(filterSource);
            var result = ServiceContext.ElementService.List(out recordCount, filter, null, SiteContext.MaximumRows * page, SiteContext.MaximumRows);// Context.ElementService.List(out recordCount, filter, null, 1 * page, 1); 
            ViewBag.FilterSource = filterSource;
            ViewBag.RecordCount = recordCount;
            return View(result);
        }

        [AllowAnonymous]
        public JsonResult MapItem(string filterSource)
        {
            Session["LastFilter"] = filterSource;
            int recordCount = 0;
            ElementFilter filter = SerializationHelper.DeSerializeJSON<ElementFilter>(filterSource);
            var result = from r in ServiceContext.ElementService.List(out recordCount, filter, null, 0, SiteContext.MaximumRows)
                         select new
                         {
                             IDElement = r.IDElement.Value,
                             Title = String.Format("{0}-{1}", r.IDElement.Value, r.Name),
                             Latitude = r.Latitude,
                             Longitude = r.Longitude,
                             Icon = String.Format("{0}{1}", SpongeSolutions.ServicoDireto.Admin.SiteContext.LayoutPath, r.IconPath)
                         };

            ViewBag.FilterSource = filterSource;
            ViewBag.RecordCount = recordCount;
            return Json(Response<dynamic>.WrapResponse(result, SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Map()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Details(long id, bool windowMode = false)
        {
            var element = ServiceContext.ElementService.GetByIdExtended(id);
            string path = String.Format("{0}/{1}/{2}", SiteContext.UploadPath, element.IDCustomer, element.IDElement.Value);
            ViewBag.UploadedFiles = ListFilesByPath(path);
            ViewBag.ImageBasePath = path;
            ViewBag.BasicAttributes = ServiceContext.AttributeService.List("BASIC", id);
            ViewBag.InfraStructureAttributes = ServiceContext.AttributeService.List("IE", id);
            ViewBag.AttributeType = ServiceContext.AttributeTypeService.GetAllActive().Where(p => p.Hidden == false && p.Acronym != "BASIC").OrderBy(p => p.Description).ToList();
            ViewBag.Customer = ServiceContext.CustomerService.GetById(element.IDCustomer);
            this.ViewBag.WindowMode = windowMode;
            return View(element);
        } 

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Near(long id, string filterSource)
        {
            var element = ServiceContext.ElementService.GetByIdExtended(id);
            ElementFilter filter = SerializationHelper.DeSerializeJSON<ElementFilter>(filterSource);
            filter.LatitudeBase = element.Latitude.ToString().Replace(",", ".");
            filter.LongitudeBase = element.Longitude.ToString().Replace(",", ".");
            filter.Radius = "1";//Próximo a 1 km do ponto selecionado
            return RedirectToAction("List", new { filterSource = SerializationHelper.SerializeJSON<ElementFilter>(filter) });
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Similar(long id, string filterSource)
        {
            int recordCount = 0;
            var element = ServiceContext.ElementService.GetByIdExtended(id);
            ElementFilter filter = SerializationHelper.DeSerializeJSON<ElementFilter>(filterSource);
            filter.LatitudeBase = element.Latitude.ToString().Replace(",", ".");
            filter.LongitudeBase = element.Longitude.ToString().Replace(",", ".");
            filter.Radius = "1";//Próximo a 1 km do ponto selecionado
            filter.IDCustomer = (Session["IDCustomer"] == null) ? "0" : Session["IDCustomer"].ToString();
            var result = ServiceContext.ElementService.List(out recordCount, filter, null, 0, 10);
            //Ignorando o selecionado
            if (result != null && result.Count > 0)
                result = result.Where(p => p.IDElement != id).ToList();
            return View(result);
        }

        public JsonResult AddAsFavorite(long idElement)
        {
            try
            {
                var profile = CustomProfile.GetProfile();
                if (profile.Preferences.FavoriteObject.Count(p => p == 1) == 0)
                    profile.Preferences.FavoriteObject.Add(idElement);

                profile.Save();
                return Json(Response<dynamic>.WrapResponse(new { Completed = true }, SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
            }
            catch (Exception ex)
            {
                return Json(Response<string>.WrapResponse(null, ex.ToString(), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.Error));
            }
        }

        public ActionResult ListFavorite(bool windowMode = false)
        {
            this.ViewBag.WindowMode = windowMode;
            return View(Services.ServiceContext.ElementService.ListFavorite(SiteContext.GetActiveProfile.Preferences.FavoriteObject));
        }

        public ActionResult Search(bool windowMode = false)
        {
            this.ViewBag.WindowMode = windowMode;
            ViewBag.RecordCount = 0;
            return View();
        }

        [AllowAnonymous]
        public ActionResult TopViewed()
        {
            //var idCustomer = Session["IDCustomer"] == null ? 0 : (int)Session["IDCustomer"];
            //var result = Services.ServiceContext.ElementService.ListTopViewed(idCustomer);
            //foreach (var item in result)
            //    item.DefaultPicturePath = this.DefaultPicturePath(item.IDCustomer, item.IDElement.Value);

            //return View(result);
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Search(ElementFilter elementFilter, int page = 1, bool windowMode = false)
        {
            page--;
            windowMode = true;
            int recordCount = 0;
            //elementFilter.ZeroAll();
            var result = ServiceContext.ElementService.List(out recordCount, elementFilter, null, SiteContext.MaximumRows * page, SiteContext.MaximumRows);
            ViewBag.RecordCount = recordCount;
            ViewBag.WindowMode = windowMode;
            return View(result);
        }

        [HttpPost]
        public JsonResult ListByCustomer(int idCustomer)
        {
            try
            {
                return Json(Response<dynamic>.WrapResponse(Services.ServiceContext.ElementService.ListByCustomer(idCustomer), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
            }
            catch (Exception ex)
            {
                return Json(Response<string>.WrapResponse(null, ex.ToString(), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.Error));
            }
        }

        [HttpPost]
        public JsonResult SetAsDefault(string path)
        {
            try
            {
                path = Server.MapPath(path);
                if (System.IO.File.Exists(path))
                {
                    string dirBase = Path.GetDirectoryName(path);
                    this.ChangeDefaultImage(dirBase, path);

                    dirBase = Path.Combine(dirBase, "thumb");
                    path = Path.Combine(Path.GetDirectoryName(path), "Thumb", Path.GetFileName(path));
                    string newPath = this.ChangeDefaultImage(dirBase, path);
                    //if (Session["IDElement"] != null)
                    //{
                    //    Uri fullPath = new Uri(newPath, UriKind.Absolute);
                    //    Uri relRoot = new Uri(dirBase, UriKind.Absolute);
                    //    newPath = relRoot.MakeRelativeUri(fullPath).ToString();
                    //    Services.ServiceContext.ElementService.ChangeDefaultImage(long.Parse(Session["IDElement"].ToString()), newPath);
                    //}
                }

                return Json(Response<dynamic>.WrapResponse(new { Executed = true }, SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
            }
            catch (Exception ex)
            {
                return Json(Response<string>.WrapResponse(null, ex.ToString(), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.Error));
            }
        }

        [HttpPost]
        public JsonResult UpImage(string path)
        {
            try
            {
                return Json(Response<dynamic>.WrapResponse(new { Executed = true }, SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
            }
            catch (Exception ex)
            {
                return Json(Response<string>.WrapResponse(null, ex.ToString(), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.Error));
            }
        }

        [HttpPost]
        public JsonResult DownImage(string path)
        {
            try
            {
                return Json(Response<dynamic>.WrapResponse(new { Executed = true }, SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
            }
            catch (Exception ex)
            {
                return Json(Response<string>.WrapResponse(null, ex.ToString(), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.Error));
            }
        }

        [HttpPost]
        public JsonResult DeleteImage(string path)
        {
            try
            {
                path = Server.MapPath(path);
                string thumbPath = Path.Combine(Path.GetDirectoryName(path), "Thumb", Path.GetFileName(path));
                IOHelper.DeleteFile(thumbPath);
                IOHelper.DeleteFile(path);

                return Json(Response<dynamic>.WrapResponse(new { Executed = true }, SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
            }
            catch (Exception ex)
            {
                return Json(Response<string>.WrapResponse(null, ex.ToString(), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.Error));
            }
        }

        private string ChangeDefaultImage(string dirBase, string imagePath)
        {
            foreach (var file in Directory.GetFiles(dirBase).Where(p => p.Contains("_default_")))
                System.IO.File.Move(file, Path.Combine(dirBase, file.Replace("_default_", "")));

            string newFileName = Path.Combine(dirBase, String.Format("_default_{0}", Path.GetFileName(imagePath)));
            System.IO.File.Move(imagePath, newFileName);
            return newFileName;
        }
    }
}
