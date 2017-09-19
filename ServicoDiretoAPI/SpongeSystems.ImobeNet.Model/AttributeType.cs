using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using SpongeSolutions.ServicoDireto.Internationalization;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using SpongeSolutions.Core.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpongeSolutions.ServicoDireto.Model
{
    [DataContract]
    [NotMapped]
    public class AttributeTypeExtended : AttributeType
    {
        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Description")]
        public System.String Description { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDIcon")]
        public string IconName { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Path")]
        public string IconPath { get; set; }
    }

    [DataContract]
    public class AttributeType : BaseEntity
    {
        [DataMember]
        [HiddenInput(DisplayValue = false)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDAttributeType")]
        [Key, Column(Order = 1)]
        public System.Int32? IDAttributeType { get; set; }

        [DataMember]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDIcon")]
        public System.Int32 IDIcon { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Hidden")]
        public bool Hidden { get; set; }

        [DataMember]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Acronym")]
        public virtual System.String Acronym { get; set; }

        [NotMapped]
        [DataMember]
        public IList<AttributeTypeCultureExtended> Culture { get; set; }

        [NotMapped]
        [DataMember]
        public IList<Option> VinculatedAttributes;

        [NotMapped]
        [DataMember]
        public IList<Option> AvailableAttributes;
    }
}
