using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.ServicoDireto
{
    public class Link : BaseClass, IMongoEntity
    {
        public Link() 
        {
            this.Activities = new List<Activity>();
            this.Phones = new List<Phone>();
            this.ProductsAndServices = new List<ProductAndService>();
            this.Emails = new List<Email>();
        }

        public ObjectId Id { get; set; }

        [BsonElementAttribute("link_id")]
        public string LinkID { get; set; }

        [BsonElementAttribute("name")]
        public string Name { get; set; }

        [BsonElementAttribute("site")]
        public string Site { get; set; }

        [BsonElementAttribute("details")]
        public string Details { get; set; }

        [BsonElementAttribute("logo")]
        public string Logo { get; set; }

        [BsonElementAttribute("address")]
        public string Address { get; set; }

        [BsonElementAttribute("address_number")]
        public string AddressNumber { get; set; }

        [BsonElementAttribute("longitude")]
        public string Longitude { get; set; }

        [BsonElementAttribute("latitude")]
        public string Latitude { get; set; }

        [BsonElementAttribute("url")]
        public string Url { get; set; }

        [BsonElementAttribute("state")]
        public string State { get; set; }

        [BsonElementAttribute("city")]
        public string City { get; set; }

        [BsonElementAttribute("neighborhood")]
        public string Neighborhood { get; set; }

        [BsonElementAttribute("category")]
        public string Category { get; set; }

        [BsonElementAttribute("phones")]
        public IList<Phone> Phones { get; set; }

        [BsonElementAttribute("activities")]
        public IList<Activity> Activities { get; set; }

        [BsonElementAttribute("products_and_services")]
        public IList<ProductAndService> ProductsAndServices { get; set; }

        [BsonElementAttribute("emails")]
        public IList<Email> Emails { get; set; }
    }
}
