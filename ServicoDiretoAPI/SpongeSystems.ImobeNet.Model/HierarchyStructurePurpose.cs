using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Internationalization;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpongeSolutions.ServicoDireto.Model
{
    public class HierarchyStructurePurpose
    {
        [HiddenInput(DisplayValue = false)]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDHierarchyStructure")]
        [Key, Column(Order = 1)]
        public System.Int32? IDHierarchyStructure { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDPurpose")]
        [Key, Column(Order = 2)]
        public System.Int32 IDPurpose { get; set; }
    }
}
