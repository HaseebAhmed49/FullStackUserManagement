using System;
using UserManagement_WebAPI.Data.Enums;

namespace UserManagement_WebAPI.Data.Entities
{
    public class ResponseModel
    {
        public ResponseModel(ResponseCode responseCode,string responseMessage,object dataset)
        {
            ResponseCode = responseCode;
            ResponseMessage = responseMessage;
            DataSet = dataset;
        }

        public ResponseCode ResponseCode { get; set; }

        public string ResponseMessage { get; set; }

        public object DataSet { get; set; }
    }
}

