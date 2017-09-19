using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Internationalization;
//using SpongeSolutions.Core.Attributes;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpongeSolutions.ServicoDireto.Model
{
    public partial class FilterPurpose
    {
        [Key, Column(Order = 1)]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        public System.Int32 IDFilter { get; set; }

        [Key, Column(Order = 2)]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        public System.Int32 IDPurpose { get; set; }

    }
}
