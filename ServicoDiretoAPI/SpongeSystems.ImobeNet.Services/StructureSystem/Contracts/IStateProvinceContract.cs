using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;


using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Model;

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts
{
    public interface IStateProvinceContract : IBaseService<StateProvince>
    {
        IList<StateProvince> GetAllActive();
        IList<StateProvince> GetAllActive(string idCountry);
        void Inactivate(int[] ids);
        IList<StateProvince> GetByStatus(short status);
        SelectList GetAllActiveAsSelectList(string idCountry);
        StateProvince GetInsert(string stateName, string idCountry);
        IList<StateProvinceExtended> GetAll(out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1);
    }
}
