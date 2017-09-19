using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSolutions.Core.RestFul
{
    public class ResponseMessage
    {
        #region [Properties]
        public string Message { get; set; }
        public Enums.ResponseStatus StatusCode { get; set; }
        #endregion

        #region [Constructor]

        public ResponseMessage(string message)
        {
            this.Message = message;
            this.StatusCode = Enums.ResponseStatus.OK;
        }

        public ResponseMessage(string message, Enums.ResponseStatus statusCode)
        {
            this.Message = message;
            this.StatusCode = statusCode;
        }

        #endregion 
    }
}
