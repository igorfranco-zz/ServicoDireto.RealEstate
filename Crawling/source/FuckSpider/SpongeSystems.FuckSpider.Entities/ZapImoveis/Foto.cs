using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.ZapImoveis
{
    public class Foto
    {
        public string UrlImagem { get; set; }
        public string UrlImagemTamanhoPP { get; set; }
        public string UrlImagemTamanhoP { get; set; }
        public string UrlImagemTamanhoM { get; set; }
        public string UrlImagemTamanhoG { get; set; }
        public string UrlImagemTamanhoGG { get; set; }
        public bool Principal { get; set; }
        public string Descricao { get; set; }
        public int Altura { get; set; }
        public int Largura { get; set; }
        public int Origem { get; set; }
        public int TipoOferta { get; set; }
    }
}
