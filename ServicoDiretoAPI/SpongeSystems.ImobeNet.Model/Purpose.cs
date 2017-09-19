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
    [NotMapped]
    [DataContract]
    public class PurposeExtended : Purpose
    {
        [DataMember]
        [GridConfig(CreateImage = true, Index = 2)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Path")]
        public string IconPath { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "Description")]
        [GridConfig(Index = 3)]
        public string Description { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDIcon")]
        public string IconName { get; set; }
    }

    [DataContract]
    public class Purpose : BaseEntity
    {
        [DataMember]
        [Key, Column(Order = 1)]
        [HiddenInput(DisplayValue = false)]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDPurpose")]
        [GridConfig(CreateCheckBox = true, CreateEditLink = true, Index = 1)]
        public System.Int32? IDPurpose { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Label), Name = "IDIcon")]
        [Required(ErrorMessageResourceType = typeof(SpongeSolutions.ServicoDireto.Internationalization.Validation), ErrorMessageResourceName = "Required")]
        public System.Int32 IDIcon { get; set; }

        [NotMapped]
        [DataMember]
        public IList<PurposeCultureExtended> Culture { get; set; }

        [NotMapped]
        [DataMember]
        public IList<HierarchyStructureExtended> Category { get; set; }
    }
}
