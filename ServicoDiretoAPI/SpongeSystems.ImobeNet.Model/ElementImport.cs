using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSolutions.ServicoDireto.Model
{
    [DataContract]
    public class ImageImport
    {
        [DataMember]
        public string UrlImageMain { get; set; }
        [DataMember]
        public string UrlImageSize1 { get; set; }
        [DataMember]
        public string UrlImageSize2 { get; set; }
        [DataMember]
        public string UrlImageSize3 { get; set; }
        [DataMember]
        public string UrlImageSize4 { get; set; }
        [DataMember]
        public bool IsMain { get; set; }
    }

    [DataContract]
    public class AttributeImport
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Value { get; set; }
    }

    [DataContract]
    public class CustomerImport
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Logo { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Phone { get; set; }
    }

    [DataContract]
    public class ElementImport
    {
        public ElementImport()
        {
            this.Customer = new CustomerImport();
            this.Attributes = new List<AttributeImport>();
            this.Images = new List<ImageImport>();
        }

        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public string DefaultPicture { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string CityName { get; set; }
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public string StateProvinceName { get; set; }
        [DataMember]
        public string Neighborhood { get; set; }
        [DataMember]
        public string PurposeName { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public string TypeName { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public decimal Latitude { get; set; }
        [DataMember]
        public decimal Longitude { get; set; }
        [DataMember]
        public CustomerImport Customer { get; set; }
        [DataMember]
        public List<AttributeImport> Attributes { get; set; }
        [DataMember]
        public List<ImageImport> Images { get; set; }
    }
}
