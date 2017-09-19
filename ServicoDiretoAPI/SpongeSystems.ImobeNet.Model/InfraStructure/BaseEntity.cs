using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using SpongeSolutions.Core.Attributes;
using System.Runtime.Serialization;

namespace SpongeSolutions.ServicoDireto.Model.InfraStructure
{
    //[Serializable()]
    [DataContract]
    public class BaseEntity
    {
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Status")]
        [GridConfig(EnumType = typeof(SpongeSolutions.ServicoDireto.Model.InfraStructure.Enums.StatusType), HiddenOnWindowMode = true, Index = 999, Style = "span-1")]
        [DataMember]
        public short Status { get; set; }

        [GridConfig(HiddenOnWindowMode = true)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "CreateDate")]
        [DataMember]
        public System.DateTime? CreateDate { get; set; }

        [GridConfig(HiddenOnWindowMode = true)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "ModifyDate")]
        [DataMember]
        public System.DateTime? ModifyDate { get; set; }

        [GridConfig(HiddenOnWindowMode = true)]
        [StringLength(50, ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "StringLength")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "CreatedBy")]
        [DataMember]
        public System.String CreatedBy { get; set; }

        [GridConfig(HiddenOnWindowMode = true)]
        [StringLength(50, ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "StringLength")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "ModifiedBy")]
        [DataMember]
        public System.String ModifiedBy { get; set; }
    }
}
