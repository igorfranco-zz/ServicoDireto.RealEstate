using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSolutions.Core.RestFul
{
    [System.Runtime.Serialization.DataContract(Name = "responseData")]
    public class ResponseData<T>
    {
        public ResponseData()
        {
        }

        [System.Runtime.Serialization.DataMember(Name = "results")]
        public List<T> Result { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "responseDetails")]
        public string Details { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "responseStatus")]
        public Enums.ResponseStatus Status { get; set; }

        public ResponseData(T value, string details, Enums.ResponseStatus status)
        {
            this.Result = new List<T>();
            this.Result.Add(value);
            this.Details = details;
            this.Status = status;
        }
    }

    [System.Runtime.Serialization.DataContract(Name = "response")]
    public class Response<T>
    {
        [System.Runtime.Serialization.DataMember(Name = "responseData")]
        public ResponseData<T> ResponseData { get; set; }

        public static Response<T> WrapResponse(T value, string details, Enums.ResponseStatus status)
        {
            ResponseData<T> responseData = new ResponseData<T>(value, details, status);
            return new Response<T>() { ResponseData = new ResponseData<T>(value, details, status) };
        }

        public static Response<T> WrapResponse(T value, Enums.ResponseStatus status)
        {
            return WrapResponse(value, null, status);
        }
    }
}
