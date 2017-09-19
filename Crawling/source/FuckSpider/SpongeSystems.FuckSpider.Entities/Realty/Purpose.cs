using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.Realty
{
    //Sell, Rent
    public class Purpose : BaseClass, IMongoEntity
    {
        public ObjectId Id { get; set; }

        [BsonElementAttribute("name")]
        public string Name { get; set; }
    }
}
