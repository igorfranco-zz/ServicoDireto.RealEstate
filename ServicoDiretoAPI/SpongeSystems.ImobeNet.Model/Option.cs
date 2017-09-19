using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSolutions.ServicoDireto.Model
{
    [DataContract]
    public class Option
    {
        [DataMember]
        public bool Checked { get; set; }
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Text { get; set; }
    }
}
