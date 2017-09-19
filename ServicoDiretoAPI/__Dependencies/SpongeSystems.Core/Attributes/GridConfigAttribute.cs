using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSystems.Core.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class GridConfigAttribute : Attribute
    {
        //public bool Hidden { get; set; }
        public bool HiddenOnWindowMode { get; set; }
        public bool CreateCheckBox { get; set; }
        public bool CreateEditLink { get; set; }
        public string Style { get; set; }           
        public bool CreateImage { get; set; }           
        public Type EnumType { get; set; }
        public string[] RelatedFieldName { get; set; }
        public int Index { get; set; }
        public bool Hidden { get; set; }
    }
}