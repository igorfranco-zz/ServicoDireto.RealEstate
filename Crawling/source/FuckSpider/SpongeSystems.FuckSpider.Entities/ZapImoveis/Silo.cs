using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.ZapImoveis
{
    public class Silo
    {
        public int NumeroModulo { get; set; }
        public string TituloModulo { get; set; }
        public IList<SiloItem> Silos { get; set; }
        public int NumeroOfertas { get; set; }
        public object Url { get; set; }
    }
}
