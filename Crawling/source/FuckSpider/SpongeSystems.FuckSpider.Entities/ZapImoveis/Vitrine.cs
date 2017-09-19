using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.ZapImoveis
{
    public class Vitrine
    {
        public string SectionClass { get; set; }
        public OfertaTopo OfertaTopo { get; set; }
        public IList<OfertasSimilare> OfertasSimilares { get; set; }
    }

}
