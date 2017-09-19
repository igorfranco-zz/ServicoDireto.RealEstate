using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSystems.Spider.Entities.Realty
{
    ///TODO>: 
    ///---- BUSCAR DO GOOGLE OS DADOS DE PONTOS COMERCIAS PROXIMOS
    public class Property : BaseClass, IMongoEntity
    {
        public Property()
        {
            this.Addresses = new List<Address>();
            this.Attributes = new List<Attribute>();
            this.Photos = new List<Photo>();
        }

        public ObjectId Id { get; set; }

        [BsonElementAttribute("agency_id")]
        public ObjectId AgencyID { get; set; }

        [BsonElementAttribute("contact_id")]
        public ObjectId ContactID { get; set; }

        [BsonElementAttribute("purpose_id")]
        public ObjectId PurposeID { get; set; }

        [BsonElementAttribute("category_id")]
        public ObjectId CategoryID { get; set; }

        [BsonElementAttribute("name")]
        public string Name { get; set; }

        [BsonElementAttribute("website_id")]
        public string WebsiteID { get; set; } //identificado do anuncio no site de origem

        [BsonElementAttribute("url")]
        public string Url { get; set; } //identificação do anuncio no site de origem        

        [BsonElementAttribute("details")]
        public string Details { get; set; }

        [BsonElementAttribute("house_size")]
        public decimal HouseSize { get; set; } //Área construída

        [BsonElementAttribute("lot_size")]
        public decimal LotSize { get; set; }   //Terreno

        [BsonElementAttribute("price")]
        public decimal Price { get; set; }

        [BsonElementAttribute("garages")]
        public short Garages { get; set; }

        [BsonElementAttribute("bathrooms")]
        public short Bathrooms { get; set; }

        [BsonElementAttribute("bedrooms")]
        public short Bedrooms { get; set; }

        [BsonElementAttribute("addresses")]
        public IList<Address> Addresses { get; set; }

        [BsonElementAttribute("attributes")]
        public IList<Attribute> Attributes { get; set; }

        [BsonElementAttribute("photos")]
        public IList<Photo> Photos { get; set; }
    }
}
