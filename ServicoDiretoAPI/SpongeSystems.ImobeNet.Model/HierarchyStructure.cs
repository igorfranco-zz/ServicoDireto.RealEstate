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
using System.Runtime.Serialization;

namespace SpongeSolutions.ServicoDireto.Model
{
    [DataContract]
    [NotMapped]
    public class HierarchyStructureExtended : HierarchyStructure
    {
        [GridConfig(CreateImage = true, Index = 2)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Path")]
        [DataMember]
        public string IconPath { get; set; }

        [DataMember]
        [GridConfig(Index = 3)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Description")]
        public System.String Description { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDCulture")]
        public string CultureName { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDIcon")]
        public string IconName { get; set; }

        [DataMember]
        public bool Checked { get; set; }
    }

    [DataContract]
    public class HierarchyStructureBasic
    {
        [DataMember]
        public System.Int32 IDHierarchyStructure { get; set; }
        [DataMember]
        public System.String Description { get; set; }
    }

    [DataContract]
    public class HierarchyStructure : BaseEntity
    {
        [NotMapped]
        [DataMember]
        public IList<HierarchyStructureCulture> Culture { get; set; }

        [DataMember]
        [HiddenInput(DisplayValue = false)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDHierarchyStructure")]
        [Key, Column(Order = 1)]
        [GridConfig(CreateCheckBox = true, CreateEditLink = true, Index = 1)]
        public System.Int32? IDHierarchyStructure { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDHierarchyStructureParent")]
        public System.Int32? IDHierarchyStructureParent { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDIcon")]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        public System.Int32 IDIcon { get; set; }
    }
}
