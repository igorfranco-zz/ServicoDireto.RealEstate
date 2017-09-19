using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.ZapImoveis
{
    public class PriceSpecification
    {
        public string @type { get; set; }
        public string priceCurrency { get; set; }
    }

    public class Seller
    {
        public string @type { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Geo
    {
        public string @type { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

    public class AddressCountry
    {
        public string @type { get; set; }
        public string name { get; set; }
    }

    public class Address
    {
        public string streetAddress { get; set; }
        public string addressLocality { get; set; }
        public string addressRegion { get; set; }
        public AddressCountry addressCountry { get; set; }
        public string postalCode { get; set; }
    }

    public class Photo
    {
        public string @type { get; set; }
        public string contentUrl { get; set; }
    }

    public class Object
    {
        public string @type { get; set; }
        public string @id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public Geo geo { get; set; }
        public Address address { get; set; }
        public IList<Photo> photo { get; set; }
    }

    public class LDScript
    {
        public string @context { get; set; }
        public string @type { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public string logo { get; set; }
        public string additionalType { get; set; }
        public string price { get; set; }
        public PriceSpecification priceSpecification { get; set; }
        public Seller seller { get; set; }
        [JsonProperty(PropertyName = "object")]
        public Object object_ { get; set; }
}

}
