using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.Realty
{
    public class Address
    {
        [BsonElementAttribute("name")]
        public string Name { get; set; }

        [BsonElementAttribute("latitute")]
        public decimal Latitute { get; set; }

        [BsonElementAttribute("longitude")]
        public decimal Longitude { get; set; }

        [BsonElementAttribute("postalcode")]
        public string PostalCode { get; set; }

        [BsonElementAttribute("neighborhood")]
        public string Neighborhood { get; set; }

        [BsonElementAttribute("city")]
        public string City { get; set; }

        [BsonElementAttribute("state_province")]
        public string StateProvince { get; set; }

        [BsonElementAttribute("country")]
        public string Country { get; set; }
    }
}
