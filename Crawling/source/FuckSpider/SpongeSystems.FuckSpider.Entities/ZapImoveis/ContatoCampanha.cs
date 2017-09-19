using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.ZapImoveis
{
    public class ContatoCampanha
    {
        public int ID { get; set; }
        public object CampanhaID { get; set; }
        public object Nome { get; set; }
        public string Email { get; set; }
        public Telefone Telefone { get; set; }
        public object UrlAtendimentoOnline { get; set; }
        public object UrlHotsite { get; set; }
    }
}
