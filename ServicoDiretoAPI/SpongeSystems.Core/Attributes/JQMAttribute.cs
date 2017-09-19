using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSolutions.Core.Attributes
{
    public class JQMAttributeAttribute : Attribute
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public JQMAttributeAttribute(string name, object value = null)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
