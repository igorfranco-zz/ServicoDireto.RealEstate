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
    public class ApiAttributeTypeController : ApiController
    {
        [HttpGet]
        public IHttpActionResult List(string idCulture)
        {
            return Ok(ServiceContext.AttributeTypeService.GetAll(idCulture));
        }

        [HttpGet]
        public IHttpActionResult GetByID(string idCulture, int idAttributeType = 0)
        {
            var item = ServiceContext.AttributeTypeService.GetById(idAttributeType);
            if (item == null)
                item = new AttributeType() { Status = 1 };

            item.Culture = ServiceContext.AttributeTypeService.ListAttributeTypeCulture(idAttributeType);
            if (idAttributeType != 0)
            {
                item.VinculatedAttributes = (from a in ServiceContext.AttributeService.ListVinculatedAttribute(idCulture, idAttributeType)
                                             select new Option
                                             {
                                                 ID = a.IDAttribute.ToString(),
                                                 Text = a.Name
                                             }).ToList();
            }
            //
            item.AvailableAttributes = (from a in ServiceContext.AttributeService.ListAvailableAttribute(idCulture, -1)
                                        select new Option
                                        {
                                            ID = a.IDAttribute.ToString(),
                                            Text = a.Name,
                                            //Checked = item.VinculatedAttributes.Count(p=>p.ID == a.IDAttribute.ToString()) > 0
                                        }).ToList();
            return Ok(item);
        }

        [HttpPost]
        public IHttpActionResult Save([FromBody]AttributeType record)
        {
            string message = string.Empty;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                ICollection<AttributeTypeCulture> attributeTypeCulture = null;
                int[] vinculatedAttribute = null;
                if (record.Culture != null)
                {
                    attributeTypeCulture = (from c in record.Culture
                                            select new AttributeTypeCulture
                                            {
                                                IDAttributeType = (record.IDAttributeType.HasValue) ? record.IDAttributeType.Value : 0,
                                                Description = c.Description,
                                                IDCulture = c.IDCulture
                                            }).ToList();
                }
                //
                if (record.VinculatedAttributes != null && record.VinculatedAttributes.Count > 0)
                    vinculatedAttribute = record.VinculatedAttributes.Select(p => int.Parse(p.ID)).ToArray();

                if (record.IDAttributeType.HasValue)
                {
                    record.ModifyDate = DateTime.Now;
                    record.ModifiedBy = SiteContext.ActiveUserName;
                    Services.ServiceContext.AttributeTypeService.Update(record, attributeTypeCulture, vinculatedAttribute);
                    message = Internationalization.Message.Record_Updated_Successfully;
                }
                else
                {
                    record.CreateDate = DateTime.Now;
                    record.CreatedBy = SiteContext.ActiveUserName;
                    record.ModifyDate = DateTime.Now;
                    record.ModifiedBy = SiteContext.ActiveUserName;
                    Services.ServiceContext.AttributeTypeService.Insert(record, attributeTypeCulture, vinculatedAttribute);
                    message = Internationalization.Message.Record_Inserted_Successfully;
                }

                return Ok(new { Message = message });
            }
        }

        [HttpPost]
        public IHttpActionResult Deactivate([FromBody]int[] id)
        {
            if (id != null && id.Count() > 0)
                ServiceContext.AttributeTypeService.Inactivate(id);

            return Ok(new { Message = Internationalization.Message.Record_Updated_Successfully });
        }
    }
}