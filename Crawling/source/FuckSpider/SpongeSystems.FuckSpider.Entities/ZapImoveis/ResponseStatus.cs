using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities.ZapImoveis
{
    public class ResponseStatus
    {
        public object ErrorCode { get; set; }
        public object Message { get; set; }
        public object StackTrace { get; set; }
        public object Errors { get; set; }
    }
}
