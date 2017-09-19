using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Internationalization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpongeSolutions.ServicoDireto.Model
{
    [NotMapped]
    [DataContract]
    public class PurposeCultureExtended : PurposeCulture {
        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCulture")]
        [NotMapped]
        public string CultureName { get; set; }

        [DataMember]
        [NotMapped]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Path")]
        public string IconPath { get; set; }
    }

    [DataContract]
    public class PurposeCulture
    {
        [DataMember]
        [HiddenInput(DisplayValue = false)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDPurpose")]
        [Key, Column(Order = 1)]
        public System.Int32? IDPurpose { get; set; }

        [DataMember]
        [HiddenInput(DisplayValue = false)]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [StringLength(5, ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "StringLength")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCulture")]
        [Key, Column(Order = 2)]
        public System.String IDCulture { get; set; }

        [DataMember]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "StringLength")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Description")]
        public System.String Description { get; set; }
    }
}
