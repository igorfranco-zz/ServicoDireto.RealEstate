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
namespace SpongeSolutions.ServicoDireto.Model
{
    public class FilterExtended : Filter
    {
        [GridConfig(Index = 4)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Name")]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        public System.String AttributeName { get; set; }

        [GridConfig(Index = 5)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "DisplayMask")]
        public System.String DisplayMask { get; set; }
    }

    public class Filter : BaseEntity
    {
        [HiddenInput(DisplayValue = false)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDFilter")]
        [Key, Column(Order = 1)]
        [GridConfig(CreateCheckBox = true, CreateEditLink = true, Index = 1)]
        public System.Int32? IDFilter { get; set; }

        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDAttribute")]
        public int IDAttribute { get; set; }

        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "InitialValue")]
        public string InitialValue { get; set; }

        [GridConfig(Index = 2)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "DefaultValue")]
        public string DefaultValue { get; set; }

        [GridConfig(Index = 3)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "FinalValue")]
        public string FinalValue { get; set; }

        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "ComponentType")]
        public short ComponentType { get; set; }
    }
}
