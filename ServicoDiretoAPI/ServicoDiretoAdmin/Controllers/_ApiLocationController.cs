using SpongeSolutions.ServicoDireto.Model;
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
    public class ApiLocationController : ApiController
    {
        [HttpGet]
        public IEnumerable<Country> ListCountry()
        {
            return ServiceContext.CountryService.GetAll();
        }

        [HttpGet]
        public IEnumerable<StateProvince> ListStateProvince(string idCountry)
        {
            return ServiceContext.StateProvinceService.GetAllActive(idCountry);
        }

        [HttpGet]
        public IEnumerable<City> ListCity(string idCountry, int idStateProvince)
        {
            return ServiceContext.CityService.GetAllActive(idCountry, idStateProvince);
        }
    }
}