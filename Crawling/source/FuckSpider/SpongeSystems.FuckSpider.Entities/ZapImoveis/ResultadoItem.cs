using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.ZapImoveis
{
    public class ResultadoItem
    {
        public int CodigoAnunciante { get; set; }
        public int Tipo { get; set; }
        public int SubTipo { get; set; }
        public int EstagioObra { get; set; }
        public string UrlFicha { get; set; }
        public string IdFavorito { get; set; }
        public int CodigoUsuario { get; set; }
        public string MensagemContatePadrao { get; set; }
        public bool VerificarExibicaoUrlLogoCliente { get; set; }
        public string DataAtualizacaoHumanizada { get; set; }
        public int CodigoOfertaZAP { get; set; }
        public int CodCampanhaImovel { get; set; }
        public string CodigoOfertaImobiliaria { get; set; }
        public string Valor { get; set; }
        public string Bairro { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public int QuantidadeQuartos { get; set; }
        public int QuantidadeQuartosMinima { get; set; }
        public int QuantidadeQuartosMaxima { get; set; }
        public int QuantidadeVagas { get; set; }
        public int QuantidadeVagasMinima { get; set; }
        public int QuantidadeVagasMaxima { get; set; }
        public int QuantidadeSuites { get; set; }
        public int QuantidadeSuitesMinima { get; set; }
        public int QuantidadeSuitesMaxima { get; set; }
        public int Area { get; set; }
        public int AreaMinima { get; set; }
        public int AreaMaxima { get; set; }
        public string DistanciaMetro { get; set; }
        public string DistanciaOnibus { get; set; }
        public string DistanciaTrabalho { get; set; }
        public IList<Foto> Fotos { get; set; }
        public string FotosOfertas { get; set; }
        public IList<object> Telefones { get; set; }
        public bool OcultadoPeloVisitante { get; set; }
        public bool PossuiQualidadeTotal { get; set; }
        public string UrlLogotipoCliente { get; set; }
        public string NomeAnunciante { get; set; }
        public string SubTipoOferta { get; set; }
        public string Transacao { get; set; }
        public int? CodImobiliaria { get; set; }
        public bool possuiTelefone { get; set; }
        public ContatoCampanha ContatoCampanha { get; set; }
        public int? IndiceContatoCampanha { get; set; }
        public bool possuiEmail { get; set; }
        public bool possuiChat { get; set; }
        public string URLAtendimento { get; set; }
        public bool IndBloqueadoBlackListOPEC { get; set; }
        public string TipoDaOferta { get; set; }
        public double? Nota { get; set; }
        public string LogNota { get; set; }
        public bool ExibirNotaAnuncio { get; set; }
        public string ZapID { get; set; }
        public string Empreendimento { get; set; }
        public double? PrecoVendaMinimo { get; set; }
        public int? PrecoVendaMaximo { get; set; }
        public bool UtilizarChatParceiro { get; set; }
        public object Categoria { get; set; }
        public bool IndSitePersonalizado { get; set; }
        public bool IndUsuarioOPEC { get; set; }
        public string IdCarrossel { get; set; }
        public string ClassBotaoFavorito { get; set; }
        public string FuncaoOnclickBotaoFavorito { get; set; }
        public string QuantidadeQuartosFormatada { get; set; }
        public string QuantidadeVagasFormatada { get; set; }
        public string QuantidadeSuitesFormatada { get; set; }
        public string AreaFormatada { get; set; }
        public string FormatarSubTipoOferta { get; set; }
        public string DetalhesOferta { get; set; }
        public string funcaoOnClickEntrarEmContato { get; set; }
        public string funcaoOnClickVerTelefone { get; set; }
        public string ApresentarLinkEntrarEmContato { get; set; }
        public string UrlFotoDestaqueTamanhoM { get; set; }
        public string UrlFotoDestaqueTamanhoP { get; set; }
        public string ObterMensagemToolTipTrabalho { get; set; }
        public string BlackList { get; set; }
        public int? PosicaoRB { get; set; }
        public bool PaginaAnunciante { get; set; }
        public string OrigemLead { get; set; }
    }
}
