using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.ZapImoveis
{
    public class Oferta
    {
        public Contab Contab { get; set; }
        public Evento Evento { get; set; }
        public string NomeCliente { get; set; }
        public FaixaDePreco FaixaDePreco { get; set; }
        public string CaminhoImagem { get; set; }
        public string UrlBusca { get; set; }
        public string UrlLogoCliente { get; set; }
        public int CodCidade { get; set; }
        public string Cidade { get; set; }
        public int CodBairroJornal { get; set; }
        public string Bairro { get; set; }
        public object CodZona { get; set; }
        public string Zona { get; set; }
        public int CodLocalidade { get; set; }
        public string Localidade { get; set; }
        public int CodEstado { get; set; }
        public string Estado { get; set; }
        public string ZonaGrupo { get; set; }
        public string Sigla { get; set; }
        public int VitrineB2CGrupoOfertaID { get; set; }
        public int CodTransacao { get; set; }
        public int Origem { get; set; }
    }

}
