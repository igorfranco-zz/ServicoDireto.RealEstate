using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SpongeSolutions.Core.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpongeSolutions.ServicoDireto.Model
{
    [NotMapped]
    public class EmailExtended : Email 
    {
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCustomerFrom")]
        public string IDCustomerFromName { get; set; }

        [GridConfig(Index = 2)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCustomerTo")]
        public string IDCustomerToName { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDElement")]
        public string ElementName { get; set; }
    }

    public class Email : BaseEntity
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        [GridConfig(CreateCheckBox = true,Index = 1)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDEmail")]
        public long? IDEmail { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDEmailParent")]
        public long? IDEmailParent { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCustomerFrom")]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        public int? IDCustomerFrom { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCustomerTo")]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        public int? IDCustomerTo { get; set; }

        [GridConfig(Index = 3)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Subject")]        
        public string Subject { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Content")]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        public string Content { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDElement")]
        public long? IDElement { get; set; }       

        public bool? Read { get; set; }
    }
}

