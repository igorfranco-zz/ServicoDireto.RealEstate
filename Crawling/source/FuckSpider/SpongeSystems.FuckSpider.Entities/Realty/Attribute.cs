using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.Realty
{
    public class Attribute 
    {
        [BsonElementAttribute("name")]
        public string Name { get; set; }

        [BsonElementAttribute("value")]
        public string Value { get; set; }
    }
}
