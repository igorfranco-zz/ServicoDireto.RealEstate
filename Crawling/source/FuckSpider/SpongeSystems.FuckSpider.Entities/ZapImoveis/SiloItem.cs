using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.ZapImoveis
{
    public class SiloItem
    {
        public object DescricaoLink { get; set; }
        public string URL { get; set; }
        public int Transacao { get; set; }
        public object Tipo { get; set; }
        public object Estado { get; set; }
        public string Cidade { get; set; }
        public object Bairro { get; set; }
        public object Zona { get; set; }
        public object Dormitorios { get; set; }
        public int QuantidadeOfertas { get; set; }
        public string Url { get; set; }
        public string TextoAmigavel { get; set; }
        public int NumeroDoModulo { get; set; }
    }
}
