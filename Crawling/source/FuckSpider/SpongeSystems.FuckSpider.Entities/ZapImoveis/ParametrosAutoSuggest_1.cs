using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.ZapImoveis
{
    public class ParametrosAutoSuggest
    {
        public string Bairro { get; set; }
        public object CodBairro { get; set; }
        public string Zona { get; set; }
        public object CodZona { get; set; }
        public string Cidade { get; set; }
        public object CodCidade { get; set; }
        public string Agrupamento { get; set; }
        public object CodAgrupamento { get; set; }
        public string Estado { get; set; }
        public object CodEstado { get; set; }
        public object CodCliente { get; set; }
        public object NomeCliente { get; set; }
    }
}
