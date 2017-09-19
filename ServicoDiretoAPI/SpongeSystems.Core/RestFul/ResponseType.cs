using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSolutions.Core.RestFul
{
    [System.Runtime.Serialization.DataContract(Name = "response")]
    public static class ResponseType<T>
    {
        public static ResponseData<T> GenerateResponse(T value, string details, Enums.ResponseStatus status)
        {
            ResponseData<T> responseData = new ResponseData<T>(value, details, status);
            return responseData;
        }
    }
}
