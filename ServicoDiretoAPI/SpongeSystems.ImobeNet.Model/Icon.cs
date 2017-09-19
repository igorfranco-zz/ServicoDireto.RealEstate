using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using SpongeSolutions.Core.Attributes;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using System.Runtime.Serialization;

namespace SpongeSolutions.ServicoDireto.Model
{
    [DataContract]
    public class Icon : BaseEntity
    {
        [Key]
        [GridConfig(CreateCheckBox = true, CreateEditLink = true, RelatedFieldName = new string[] { "Name" }, Index = 1)]
        [HiddenInput(DisplayValue = false)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDIcon")]
        [DataMember]
        public System.Int32? IDIcon { get; set; }

        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "StringLength")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Path")]
        [GridConfig(CreateImage = true, Index = 3)]
        [DataMember]
        public System.String Path { get; set; }

        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "StringLength")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Name")]
        [GridConfig(Index = 2)]
        [DataMember]
        public System.String Name { get; set; }
    }

}
