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
    public class ApiAttributeController : ApiController
    {
        [HttpGet]
        public IHttpActionResult List(string idCulture, int page = 1, int maximumRows = 0)
        {
            page--;
            int recordCount = 0;
            var result = ServiceContext.AttributeService.GetAll(out recordCount, idCulture, startRowIndex: maximumRows * page, maximumRows: maximumRows);
            return Ok(new
            {
                Records = result,
                RecordCount = recordCount
            });
        }

        [HttpGet]
        public IHttpActionResult GetByID(int idAttribute = 0)
        {
            var item = ServiceContext.AttributeService.GetById(idAttribute);
            if (item == null)
                item = new Model.Attribute() { Status = 1 };

            item.Culture = ServiceContext.AttributeService.ListAttributeCulture(idAttribute);
            return Ok(item);
        }

        [HttpPost]
        public IHttpActionResult Save([FromBody]Model.Attribute record)
        {
            string message = string.Empty;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                ICollection<AttributeCulture> attributeCulture = null;
                if (record.Culture != null)
                {
                    attributeCulture = (from c in record.Culture
                                        select new AttributeCulture
                                        {
                                            IDAttribute = (record.IDAttribute.HasValue) ? record.IDAttribute.Value : 0,
                                            Name = c.Name,
                                            Value = c.Value,
                                            IDCulture = c.IDCulture
                                        }).ToList();
                }

                if (record.IDAttribute.HasValue)
                {
                    record.ModifyDate = DateTime.Now;
                    record.ModifiedBy = SiteContext.ActiveUserName;
                    Services.ServiceContext.AttributeService.Update(record, attributeCulture);
                    message = Internationalization.Message.Record_Updated_Successfully;
                }
                else
                {
                    record.CreateDate = DateTime.Now;
                    record.CreatedBy = SiteContext.ActiveUserName;
                    record.ModifyDate = DateTime.Now;
                    record.ModifiedBy = SiteContext.ActiveUserName;
                    Services.ServiceContext.AttributeService.Insert(record, attributeCulture);
                    message = Internationalization.Message.Record_Inserted_Successfully;
                }

                return Ok(new { Message = message });
            }
        }

        [HttpPost]
        public IHttpActionResult Deactivate([FromBody]int[] id)
        {
            if (id != null && id.Count() > 0)
                ServiceContext.AttributeService.Inactivate(id);

            return Ok(new { Message = Internationalization.Message.Record_Updated_Successfully });
        }
    }
}