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
    public class ApiCountryController : ApiController
    {
        [HttpGet]
        public IHttpActionResult List(int page = 1, int maximumRows = 0, int recordCount = 0)
        {
            page--;
            var result = ServiceContext.CountryService.GetAll(out recordCount, startRowIndex: maximumRows * page, maximumRows: maximumRows);
            return Ok(new
            {
                RecordCount = recordCount,
                Records = result
            });
        }

        [HttpGet]
        public IHttpActionResult ListActive()
        {
            return Ok(ServiceContext.CountryService.GetAllActive());
        }

        [HttpGet]
        public IHttpActionResult GetByID(string idCountry)
        {
            return Ok(ServiceContext.CountryService.GetById(idCountry));
        }

        [HttpPost]
        public IHttpActionResult Save([FromBody]Country record)
        {
            string message = string.Empty;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var country = Services.ServiceContext.CountryService.GetById(record.IDCountry);
                if (country == null)
                {
                    record.CreateDate = DateTime.Now;
                    record.CreatedBy = SiteContext.ActiveUserName;
                    record.ModifyDate = DateTime.Now;
                    record.ModifiedBy = SiteContext.ActiveUserName;
                    Services.ServiceContext.CountryService.Insert(record);
                    message = Internationalization.Message.Record_Inserted_Successfully;
                }
                else
                {
                    record.ModifyDate = DateTime.Now;
                    record.ModifiedBy = SiteContext.ActiveUserName;
                    Services.ServiceContext.CountryService.Update(record);
                    message = Internationalization.Message.Record_Updated_Successfully;
                }

                return Ok(new { Message = message });
            }
        }

        [HttpPost]
        public IHttpActionResult Deactivate([FromBody]string[] id)
        {
            if (id != null && id.Count() > 0)
                ServiceContext.CountryService.Inactivate(id);

            return Ok(new { Message = Internationalization.Message.Record_Updated_Successfully });
        }
    }
}