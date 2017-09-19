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
    [AllowAnonymous]
    public class ApiStructureController : ApiController
    {
        [HttpGet]
        public IHttpActionResult ListPurpose(string idCulture)
        {
            return Ok(ServiceContext.PurposeService.GetByStatus((short)Enums.StatusType.Active, idCulture));
        }
        //
        [HttpGet]
        public IEnumerable<HierarchyStructureBasic> ListCategory(string idCulture, int idPurpose)
        {
            return ServiceContext.PurposeService.ListVinculatedCategory(new int[] { idPurpose }, idCulture);
        }
        //
        [HttpGet]
        public IEnumerable<HierarchyStructureBasic> ListType(string idCulture, int idCategory)
        {
            return ServiceContext.PurposeService.ListType(idCategory, idCulture);
        }
    }
}
