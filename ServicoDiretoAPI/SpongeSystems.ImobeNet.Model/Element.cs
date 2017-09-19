using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Internationalization;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using SpongeSolutions.Core.Attributes;
//using System.Data.Objects.DataClasses;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpongeSolutions.ServicoDireto.Model
{
    [NotMapped]
    [DataContract]
    public class ElementExtended : SpongeSolutions.ServicoDireto.Model.Element
    {
        [DataMember]
        public string IDCulture { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCountry")]
        public string IDCountry { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDStateProvince")]
        public int IDStateProvince { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "CountryName")]
        public string CountryName { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "StateProvinceName")]
        public string StateProvinceName { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "CityName")]
        public string CityName { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "CustomerName")]
        public string CustomerName { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "PurposeName")]
        public string PurposeName { get; set; }

        [DataMember]
        public string CategoryName { get; set; }

        [DataMember]
        public string TypeName { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Description")]
        public string Description { get; set; }

        [DataMember]
        public string HierarchyPath { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Name")]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string IconPath { get; set; }

        [DataMember]
        public IList<AttributeExtended> BasicAttributes { get; set; }

        [DataMember]
        public IList<AttributeExtended> StructureAttributes { get; set; }

        [DataMember]
        public bool AddedAsFavorite { get; set; }

        [DataMember]
        public string[] Images { get; set; }

        [DataMember]
        public string[][] AgregatedImages { get; set; }

        //Attributes
        [DataMember]
        public string TotalArea { get; set; }

        [DataMember]
        public string Beds { get; set; }

        [DataMember]
        public string Bathrooms { get; set; }

        [DataMember]
        public string Garages { get; set; }

        [DataMember]
        public string Price { get; set; }
    }

    [DataContract]
    public class Element : BaseEntity
    {
        [DataMember]
        [Key, Column(Order = 1)]
        [HiddenInput(DisplayValue = false)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDElement")]
        public long? IDElement { get; set; }

        [DataMember]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDHierarchyStructure")]
        public System.Int32 IDHierarchyStructure { get; set; }

        [DataMember]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDHierarchyStructureParent")]
        public System.Int32 IDHierarchyStructureParent { get; set; }

        [DataMember]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDPurpose")]
        public System.Int32 IDPurpose { get; set; }

        [DataMember]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCity")]
        public System.Int32 IDCity { get; set; }

        [DataMember]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCustomer")]
        public System.Int32 IDCustomer { get; set; }

        [DataMember]
        [StringLength(250, ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "StringLength")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDItemSite")]
        public System.String IDItemSite { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Latitude")]
        public decimal Latitude { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Longitude")]
        public decimal Longitude { get; set; }

        [DataMember]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "ShowAddress")]
        public System.Boolean ShowAddress { get; set; }

        [DataMember]
        public System.Boolean? AllowRatting { get; set; }

        [DataMember]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [StringLength(1000, ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "StringLength")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Address")]
        public System.String Address { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Neighborhood")]
        public System.String Neighborhood { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "PageView")]
        public UInt64? PageView { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "DetailView")]
        public UInt64? DetailView { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IsPromoted")]
        public System.Boolean IsPromoted { get; set; }

        [DataMember]
        [StringLength(300, ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "StringLength")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Url")]
        public System.String Url { get; set; }

        [DataMember]
        public string DefaultPicturePath { get; set; }
    }
}
