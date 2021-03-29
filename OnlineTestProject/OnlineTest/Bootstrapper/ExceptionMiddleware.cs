using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using OnlineTest.Base;
using OnlineTest.Base.Exceptions;
using OnlineTest.Base.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OnlineTest.Bootstrapper
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate request, IWebHostEnvironment env)
        {
            _next = request;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            string message = MessagesDictionary.ServerError;
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

            try
            {
                await _next(httpContext);
            }
            catch (AppException ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                httpContext.Response.ContentType = "application/json";
                if (ex.Message == MessagesDictionary.ValidationErrorMessage)
                {
                    var result = new ApiResult(false, ex.Message) 
                    {
                         Exceptions = ex.ValidationExceptions
                    };
                    await httpContext.Response.WriteAsync(result.Serialize());
                }
                else
                {
                    var result = new ApiResult(false, ex.Message);
                    await httpContext.Response.WriteAsync(result.Serialize());
                }
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                {
                    var dic = new Dictionary<string, string>();
                    dic.Add("Exception", ex.Message);
                    dic.Add("StackTrace", ex.StackTrace);
                    if (ex.InnerException != null)
                        dic.Add("InnerException", ex.InnerException.Serialize());
                    message = dic.Serialize();
                }

                httpContext.Response.StatusCode = (int)httpStatusCode;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(new ApiResult(false, message).Serialize());
            }
        }
    }
}
