using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSolutions.Core.Model
{
    public class ResultMessage
    {
        public Core.Extension.Enums.MessageType MessageType { get; set; }
        public string Message { get; set; }

        public ResultMessage(Core.Extension.Enums.MessageType messageType, string message) 
        {
            this.Message = message;
            this.MessageType = messageType;
        }
    }
}
