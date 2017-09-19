using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Internationalization;
using SpongeSolutions.Core.Attributes;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using System.Runtime.Serialization;

namespace SpongeSolutions.ServicoDireto.Model
{
    [DataContract]
    public partial class Country : BaseEntity
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [StringLength(5, ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "StringLength")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCountry")]
        [GridConfig(CreateCheckBox = true, CreateEditLink = true, RelatedFieldName = new string[] { "IDCountry", "Name" }, Index = 1)]
        [DataMember]
        public System.String IDCountry { get; set; }

        [GridConfig(Index = 2)]
        [StringLength(50, ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "StringLength")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Name")]
        [DataMember]
        public System.String Name { get; set; }
    }
}
