using Newtonsoft.Json;
using SpongeSolutions.Core.Helpers;
using SpongeSolutions.ServicoDireto.Admin.Providers;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Model.AccountSystem;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using SpongeSolutions.ServicoDireto.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    [AllowAnonymous]
    public class ApiElementController : BaseApiController
    {
        [HttpPost]
        public IHttpActionResult Import(ElementImport entity)
        {
            var element = ServiceContext.ElementService.GetByIDItemSite(entity.ID);
            if (element == null)
            {
                var customer = ServiceContext.CustomerService.GetInsert(entity.Customer.Name, "-", "-", entity.Customer.Email, entity.Customer.Phone, entity.Customer.ID, entity.Customer.Logo);
                var purpose = ServiceContext.PurposeService.GetInsert(entity.PurposeName);
                var category = ServiceContext.HierarchyStructureService.GetInsert(entity.CategoryName, purpose.IDPurpose);
                var stateProvice = ServiceContext.StateProvinceService.GetInsert(entity.StateProvinceName, entity.CountryName);
                var city = ServiceContext.CityService.GetInsert(entity.CityName, stateProvice.IDStateProvince.Value);

                element = new Element()
                {
                    IDCity = city.IDCity.Value,
                    IDCustomer = customer.IDCustomer.Value,
                    IDPurpose = purpose.IDPurpose.Value,
                    IDHierarchyStructure = category.IDHierarchyStructure.Value,
                    IDHierarchyStructureParent = category.IDHierarchyStructure.Value,
                    Address = entity.Address ?? "-",
                    AllowRatting = true,
                    CreateDate = DateTime.Now,
                    CreatedBy = "IMPORT",
                    ModifiedBy = "IMPORT",
                    ModifyDate = DateTime.Now,
                    DefaultPicturePath = entity.DefaultPicture,
                    DetailView = 0,
                    IDItemSite = entity.ID,
                    IsPromoted = false,
                    Neighborhood = entity.Neighborhood,
                    Latitude = entity.Latitude,
                    Longitude = entity.Longitude,
                    PageView = 0,
                    ShowAddress = true,
                    Url = entity.Url,
                    Status = (short)Enums.StatusType.Active
                };
            }
            //carregando cultures
            var elementCulture = new List<ElementCulture>();
            foreach (var item in ServiceContext.CultureService.GetAllActive())
            {
                elementCulture.Add(new ElementCulture()
                {
                    Description = entity.Description,
                    Name = entity.Title,
                    IDCulture = item.IDCulture
                });
            }
            //
            //carregando elementos
            var elementAttribute = new List<ElementAttribute>();
            foreach (var item in entity.Attributes)
            {
                elementAttribute.Add(new ElementAttribute()
                {
                    IDAttribute = ServiceContext.AttributeService.GetInsert(item.Name),
                    Value = item.Value
                });
            }

            //carregando images
            var elementImages = new List<string>();
            foreach (var item in entity.Images)
            {
                //if(item.IsMain)
                //    elementImages.Add(item.UrlImageMain);
                //else
                //    elementImages.Add(item.UrlImageSize4);
                elementImages.Add(item.UrlImageSize4);
            }

            if (!element.IDElement.HasValue)
            {
                ServiceContext.ElementService.Insert(element, elementCulture, elementAttribute, elementImages);
                return Ok(new { Message = "OK", Action = "Insert", id = element.IDItemSite });
            }
            else
            {
                ServiceContext.ElementService.Update(element, elementCulture, elementAttribute, elementImages);
                return Ok(new { Message = "OK", Action = "Update", id = element.IDItemSite });
            }

        }

        [Authorize]
        [HttpPost]
        public IHttpActionResult InsertUpdateElement([FromBody]ElementExtended entity)
        {
            IList<ElementAttribute> selectedAttributes = null;
            IList<ElementCulture> elementCulture = new List<ElementCulture>();
            selectedAttributes = (from ea in entity.BasicAttributes
                                  where ea.Checked == true
                                  select new ElementAttribute
                                  {
                                      IDAttribute = ea.IDAttribute.Value,
                                      Value = ea.Value
                                  }).ToList();

            Element element = new Element();
            ObjectCopier.Copy(entity, element);

            //TODO: colocar validacao server-side e imagens
            elementCulture.Add(new ElementCulture()
            {
                Description = entity.Description,
                IDCulture = entity.IDCulture,
                IDElement = entity.IDElement.HasValue ? entity.IDElement.Value : 0,
                Name = entity.Name
            });

            if (!element.IDElement.HasValue)
            {
                element.Status = (short)Model.InfraStructure.Enums.StatusType.Active;
                element.IDCustomer = this.GetAuthCustomer().IDCustomer.Value;
                element.CreateDate = DateTime.Now;
                element.CreatedBy = SiteContext.ActiveUserName;
                element.ModifyDate = DateTime.Now;
                element.ModifiedBy = SiteContext.ActiveUserName;
                ServiceContext.ElementService.Insert(element, elementCulture, selectedAttributes);

                return Ok(new
                {
                    IDElement = element.IDElement,
                    IDCustomer = element.IDCustomer,
                    Message = Internationalization.Message.Element_Inserted_Successfully
                });
            }
            else
            {
                if (CheckOwnership(element.IDCustomer))
                {
                    //entity.DefaultPicturePath = this.DefaultPicturePath(entity.IDCustomer, entity.IDElement.Value);
                    element.ModifyDate = DateTime.Now;
                    element.ModifiedBy = SiteContext.ActiveUserName;
                    ServiceContext.ElementService.Update(element, elementCulture, selectedAttributes);
                    return Ok(new
                    {
                        IDElement = element.IDElement,
                        IDCustomer = element.IDCustomer,
                        Message = Internationalization.Message.Element_Updated_Successfully
                    });
                }
                else
                {
                    return BadRequest(Internationalization.Message.Element_Does_Not_Belongs_To_Customer);
                }
            }
        }

        [HttpPost]
        public IHttpActionResult ListElement(ElementFilter filter)
        {
            int recordCount = 0;
            int maximumRows = (filter.TotalRecodsPerPage == 0) ? SiteContext.MaximumRows : filter.TotalRecodsPerPage;
            if (filter.UseInternalAuth)
                filter.IDCustomer = base.GetAuthCustomer().IDCustomer.Value.ToString();

            var result = ServiceContext.ElementService.List(out recordCount,
                                                            filter,
                                                            filter.OrderBy,
                                                            maximumRows * filter.PageIndex, maximumRows);
            if (filter.GroupIn == 0)
                return Ok(new
                {
                    RecordCount = recordCount,
                    Records = result
                });
            else
            {
                List<IEnumerable<ElementExtended>> resultAgregated = new List<IEnumerable<ElementExtended>>();
                if (result != null && result.Count() > 0)
                {
                    int totalPages = (int)(result.Count() / filter.GroupIn);
                    if (result.Count() % filter.GroupIn > 0)
                        totalPages++;

                    for (int i = 0; i < totalPages; i++)
                        resultAgregated.Add(result.Skip(i * filter.GroupIn).Take(filter.GroupIn));
                }

                return Ok(new
                {
                    RecordCount = recordCount,
                    Records = resultAgregated
                });
            }
        }

        [HttpGet]
        public IHttpActionResult ListTopViewed(string idCulture, int idCustomer = 0)
        {
            int totalRecords = 4;
            List<IEnumerable<ElementExtended>> result = new List<IEnumerable<ElementExtended>>();
            var items = ServiceContext.ElementService.ListTopViewed(idCulture, idCustomer);
            if (items != null && items.Count() > 0)
            {
                int totalPages = (int)(items.Count() / totalRecords);
                if (items.Count() % totalRecords > 0)
                    totalPages++;

                for (int i = 0; i < totalPages; i++)
                    result.Add(items.Skip(i * totalRecords).Take(totalRecords));
            }

            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult ListSimilar(string idCulture, long idElement, int idCustomer = 0)
        {
            return Ok(ServiceContext.ElementService.ListSimilar(idCulture, idElement, idCustomer));
        }

        [HttpGet]
        public IHttpActionResult ListFeatured(string idCulture, int idCustomer = 0)
        {
            return Ok(ServiceContext.ElementService.ListFeatured(idCulture, idCustomer));
        }

        [HttpGet]
        public IHttpActionResult GetElement(string idCulture, long idElement, bool igoreAttributes = false, bool validateCustomer = false)
        {
            var item = ServiceContext.ElementService.GetByIdExtended(idCulture, idElement, igoreAttributes);
            if (item != null)
            {
                if (validateCustomer)
                {
                    var customer = base.GetAuthCustomer();
                    if (customer == null)
                    {
                        return BadRequest(Internationalization.Message.Customer_Not_Found);
                    }
                    else
                    {
                        if (item.IDCustomer != customer.IDCustomer)
                            return BadRequest(Internationalization.Message.Element_Does_Not_Belongs_To_Customer);
                    }
                }
                return Ok(item);
            }
            else
            {
                return BadRequest(Internationalization.Message.Element_Not_Found);
            }
        }

        [HttpGet]
        public IHttpActionResult ListImages(long idElement, int groupIn = 0)
        {
            var images = ServiceContext.ElementService.ListImages(idElement);
            //var images = records.Select(p => p.Value);
            List<ElementAttribute[]> resultAgregated = new List<ElementAttribute[]>();
            if (groupIn > 0)
            {
                int totalPages = (int)(images.Count() / groupIn);
                if (images.Count() % groupIn > 0)
                    totalPages++;

                for (int i = 0; i < totalPages; i++)
                    resultAgregated.Add(images.Skip(i * groupIn).Take(groupIn).ToArray());

                return Ok(new { Images = resultAgregated.ToArray() });
            }
            else
                return Ok(new { Images = images });
        }

        [HttpGet]
        [Authorize]
        public IHttpActionResult AddAsFavorite(long idElement)
        {
            var customer = base.GetAuthCustomer();
            if (customer == null)
                return BadRequest(Internationalization.Message.User_Not_Authenticated);
            else
            {
                var element = ServiceContext.ElementBookmarkedService.Find(p => p.IDCustomer == customer.IDCustomer && p.IDElement == idElement).FirstOrDefault();
                if (element == null)
                {
                    ServiceContext.ElementBookmarkedService.Insert(new ElementBookmarked()
                    {
                        CreateDate = DateTime.Now,
                        IDCustomer = customer.IDCustomer,
                        IDElement = idElement
                    });
                    ServiceContext.ElementBookmarkedService.SaveChanges();
                }
                return Ok(new { Message = Internationalization.Message.Bookmark_Added });
            }
        }

        [HttpGet]
        [Authorize]
        public IHttpActionResult RemoveAsFavorite(long idElement)
        {
            var customer = base.GetAuthCustomer();
            if (customer == null)
                return BadRequest(Internationalization.Message.User_Not_Authenticated);
            else
            {
                ServiceContext.ElementBookmarkedService.Delete(p => p.IDElement == idElement);
                ServiceContext.ElementBookmarkedService.SaveChanges();
                return Ok(new { Message = Internationalization.Message.Bookmark_Removed });
            }
        }

        [HttpGet]
        [Authorize]
        public IHttpActionResult ListBookmarked(string idCulture)
        {
            var customer = base.GetAuthCustomer();
            if (customer == null)
                return BadRequest(Internationalization.Message.User_Not_Authenticated);
            else
                return Ok(ServiceContext.ElementBookmarkedService.List(idCulture, customer.IDCustomer.Value));
        }

        [HttpGet]
        public IHttpActionResult ListBasicAttributes(string idCulture, long? idElement = null)
        {
            int totalRecords = 2;
            var attrType = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeTypeService.GetByAcronym("BASIC");
            List<IEnumerable<AttributeExtended>> result = new List<IEnumerable<AttributeExtended>>();
            var items = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeService.List(idCulture, attrType.IDAttributeType.Value, idElement ?? 0);
            if (items != null && items.Count() > 0)
            {
                int totalPages = (int)(items.Count() / totalRecords);
                if (items.Count() % totalRecords > 0)
                    totalPages++;

                for (int i = 0; i < totalPages; i++)
                    result.Add(items.Skip(i * totalRecords).Take(totalRecords));
            }

            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult ListInfrastructureAttributes(string idCulture, long? idElement = null)
        {
            var attrType = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeTypeService.GetByAcronym("IE");
            if (attrType != null)
                return Ok(SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeService.List(idCulture, attrType.IDAttributeType.Value, idElement ?? 0));
            else
                return BadRequest(Internationalization.Message.Attributes_Not_Found);
        }

        [HttpGet]
        [Authorize]
        public IHttpActionResult InactivateElement(long idElement)
        {
            var element = ServiceContext.ElementService.GetById(idElement);
            if (element != null)
            {
                if (CheckOwnership(element.IDCustomer))
                {
                    element.Status = (short)Model.InfraStructure.Enums.StatusType.Inactive;
                    element.ModifyDate = DateTime.Now;
                    ServiceContext.ElementService.Update(element);
                    return Ok(new { Message = Internationalization.Message.Record_Updated_Successfully });
                }
                else
                {
                    return BadRequest(Internationalization.Message.Element_Does_Not_Belongs_To_Customer);
                }
            }
            else
                return BadRequest(Internationalization.Message.Element_Not_Found);
        }

        [Authorize]
        [ValidateMimeMultipartContentFilter]
        [HttpPost]
        public async Task<IHttpActionResult> UploadImages()
        {
            var customer = base.GetAuthCustomer();
            if (customer == null)
            {
                return BadRequest(Internationalization.Message.Customer_Not_Found);
            }
            else
            {
                bool deleteDir = false;
                long? idElement = null;
                if (HttpContext.Current.Request.Form["idElement"] != null)
                    idElement = Convert.ToInt64(HttpContext.Current.Request.Form["idElement"]);

                if (HttpContext.Current.Request.Form["deleteDir"] != null && HttpContext.Current.Request.Form["deleteDir"] == "true")
                    deleteDir = true;

                if (ServiceContext.ElementService.GetTotalImages(idElement.Value) >= SiteContext.MaximumImages)
                    return BadRequest(string.Format(Internationalization.Message.Number_Of_Images_Exceeded_Allowed, SiteContext.MaximumImages));
                else
                {
                    string imgBasePath = String.Format("{0}/{1}/{2}", SiteContext.UploadPath, customer.IDCustomer, idElement.HasValue ? idElement.ToString() : "TEMP");
                    //if (deleteDir)
                    //    IOHelper.DeleteFilesAndFoldersRecursively(HttpContext.Current.Server.MapPath(imgBasePath));

                    var provider = GetMultipartProvider(imgBasePath);
                    var result = await Request.Content.ReadAsMultipartAsync(provider);
                    foreach (var item in result.FileData)
                    {
                        var originalFileName = GetDeserializedFileName(item);
                        var uploadedFileInfo = new FileInfo(item.LocalFileName);
                        string ext = originalFileName.Substring(originalFileName.LastIndexOf('.'));

                        string imageName = String.Format(@"{0}{1}", Guid.NewGuid(), ext);
                        string savedFileName = HttpContext.Current.Server.MapPath(String.Format(@"{0}/{1}", imgBasePath, imageName));
                        System.IO.File.Move(uploadedFileInfo.FullName, savedFileName);

                        //Criando o thumbnail da imagem recebida
                        string thumbImagePathVirtual = String.Format(@"{0}/Thumb/{1}", imgBasePath, imageName);
                        string thumbImagePath = HttpContext.Current.Server.MapPath(thumbImagePathVirtual);

                        IOHelper.CreateFile(File.OpenRead(savedFileName), thumbImagePath);
                        ImageHelper.ResizeImage(savedFileName, thumbImagePath, 250, 250, true);

                        //salvando com atributo em banco
                        ServiceContext.ElementService.InsertImage(idElement.Value, thumbImagePathVirtual);
                    }
                }
                return Ok(new { Message = Internationalization.Message.All_Files_Were_Uploaded_Sucessfully });
            }
        }

        [Authorize]
        [HttpGet]
        public IHttpActionResult DeleteElementImage(long idElement, long? idElementAttribute = null)
        {
            var element = ServiceContext.ElementService.GetById(idElement);
            if (CheckOwnership(element.IDCustomer))
            {
                var images = ServiceContext.ElementService.DeleteImage(idElement, idElementAttribute);
                foreach (var image in images)
                    IOHelper.DeleteFile(HttpContext.Current.Server.MapPath(image));

                return Ok(new { Message = Internationalization.Message.Image_Deleted_Successfully });
            }
            else
            {
                return BadRequest(Internationalization.Message.Element_Does_Not_Belongs_To_Customer);
            }
        }

        //[Authorize]
        //[HttpGet]
        //public IHttpActionResult DeleteElementImage(long idElement, string imageName = null)
        //{
        //    var element = ServiceContext.ElementService.GetById(idElement);
        //    if (CheckOwnership(element.IDCustomer))
        //    {
        //        string message = "";
        //        var customer = base.GetAuthCustomer();
        //        string imgBasePath = HttpContext.Current.Server.MapPath(String.Format("{0}/{1}/{2}", SiteContext.UploadPath, customer.IDCustomer, idElement));
        //        if (string.IsNullOrEmpty(imageName))
        //        {
        //            IOHelper.DeleteFilesAndFoldersRecursively(imgBasePath);
        //            element.DefaultPicturePath = null;
        //            ServiceContext.ElementService.Update(element);
        //            message = Internationalization.Message.Images_Deleted_Successfully;
        //        }
        //        else
        //        {
        //            string imagePath = HttpContext.Current.Server.MapPath(String.Format("{0}/{1}/{2}/{3}", SiteContext.UploadPath, customer.IDCustomer, idElement, imageName));
        //            string virtualThumbPath = String.Format("{0}/{1}/{2}/thumb/{3}", SiteContext.UploadPath, customer.IDCustomer, idElement, imageName);
        //            string thumbPath = HttpContext.Current.Server.MapPath(virtualThumbPath);
        //            IOHelper.DeleteFile(imagePath);
        //            IOHelper.DeleteFile(thumbPath);
        //            if (element.DefaultPicturePath != null && element.DefaultPicturePath.Trim() == virtualThumbPath.Trim())
        //                element.DefaultPicturePath = null;

        //            message = Internationalization.Message.Image_Deleted_Successfully;
        //        }

        //        ServiceContext.ElementService.Update(element);
        //        return Ok(new
        //        {
        //            Message = message
        //        });
        //    }
        //    else
        //    {
        //        return BadRequest(Internationalization.Message.Element_Does_Not_Belongs_To_Customer);
        //    }
        //}

        [Authorize]
        [HttpGet]
        public IHttpActionResult SetDefaultImage(long idElement, long idElementAttribute)
        {
            var element = ServiceContext.ElementService.GetById(idElement);
            if (CheckOwnership(element.IDCustomer))
            {
                return Ok(new
                {
                    DefaultPicturePath = ServiceContext.ElementService.SetDefaultImage(idElement, idElementAttribute),
                    Message = Internationalization.Message.Default_Image_Defined
                });
            }
            else
            {
                return BadRequest(Internationalization.Message.Element_Does_Not_Belongs_To_Customer);
            }
        }

        #region [Auxiliary Methods]

        public bool CheckOwnership(int idCustomer)
        {
            bool result = true;
            var customer = base.GetAuthCustomer();
            if (customer == null)
                return false;
            else
            {
                if (idCustomer != customer.IDCustomer.Value)
                    return false;
            }

            return result;
        }

        private MultipartFormDataStreamProvider GetMultipartProvider(string imgBasePath)
        {
            var root = HttpContext.Current.Server.MapPath(imgBasePath);
            Directory.CreateDirectory(root);
            return new MultipartFormDataStreamProvider(root);
        }

        public string GetDeserializedFileName(MultipartFileData fileData)
        {
            var fileName = fileData.Headers.ContentDisposition.FileName;
            return JsonConvert.DeserializeObject(fileName).ToString();
        }

        private string[] ListFilesByPath(string path)
        {
            List<string> result = new List<string>();
            string basePath = HttpContext.Current.Server.MapPath(path);
            if (Directory.Exists(basePath))
            {
                foreach (var filePath in System.IO.Directory.GetFiles(basePath, "*.*").OrderBy(p => p))
                    result.Add(new FileInfo(filePath).Name);
            }
            return result.ToArray();
        }

        #endregion 
    }
}
