using SpongeSolutions.Core.Helpers;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Model.AccountSystem;
using SpongeSolutions.ServicoDireto.Services;
using SpongeSolutions.ServicoDireto.Services.AccountSystem.Contracts;
using SpongeSolutions.ServicoDireto.Services.AccountSystem.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using System.Xml.Linq;
using System.Web.Http.Controllers;
using System.Web.Security;
using System.Web;
using SpongeSolutions.Core.RestFul;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.IO;
using Newtonsoft.Json;
using System.Web.Http.Filters;
using SpongeSolutions.ServicoDireto.Admin.Providers;
using System.Net.Configuration;
using System.Configuration;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    public class ApiCustomerController : BaseApiController
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetCustomerAsync(int? idCustomer = null)
        {
            if (idCustomer.HasValue)
                return Ok(await ServiceContext.CustomerService.GetByIdAsync(idCustomer.Value));
            else
            {
                var customer = base.GetAuthCustomer();
                if (customer == null)
                    return BadRequest(Internationalization.Message.User_Not_Authenticated);
                else
                {
                    var item = await ServiceContext.CustomerService.GetByIdAsync(customer.IDCustomer.Value);
                    if (item == null)
                        return BadRequest(Internationalization.Message.Customer_Not_Found);
                    else
                        return Ok(item);
                }
            }
        }

        [HttpGet]
        public IHttpActionResult GetFullCustomer()
        {
            var customer = GetAuthCustomer();
            if (GetAuthCustomer() == null)
                return BadRequest(Internationalization.Message.Customer_Not_Found);
            else
                return Ok(customer);
        }

        [Authorize]
        [HttpPost]
        public IHttpActionResult SaveCustomer(Customer customer)
        {
            string message = string.Empty;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                //Verifica se o já existe o mesmo nome de site
                if (!string.IsNullOrEmpty(customer.SiteName))
                {
                    var customerBase = Services.ServiceContext.CustomerService.GetBySiteName(customer.SiteName);
                    if (customerBase != null && customer.IDCustomer.HasValue && customerBase.IDCustomer != customer.IDCustomer.Value)
                        return BadRequest(SpongeSolutions.ServicoDireto.Internationalization.Message.SiteName_Already_Exists);
                }
                else
                {
                    if (!customer.IDCustomer.HasValue)
                    {
                        customer.UserName = SiteContext.ActiveUserName;
                        customer.CreateDate = DateTime.Now;
                        customer.CreatedBy = SiteContext.ActiveUserName;
                        customer.ModifyDate = DateTime.Now;
                        customer.ModifiedBy = SiteContext.ActiveUserName;
                        ServiceContext.CustomerService.Insert(customer);
                        message = Internationalization.Message.Record_Inserted_Successfully;
                    }
                    else
                    {
                        customer.ModifyDate = DateTime.Now;
                        customer.ModifiedBy = SiteContext.ActiveUserName;
                        ServiceContext.CustomerService.Update(customer);
                        message = Internationalization.Message.Record_Updated_Successfully;
                    }
                }
                return Ok(message);
            }
        }

        [Authorize]
        [ValidateMimeMultipartContentFilter]
        [HttpPost]
        public async Task<IHttpActionResult> UploadAvatar()
        {
            var customer = base.GetAuthCustomer();
            if (customer == null)
            {
                return BadRequest(Internationalization.Message.Customer_Not_Found);
            }
            else
            {
                string imgBasePath = String.Format("{0}/{1}", SiteContext.UploadPath, customer.IDCustomer);
                var provider = GetMultipartProvider(imgBasePath);
                var result = await Request.Content.ReadAsMultipartAsync(provider);
                var originalFileName = GetDeserializedFileName(result.FileData.First());
                var uploadedFileInfo = new FileInfo(result.FileData.First().LocalFileName);
                string ext = originalFileName.Substring(originalFileName.LastIndexOf('.'));

                string imageName = String.Format(@"{0}{1}", Guid.NewGuid(), ext);
                string savedFileName = HttpContext.Current.Server.MapPath(String.Format(@"{0}/{1}", imgBasePath, imageName));
                System.IO.File.Move(uploadedFileInfo.FullName, savedFileName);

                //Criando o thumbnail da imagem recebida
                string thumbImagePathVirtual = String.Format(@"{0}/Thumb/{1}", imgBasePath, imageName);
                string thumbImagePath = HttpContext.Current.Server.MapPath(thumbImagePathVirtual);
                IOHelper.CreateFile(File.OpenRead(savedFileName), thumbImagePath);
                ImageHelper.ResizeImage(savedFileName, thumbImagePath, 250, 250, true);

                //atualizando o nome do logo
                customer.Logo = thumbImagePathVirtual;
                ServiceContext.CustomerService.Update(customer);
                return Ok(new { Message = Internationalization.Message.Profile_Image_Changed });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult SendEmailRequestInfo(dynamic contact)
        {
            if (contact.idElement != null)
            {
                var element = ServiceContext.ElementService.GetByIdExtended((string)contact.idCulture, (long)contact.idElement, true);
                var customer = ServiceContext.CustomerService.GetById((int)contact.idCustomer);
                this.SendEmailInfoElement(contact, customer, element, (string)contact.idCulture);
            }
            else
            {
                var customer = ServiceContext.CustomerService.GetById((int)contact.idCustomer);
                this.SendEmailAgentContact(contact, customer, (string)contact.idCulture);
            }
            return Ok(new { Message = SpongeSolutions.ServicoDireto.Internationalization.Message.Email_Sent_Successfully });
        }

        #region [Auxiliar Methods]

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

        public void SendEmailAgentContact(dynamic contact, Customer customer, string idCulture)
        {
            XDocument xml = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
               new XElement("Root",
               new XElement("Data",
               new XElement("SiteName", SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SiteName),
               new XElement("Name", (string)contact.name),
               new XElement("Email", (string)contact.email),
               new XElement("Site", SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SitePath),
               new XElement("Message", (string)contact.message))));

            EmailHelper.Send(customer.Email,
                SpongeSolutions.ServicoDireto.Internationalization.Label.Request_Element_Info,
                HttpContext.Current.Server.MapPath(String.Format("/templates/{0}/EmailAgentContact.xsl",
                idCulture)), xml);
        }

        public void SendEmailInfoElement(dynamic contact, Customer customer, ElementExtended element, string idCulture)
        {           
            XDocument xml = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
               new XElement("Root",
               new XElement("Data",
                   new XElement("SiteName", SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SiteName),
                   new XElement("IDElement", element.IDElement),
                   new XElement("Title", element.Name),
                   new XElement("Description", element.Description),
                   new XElement("Name", (string)contact.name),
                   new XElement("Email", (string)contact.email),
                   new XElement("Site", SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SitePath),
                   new XElement("Message", (string)contact.message))));

            EmailHelper.Send(customer.Email,
                SpongeSolutions.ServicoDireto.Internationalization.Label.Request_Element_Info,
                HttpContext.Current.Server.MapPath(String.Format("/templates/{0}/CustomerInfoElement.xsl",
                idCulture)), xml);
        }

        [AllowAnonymous]
        public void SendEmailContact(dynamic contact)
        {
            XDocument xml = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
               new XElement("Root",
               new XElement("Data",
               new XElement("SiteName", SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SiteName),
               new XElement("Name", (string)contact.name),
               new XElement("Email", (string)contact.email),
               new XElement("Site", SpongeSolutions.ServicoDireto.Admin.Properties.Settings.Default.SitePath),
               new XElement("Message", (string)contact.message))));

            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            EmailHelper.Send(smtpSection.From,
                SpongeSolutions.ServicoDireto.Internationalization.Label.Contact,
                HttpContext.Current.Server.MapPath(String.Format("/templates/{0}/EmailContact.xsl",
                (string)contact.idCulture)), xml);
        }

        #endregion
    }
}
