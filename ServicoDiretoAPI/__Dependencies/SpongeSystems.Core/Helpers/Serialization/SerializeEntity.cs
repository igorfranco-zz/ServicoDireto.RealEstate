using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace SpongeSystems.Core.Helpers.Serialization
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://tempuri.org/")]
    public partial class SerializeEntity : object, System.ComponentModel.INotifyPropertyChanged
    {
        #region [Constructors]
        public SerializeEntity() { }


        public SerializeEntity(XmlDocument xmlSource, string objectTypeName, string assemblyTypeName)
        {
            this.xmlSourceField = xmlSource;
            this.objectTypeNameField = objectTypeName;
            this.assemblyTypeNameField = assemblyTypeName;
        }
        #endregion


        private System.Xml.XmlDocument xmlSourceField;

        private string objectTypeNameField;

        private string assemblyTypeNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public System.Xml.XmlDocument XmlSource
        {
            get
            {
                return this.xmlSourceField;
            }
            set
            {
                this.xmlSourceField = value;
                this.RaisePropertyChanged("XmlSource");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ObjectTypeName
        {
            get
            {
                return this.objectTypeNameField;
            }
            set
            {
                this.objectTypeNameField = value;
                this.RaisePropertyChanged("ObjectTypeName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string AssemblyTypeName
        {
            get
            {
                return this.assemblyTypeNameField;
            }
            set
            {
                this.assemblyTypeNameField = value;
                this.RaisePropertyChanged("AssemblyTypeName");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
