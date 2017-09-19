using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Model;

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts
{
    public interface ICityContract : IBaseService<City>
    {
        IList<CityExtended> GetAll(out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1);
        IList<CityExtended> GetAllActive(string idCountry, int idStateProvince);
        void Inactivate(int[] ids);
        IList<CityExtended> GetByStatus(short status);
        CityExtended Get(int id);
        SelectList GetAllActiveAsSelectList(string idCountry, int idStateProvince);
        City GetInsert(string cityName, int idStateProvince);
    }
}