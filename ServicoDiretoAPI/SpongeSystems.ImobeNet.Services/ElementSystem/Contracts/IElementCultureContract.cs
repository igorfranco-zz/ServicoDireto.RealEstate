using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;

namespace SpongeSolutions.ServicoDireto.Services.ElementSystem.Contracts
{
    public interface IElementCultureContract : IBaseService<ElementCulture>
    {
        void DeleteByElement(int idElement);
        ElementCultureExtended GetById(int idElement, string idCulture);
        IList<ElementCultureExtended> GetAll(out int recordCount, int idElement = -1, string idCulture = null, string sortType = null, int startRowIndex = -1, int maximumRows = -1);
    }
}
