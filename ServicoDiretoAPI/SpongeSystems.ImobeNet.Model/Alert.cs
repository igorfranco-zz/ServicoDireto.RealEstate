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

namespace SpongeSolutions.ServicoDireto.Model
{
    [NotMapped]
    public class AlertExtended : SpongeSolutions.ServicoDireto.Model.Alert
    {
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCountry")]
        public string IDCountry { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDStateProvince")]
        public int IDStateProvince { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "CountryName")]
        public string CountryName { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "StateProvinceName")]
        public string StateProvinceName { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "CityName")]
        public string CityName { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "CustomerName")]
        public string CustomerName { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "PurposeName")]
        public string PurposeName { get; set; }

        public string CategoryName { get; set; }

        public string TypeName { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Description")]
        public string Description { get; set; }

        [GridConfig(Index = 4)]
        public string HierarchyPath { get; set; }

        public IList<AttributeExtended> BasicAttributes { get; set; }
    }

    [Serializable]
    public class Alert : BaseEntity
    {
        [Key, Column(Order = 1)]
        [HiddenInput(DisplayValue = false)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDAlert")]
        [GridConfig(CreateCheckBox = true, CreateEditLink = true, Index = 1, RelatedFieldName = new string[] { "Name" })]
        public long? IDAlert { get; set; }

        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Name")]
        [GridConfig(Index = 2)]
        public string Name { get; set; }

        [GridConfig(Index = 3)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Address")]
        public string Address { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDHierarchyStructure")]
        public System.Int32? IDHierarchyStructure { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDHierarchyStructureParent")]
        public System.Int32? IDHierarchyStructureParent { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCity")]
        public System.Int32? IDCity { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCustomer")]
        public System.Int32 IDCustomer { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Radius")]
        public decimal? Radius { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Latitude")]
        public decimal? Latitude { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Longitude")]
        public decimal? Longitude { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDPurpose")]
        public System.Int32? IDPurpose { get; set; }
    }
}
