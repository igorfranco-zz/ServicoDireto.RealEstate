using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSolutions.Core.Attributes
{
    /// <summary>
    /// Define a chave do repositório que terá a tradução do enumerador
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class TranslationAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceName"></param>
        public TranslationAttribute(string resourceName, bool displayable = true)
        {
            if (String.IsNullOrEmpty(resourceName)) throw new ArgumentNullException("resourceName");

            this.ResourceName = resourceName;
            this.Displayable = displayable;
        }

        /// <summary>
        /// Obtém o nome do item que contém a tradução do enumerador
        /// </summary>
        public string ResourceName
        {
            get;
            private set;
        }

        public bool Displayable
        {
            get;
            private set;
        }
    }
}
