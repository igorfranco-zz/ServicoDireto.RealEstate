using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Internationalization;
using SpongeSolutions.Core.Attributes;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpongeSolutions.ServicoDireto.Model
{
    [NotMapped]
    [DataContract]
    public class CityExtended : City
    {
        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCountry")]
        public string IDCountry { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "CountryName")]
        public string CountryName { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "StateProvinceName")]
        public string StateName { get; set; }
    }

    [DataContract]
    public partial class City : BaseEntity
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCity")]
        [GridConfig(CreateCheckBox = true, CreateEditLink = true, RelatedFieldName = new string[] { "Name" }, Index = 1)]
        [DataMember]
        public System.Int32? IDCity { get; set; }

        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDStateProvince")]
        [DataMember]
        public int IDStateProvince { get; set; }

        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "StringLength")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Name")]
        [GridConfig(Index = 2)]
        [DataMember]
        public System.String Name { get; set; }
    }
}
