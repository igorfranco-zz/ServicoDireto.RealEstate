using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.ZapImoveis
{
    public class Resultado
    {
        public int QuantidadeRegistros { get; set; }
        public string QuantidadeRegistrosFormatado { get; set; }
        public int QuantidadePaginas { get; set; }
        public string QuantidadePaginasFormatado { get; set; }
        public int PaginaAtual { get; set; }
        public string PaginaAtualFormatado { get; set; }
        public int Formato { get; set; }
        public int Ordenacao { get; set; }
        public IList<int> TiposOrdenacao { get; set; }
        public int Transacao { get; set; }
        public object Navegadores { get; set; }
        public object GrupoNavegadores { get; set; }
        [JsonProperty(PropertyName = "Resultado")]
        public IList<ResultadoItem> Resultado_ { get; set; }
        public string SubTipoImovel { get; set; }
        public object TextoBuscaAberta { get; set; }
        public int PaginaOrigem { get; set; }
        public string CriteriosAutoSuggest { get; set; }
        public IList<ParametrosAutoSuggest> ParametrosAutoSuggest { get; set; }
        public object PrecoMinimo { get; set; }
        public long PrecoMaximo { get; set; }
        public string NomeImobiliariaBusca { get; set; }
        public object Latitude { get; set; }
        public object Longitude { get; set; }
        public string Localizacao { get; set; }
        public int Semente { get; set; }
        public string FiltroDormitorios { get; set; }
        public object FiltroSuites { get; set; }
        public object FiltroVagas { get; set; }
        public object AreaUtilMinima { get; set; }
        public object AreaUtilMaxima { get; set; }
        public object AreaTotalMinima { get; set; }
        public object AreaTotalMaxima { get; set; }
        public object PossuiFotos { get; set; }
        public object PossuiEndereco { get; set; }
        public object CodigoCliente { get; set; }
        public object NomeCliente { get; set; }
        public object CodigoOferta { get; set; }
        public object Contate { get; set; }
        public int SubTipoOferta { get; set; }
        public string EnderecoTrabalho { get; set; }
        public string FotosOfertas { get; set; }
        public object ListaSubtiposVenda { get; set; }
        public object ListaSubtiposLocacao { get; set; }
        public object DebugNota { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public SitePersonalizado SitePersonalizado { get; set; }
        public object TextoQuantidadeResultados { get; set; }
        public string TituloPagina { get; set; }
        public string DescricaoPagina { get; set; }
        public bool IndResultadoZero { get; set; }
        public bool IndExecutaResultadoZero { get; set; }
        public bool IndResultadoCampanha { get; set; }
        public string TituloBusca { get; set; }
        public bool IndSitePersonalizado { get; set; }
        public IList<object> Tags { get; set; }
        public int IndicePosicaoBanner { get; set; }
        public object SugestoesResultadoZero { get; set; }
        public object ObterSugestaoZero { get; set; }
        public bool AtivarNovoResultadoZero { get; set; }
        public object JsonLD { get; set; }
    }
}
