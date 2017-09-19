using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using SpongeSolutions.ServicoDireto.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    [Authorize(Roles = SiteContext.Roles.ADMINISTRATOR)]
    public class ApiPurposeController : ApiController
    {
        [HttpGet]
        public IHttpActionResult ListActive(string idCulture)
        {
            return Ok(ServiceContext.PurposeService.GetByStatus((short)Enums.StatusType.Active, idCulture));
        }

        [HttpGet]
        public IHttpActionResult List(string idCulture)
        {
            return Ok(ServiceContext.PurposeService.ListAll(idCulture));
        }

        [HttpGet]
        public IHttpActionResult GetByID(int idPurpose = 0, string idCulture = "pt-BR")
        {
            //System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            var item = ServiceContext.PurposeService.GetById(idPurpose);
            if (item == null)
                item = new Purpose() { Status = 1 };

            item.Culture = ServiceContext.PurposeService.ListPurposeCulture(idPurpose);
            item.Category = ServiceContext.PurposeService.ListCategory(idCulture, new int[] { idPurpose });
            return Ok(item);
        }

        [HttpPost]
        public IHttpActionResult Save([FromBody]Purpose record)
        {
            string message = string.Empty;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                int[] categories = null;
                ICollection<PurposeCulture> purposeCulture = null;
                if (record.Category != null)
                {
                    categories = (from c in record.Category
                                  where c.Checked
                                  select c.IDHierarchyStructure.Value).ToArray();
                }

                if (record.Culture != null)
                {
                    purposeCulture = (from c in record.Culture
                                      select new PurposeCulture
                                      {
                                          IDPurpose = record.IDPurpose,
                                          Description = c.Description,
                                          IDCulture = c.IDCulture
                                      }).ToList();
                }

                if (record.IDPurpose.HasValue)
                {
                    record.ModifyDate = DateTime.Now;
                    record.ModifiedBy = SiteContext.ActiveUserName;
                    Services.ServiceContext.PurposeService.Update(record, purposeCulture, categories);
                    message = Internationalization.Message.Record_Updated_Successfully;
                }
                else
                {
                    record.CreateDate = DateTime.Now;
                    record.CreatedBy = SiteContext.ActiveUserName;
                    record.ModifyDate = DateTime.Now;
                    record.ModifiedBy = SiteContext.ActiveUserName;
                    Services.ServiceContext.PurposeService.Insert(record, purposeCulture, categories);
                    message = Internationalization.Message.Record_Inserted_Successfully;
                }

                return Ok(new { Message = message });
            }
        }

        [HttpPost]
        public IHttpActionResult Deactivate([FromBody]int[] id)
        {
            if (id != null && id.Count() > 0)
                ServiceContext.PurposeService.Inactivate(id);

            return Ok(new { Message = Internationalization.Message.Record_Updated_Successfully });
        }
    }
}