using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSolutions.ServicoDireto.Services.ElementSystem.Contracts
{
    public interface IElementBookmarkedContract : IBaseRepository<ElementBookmarked> //IBaseService<ElementBookmarked>
    {
        IList<ElementExtended> List(string idCulture, int idCustomer);
    }
}
