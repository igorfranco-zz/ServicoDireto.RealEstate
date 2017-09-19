using SpongeSystems.Spider.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpongeSystems.Spider.Entities.ZapImoveis;

namespace SpongeSystems.Spider.Services.Implementation
{
    public class ZapImoveisService : BaseService<ResultContainer>, IZapImoveisContract
    {
        public ZapImoveisService()
            : base("ZapImoveis")
        {

        }
    }
}
