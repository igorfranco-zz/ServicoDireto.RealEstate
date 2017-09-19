using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.ZapImoveis
{
    public class OfertasSimilare
    {
        public string ValorFormatado { get; set; }
        public string Bairro { get; set; }
        public int DormitoriosMinima { get; set; }
        public int DormitoriosMaxima { get; set; }
        public int SuitesMinima { get; set; }
        public int SuitesMaxima { get; set; }
        public int AreaMinima { get; set; }
        public int AreaMaxima { get; set; }
        public string NomeAnunciante { get; set; }
        public bool PossuiSeloQualidade { get; set; }
        public string URLLogoAnunciante { get; set; }
        public string URLImagemPrincipal { get; set; }
        public object URLOfertasAnunciante { get; set; }
        public string URLOferta { get; set; }
        public string IdFavorito { get; set; }
        public int CodigoOferta { get; set; }
        public int Tipo { get; set; }
        public int Transacao { get; set; }
        public string ClassBotaoFavorito { get; set; }
        public string AllClassBotaoFavorito { get; set; }
        public string TitleFavorito { get; set; }
        public string DetalhesOferta { get; set; }
        public string DormitoriosFormatado { get; set; }
        public string SuitesFormatado { get; set; }
        public string AreaFormatada { get; set; }
        public string FuncaoOnClickFavorito { get; set; }
    }
}
