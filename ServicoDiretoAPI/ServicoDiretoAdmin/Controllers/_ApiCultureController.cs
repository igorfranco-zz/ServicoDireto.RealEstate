using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    [Authorize(Roles = SiteContext.Roles.ADMINISTRATOR)]
    public class ApiCultureController : ApiController
    {
        [HttpGet]
        public IHttpActionResult ListActive()
        {
            return Ok(ServiceContext.CultureService.GetAllActive());
        }

        [HttpGet]
        public IHttpActionResult List(int page = 1, int maximumRows = 0, int recordCount = 0)
        {
            page--;
            var result = ServiceContext.CultureService.GetAll(out recordCount, startRowIndex: maximumRows * page, maximumRows: maximumRows);
            return Ok(new
            {
                RecordCount = recordCount,
                Records = result
            });
        }

        [HttpGet]
        public IHttpActionResult GetByID(string idCulture)
        {
            return Ok(ServiceContext.CultureService.GetById(idCulture));
        }

        [HttpPost]
        public IHttpActionResult Save([FromBody]Culture record)
        {
            string message = string.Empty;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var country = Services.ServiceContext.CultureService.GetById(record.IDCulture);
                if (country == null)
                {
                    record.CreateDate = DateTime.Now;
                    record.CreatedBy = SiteContext.ActiveUserName;
                    record.ModifyDate = DateTime.Now;
                    record.ModifiedBy = SiteContext.ActiveUserName;
                    Services.ServiceContext.CultureService.Insert(record);
                    message = Internationalization.Message.Record_Inserted_Successfully;
                }
                else
                {
                    record.ModifyDate = DateTime.Now;
                    record.ModifiedBy = SiteContext.ActiveUserName;
                    Services.ServiceContext.CultureService.Update(record);
                    message = Internationalization.Message.Record_Updated_Successfully;
                }

                return Ok(new { Message = message });
            }
        }

        [HttpPost]
        public IHttpActionResult Deactivate([FromBody]string[] id)
        {
            if (id != null && id.Count() > 0)
                ServiceContext.CultureService.Inactivate(id);

            return Ok(new { Message = Internationalization.Message.Record_Updated_Successfully });
        }
    }
}