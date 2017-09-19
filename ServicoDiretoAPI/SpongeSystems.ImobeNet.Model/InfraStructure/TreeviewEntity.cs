
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSolutions.ServicoDireto.Model.InfraStructure
{
    [Serializable]
    public class TreeviewEntity
    {
        private List<TreeviewEntity> _children;
        public string ID { get; set; }
        public bool Clickable { get; set; }
        public string Name { get; set; }
        public dynamic JSONAttributes { get; set; }
        public List<TreeviewEntity> Children
        {
            get
            {
                if (_children == null)
                    _children = new List<TreeviewEntity>();
                return _children;
            }
        }
    }
}
