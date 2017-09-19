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
    public class ApiIconController : ApiController
    {
        [HttpGet]
        public IHttpActionResult List(int page = 1, int maximumRows = 0, int recordCount = 0)
        {
            page--;
            var result = ServiceContext.IconService.GetAll(out recordCount, startRowIndex: maximumRows * page, maximumRows: maximumRows);
            return Ok(new
            {
                RecordCount = recordCount,
                Records = result
            });
        }

        [HttpGet]
        public IHttpActionResult ListActive()
        {
            return Ok(ServiceContext.IconService.GetByStatus((short)Enums.StatusType.Active));
        }

        [HttpGet]
        public IHttpActionResult GetByID(int idIcon)
        {
            return Ok(ServiceContext.IconService.GetById(idIcon));
        }

        [HttpPost]
        public IHttpActionResult Save([FromBody]Icon record)
        {
            string message = string.Empty;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                if (record.IDIcon.HasValue)
                {
                    record.ModifyDate = DateTime.Now;
                    record.ModifiedBy = SiteContext.ActiveUserName;
                    Services.ServiceContext.IconService.Update(record);
                    message = Internationalization.Message.Record_Updated_Successfully;
                }
                else
                {
                    record.CreateDate = DateTime.Now;
                    record.CreatedBy = SiteContext.ActiveUserName;
                    record.ModifyDate = DateTime.Now;
                    record.ModifiedBy = SiteContext.ActiveUserName;
                    Services.ServiceContext.IconService.Insert(record);
                    message = Internationalization.Message.Record_Inserted_Successfully;
                }

                return Ok(new { Message = message });
            }
        }

        [HttpPost]
        public IHttpActionResult Deactivate([FromBody]int[] id)
        {
            if (id != null && id.Count() > 0)
                ServiceContext.IconService.Inactivate(id);

            return Ok(new { Message = Internationalization.Message.Record_Updated_Successfully });
        }
    }
}