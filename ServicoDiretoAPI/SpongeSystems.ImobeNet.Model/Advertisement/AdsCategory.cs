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
    [NotMapped]
    public class AdsCategoryExtended : AdsCategory
    {        
        [GridConfig(Index = 2, Style = "span-1")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Name")]
        public string Name { get; set; }
    }

    public class AdsCategory : BaseEntity
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        [GridConfig(CreateCheckBox = true, Index = 1, CreateEditLink = true, Style="span-1")]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDAdsCategory")]
        public int? IDAdsCategory { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "CustomerName")]
        public int? IDCustomer { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "CustomerName")]
        public string CustomerName { get; set; }
    }
}
