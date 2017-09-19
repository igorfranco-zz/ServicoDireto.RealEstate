using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.ZapImoveis
{
    public class FaixasDePreco
    {
        public int CodFaixaPrecoImovel { get; set; }
        public int FaixaPrecoIni { get; set; }
        public int FaixaPrecoFinal { get; set; }
        public string Descricao { get; set; }
    }
}
