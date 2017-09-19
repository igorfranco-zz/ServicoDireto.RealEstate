using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Model;

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts
{
    public interface ICountryContract : IBaseService<Country, string>
    {
        void Inactivate(string[] ids);
        bool Exists(string id);
        IList<Country> GetByStatus(short status);
        IList<Country> GetAll(out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1);
        SelectList GetAllActive(string selectedCountry = null);
    }
}
