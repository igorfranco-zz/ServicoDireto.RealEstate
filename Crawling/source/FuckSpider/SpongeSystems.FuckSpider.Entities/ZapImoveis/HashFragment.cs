using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.ZapImoveis
{
    public class HashFragment
    {
        public string precominimo { get; set; }
        public string precomaximo { get; set; }
        public string filtrodormitorios { get; set; }
        public string filtrosuites { get; set; }
        public string filtrovagas { get; set; }
        public string areautilmaxima { get; set; }
        public IList<Parametrosautosuggest> parametrosautosuggest { get; set; }
        public string pagina { get; set; }
        public string ordem { get; set; }
        public string paginaOrigem { get; set; }
        public string semente { get; set; }
        public string formato { get; set; }
    }
}
