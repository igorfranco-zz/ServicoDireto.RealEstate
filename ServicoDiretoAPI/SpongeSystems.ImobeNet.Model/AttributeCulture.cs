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
    public class AttributeCultureExtended : AttributeCulture
    {
        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCulture")]
        public string CultureName { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Path")]
        public string IconPath { get; set; }
    }

    [DataContract]
    public class AttributeCulture
    {
        [StringLength(5, ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "StringLength")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCulture")]
        [Key, Column(Order = 1)]
        [DataMember]
        public System.String IDCulture { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDAttribute")]
        [Key, Column(Order = 2)]
        [DataMember]
        public System.Int32 IDAttribute { get; set; }

        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required_Name")]
        [StringLength(250, ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "StringLength")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Name")]
        [DataMember]
        public System.String Name { get; set; }

        [DataMember]
        [StringLength(50, ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "StringLength")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Value")]
        public System.String Value { get; set; }
    }
}
