using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Model;
namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts
{
    public interface ICultureContract : IBaseService<Culture, string>
    {
        bool Exists(string id);
        void Inactivate(string[] ids);
        CultureExtended GetById(string id);
        IList<CultureExtended> GetAll(out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1);
        IList<CultureExtended> GetByStatus(short status, out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1);
        IList<CultureExtended> GetAllActive(out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1);
        IList<CultureExtended> GetAllActive();
        IList<CultureExtended> GetAll();
    }
}
