using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.ServicoDireto
{
    public class ProductAndService
    {
        [BsonElementAttribute("name")]
        public string Name { get; set; }
    }
}
