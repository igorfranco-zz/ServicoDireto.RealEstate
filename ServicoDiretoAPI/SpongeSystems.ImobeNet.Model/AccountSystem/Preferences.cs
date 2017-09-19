using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;

namespace SpongeSolutions.ServicoDireto.Model.AccountSystem
{
    [Serializable]
    public class Preferences
    {
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Radius")]
        public int? Radius { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "ShowRadiusCircle")]
        public bool? ShowRadiusCircle { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "AllowShowAddress")]
        public bool? AllowShowAddress { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "FavoriteObject")]
        public List<long> FavoriteObject { get; set; }

        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCustomer")]
        public int IDCustomer { get; set; }

        public bool AllowNewsletter { get; set; }
    }
}
