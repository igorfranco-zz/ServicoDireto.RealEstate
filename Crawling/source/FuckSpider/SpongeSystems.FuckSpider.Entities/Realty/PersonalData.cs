using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.Realty
{
    public class PersonalData : BaseClass
    {
        public PersonalData()
        {
            this.Addresses = new List<Address>();
        }

        [BsonElementAttribute("name")]
        public string Name { get; set; }

        [BsonElementAttribute("email")]
        public string Email { get; set; }

        [BsonElementAttribute("facebook")]
        public string Facebook { get; set; }

        [BsonElementAttribute("twitter")]
        public string Twitter { get; set; }

        [BsonElementAttribute("url")]
        public string Url { get; set; }

        [BsonElementAttribute("phone")]
        public string Phone { get; set; }

        [BsonElementAttribute("cellphone")]
        public string CellPhone { get; set; }

        [BsonElementAttribute("addresses")]
        public IList<Address> Addresses { get; set; }
    }
}
