using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SpongeSolutions.Core.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpongeSolutions.ServicoDireto.Model.Advertisement
{
    public class AdsCategoryCulture
    {
        [Key, Column(Order = 1)]
        [HiddenInput(DisplayValue = false)]
        [GridConfig(CreateCheckBox = true, Index = 1)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDAdsCategory")]
        public int? IDAdsCategory { get; set; }

        [Key, Column(Order = 2)]
        [StringLength(5, ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "StringLength")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCulture")]
        public System.String IDCulture { get; set; }

        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Name")]
        public System.String Name { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCulture")]
        [NotMapped]
        public string CultureName { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Path")]
        public string IconPath { get; set; }
    }
}
