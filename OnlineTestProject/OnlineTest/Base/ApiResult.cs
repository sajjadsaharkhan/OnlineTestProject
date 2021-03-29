using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTest.Base
{
    public class ApiResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Exceptions { get; set; }
        public ApiResult() { }
        public ApiResult(bool _isSuccess, string _message)
        {
            this.IsSuccess = _isSuccess;
            this.Message = _message;
        }

        public static OkObjectResult Ok()
        {
            return new OkObjectResult(new ApiResult(true, MessagesDictionary.Success));
        }
    }
    public class ApiResult<TData> : ApiResult
    {
        public TData Data { get; set; }
        public ApiResult() { }
        public ApiResult(bool _isSuccess, string _message, TData _data)
        {
            this.IsSuccess = _isSuccess;
            this.Message = _message;
            this.Data = _data;
        }

        public static OkObjectResult Ok(TData data)
        {
            return new OkObjectResult(new ApiResult<TData>(true,MessagesDictionary.Success,data));
        }
    }
}
