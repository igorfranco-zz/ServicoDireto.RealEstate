using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.Realty
{
    public class Agency : PersonalData, IMongoEntity
    {
        public Agency() : base()
        {
            this.Contacts = new List<Contact>();
        }
        
        public ObjectId Id { get; set; }
        [BsonElementAttribute("contacts")]
        public List<Contact> Contacts { get; set; }
    }
}
