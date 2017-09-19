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
    public class ApiHierarchyStructureController : ApiController
    {
        [HttpGet]
        public IHttpActionResult ListCategory(string idCulture, int page = 1, int maximumRows = 0)
        {
            page--;
            int recordCount = 0;
            var result = (ServiceContext.HierarchyStructureService.ListCategory(out recordCount, idCulture, startRowIndex: maximumRows * page, maximumRows: maximumRows));
            return Ok(new
            {
                Records = result,
                RecordCount = recordCount
            });
        }

        [HttpGet]
        public IHttpActionResult ListType(string idCulture, int page = 1, int maximumRows = 0)
        {
            page--;
            int recordCount = 0;
            var result = (ServiceContext.HierarchyStructureService.ListType(out recordCount, idCulture, startRowIndex: maximumRows * page, maximumRows: maximumRows));
            return Ok(new
            {
                Records = result,
                RecordCount = recordCount
            });
        }

        [HttpGet]
        public IHttpActionResult List(string idCulture, int page = 1, int maximumRows = 0)
        {
            page--;
            int recordCount = 0;
            var result = ServiceContext.HierarchyStructureService.GetAll(out recordCount, null, startRowIndex: maximumRows * page, maximumRows: maximumRows, idCulture: idCulture);
            return Ok(new
            {
                Records = result,
                RecordCount = recordCount
            });
        }

        [HttpGet]
        public IHttpActionResult GetByID(int idHierarchyStructure = 0)
        {
            var item = ServiceContext.HierarchyStructureService.GetById(idHierarchyStructure);
            if (item == null)
                item = new HierarchyStructure() { Status = 1 };

            item.Culture = ServiceContext.HierarchyStructureService.ListCulture(idHierarchyStructure);
            return Ok(item);
        }

        [HttpPost]
        public IHttpActionResult Save([FromBody]HierarchyStructure record)
        {
            string message = string.Empty;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                ICollection<HierarchyStructureCulture> hierarchyStructureCulture = null;
                if (record.Culture != null)
                {
                    hierarchyStructureCulture = (from c in record.Culture
                                                 select new HierarchyStructureCulture
                                                 {
                                                     IDHierarchyStructure = ((record.IDHierarchyStructure.HasValue) ? record.IDHierarchyStructure.Value : 0),
                                                     Description = c.Description,
                                                     IDCulture = c.IDCulture
                                                 }).ToList();
                }

                if (record.IDHierarchyStructure.HasValue)
                {
                    record.ModifyDate = DateTime.Now;
                    record.ModifiedBy = SiteContext.ActiveUserName;
                    Services.ServiceContext.HierarchyStructureService.Update(record, hierarchyStructureCulture, null);
                    message = Internationalization.Message.Record_Updated_Successfully;
                }
                else
                {
                    record.CreateDate = DateTime.Now;
                    record.CreatedBy = SiteContext.ActiveUserName;
                    record.ModifyDate = DateTime.Now;
                    record.ModifiedBy = SiteContext.ActiveUserName;
                    Services.ServiceContext.HierarchyStructureService.Insert(record, hierarchyStructureCulture, null);
                    message = Internationalization.Message.Record_Inserted_Successfully;
                }

                return Ok(new { Message = message });
            }
       }

        [HttpPost]
        public IHttpActionResult Deactivate([FromBody]int[] id)
        {
            if (id != null && id.Count() > 0)
                ServiceContext.HierarchyStructureService.Inactivate(id);

            return Ok(new { Message = Internationalization.Message.Record_Updated_Successfully });
        }
    }
}