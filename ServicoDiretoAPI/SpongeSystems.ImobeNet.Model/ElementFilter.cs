using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace SpongeSolutions.ServicoDireto.Model
{
    [DataContract]
    public class ElementFilter
    {
        public ElementFilter()
        {
            this.FilterAttribute = new List<FilterAttribute>();
        }

        [DataMember]
        public string IDCustomer { get; set; }

        [DataMember]
        public string IDCulture { get; set; }

        [DataMember]
        public string IDCountry { get; set; }

        [DataMember]
        public string IDStateProvince { get; set; }

        [DataMember]
        public string IDCity { get; set; }

        [DataMember]
        [XmlArray("Purposes")]
        [XmlArrayItem("IDPurpose")]
        public string[] IDPurpose { get; set; }

        [DataMember]
        public string IDHierarchyStructureParent { get; set; } //Categoria

        [DataMember]
        public string IDHierarchyStructure { get; set; } //Tipo

        [DataMember]
        public string Radius { get; set; }

        [DataMember]
        public string LatitudeBase { get; set; }

        [DataMember]
        public string LongitudeBase { get; set; }

        [DataMember]
        public string OrderBy { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Description")]
        public string Title { get; set; }

        [DataMember]
        public string BaseAddress { get; set; }

        [DataMember]
        [XmlArray("Attributes")]
        [XmlArrayItem("Attribute")]
        public List<FilterAttribute> FilterAttribute { get; set; }

        [DataMember]
        public int PageIndex { get; set; }

        [DataMember]
        public int TotalRecodsPerPage { get; set; }

        [DataMember]
        public int GroupIn { get; set; }

        [DataMember] 
        public bool UseInternalAuth { get; set; }
    }

    [DataContract]
    public class FilterAttribute
    {
        [DataMember]
        public string Acronym { get; set; }

        [DataMember]
        public string IDFilter { get; set; }

        [DataMember]
        public string InitialValue { get; set; }

        [DataMember]
        public int ActiveView { get; set; }

        [DataMember]
        public string FinalValue { get; set; }
    }
}
