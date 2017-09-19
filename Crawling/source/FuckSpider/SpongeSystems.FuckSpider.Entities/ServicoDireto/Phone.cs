using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.ServicoDireto
{
    public class Phone
    {
        [BsonElementAttribute("number")]
        public string Number { get; set; }

        [BsonElementAttribute("type")]
        public string Type { get; set; }
    }
}
