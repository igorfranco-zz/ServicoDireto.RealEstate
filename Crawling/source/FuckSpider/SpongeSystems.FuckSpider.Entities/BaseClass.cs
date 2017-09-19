using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities
{
    public class BaseClass
    {
        [BsonElementAttribute("create_date")]
        //[BsonDateTimeOptions(DateOnly = true)]
        public DateTime CreateDate { get; set; }

        [BsonElementAttribute("update_date")]
        public DateTime ModifyDate { get; set; }

        [BsonElementAttribute("created_by")]
        public string CreatedBy { get; set; }

        [BsonElementAttribute("modified_by")]
        public string ModifiedBy { get; set; }

        [BsonElementAttribute("status")]
        [BsonRepresentation(BsonType.String)]
        public Enums.StatusType Status { get; set; }
    }
}
