using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace SpongeSystems.Spider.Entities.ZapImoveis
{
    public class ResultContainer: BaseClass, IMongoEntity
    {
        public ObjectId Id { get; set; }
        public Resultado Resultado { get; set; }
        public IList<Breadcrumb> breadcrumb { get; set; }
        public IList<Silo> silos { get; set; }
        public object Formato { get; set; }
        public string TituloModalAlerta { get; set; }
        public string FaixaPrecoModalAlerta { get; set; }
        public bool IsContabilizarAssincrono { get; set; }
        public bool IsPaginaAnunciante { get; set; }
        public Vitrine Vitrine { get; set; }      
    }
}
