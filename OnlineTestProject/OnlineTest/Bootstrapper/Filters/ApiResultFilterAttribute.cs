using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineTest.Base;
using OnlineTest.Base.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTest.Bootstrapper.Filters
{
    public class ApiResultFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var apiResult = new ApiResult(false,MessagesDictionary.JsonFormatError);
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(apiResult);
            }
            base.OnResultExecuting(context);
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {

            if (context.Result == null)
            {
                var apiResult = new ApiResult(false, MessagesDictionary.ServerError);
                context.HttpContext.Response.StatusCode = 500;
                context.Result = new JsonResult(apiResult);
            }

            string s = context.Result.Serialize();

            base.OnActionExecuted(context);
        }
    }
}
