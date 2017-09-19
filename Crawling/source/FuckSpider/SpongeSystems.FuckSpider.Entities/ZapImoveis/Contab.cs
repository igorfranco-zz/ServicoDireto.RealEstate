using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.ZapImoveis
{
    public class Contab
    {
        public IList<FaixasDePreco> FaixasDePreco { get; set; }
        public int VitrineB2CGrupoOfertaID { get; set; }
        public int CodEstado { get; set; }
        public int CodLocalidade { get; set; }
        public int CodCidade { get; set; }
        public object CodZona { get; set; }
        public int CodBairroJornal { get; set; }
        public int CodTransacao { get; set; }
        public string SitePagina { get; set; }
        public int Origem { get; set; }
    }

}
