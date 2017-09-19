using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts
{
    public interface IIconContract : IBaseService<Icon>
    {
        IList<Icon> GetAll(out int recordCount, string sortType = null, int startRowIndex = -1, int maximumRows = -1);
        IList<Icon> GetByStatus(short status);
        void Inactivate(int[] ids);
        IEnumerable<dynamic> AutoComplete(string text);
    }
}
