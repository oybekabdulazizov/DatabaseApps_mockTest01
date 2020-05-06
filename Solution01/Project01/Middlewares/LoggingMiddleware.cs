using Microsoft.AspNetCore.Http;
using Project01.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project01.Middlewares
{
    public class LoggingMiddleware
    {
        public readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IDbService service)
        {
            if (context.Request != null)
            {
                string requester = context.Request.Headers["index"].ToString();
                string path = context.Request.Path;
                string method = context.Request.Method;
                var permissionGranted = (requester == "s17712");
                string queryString = context.Request.QueryString.ToString();
                string bodyString = context.Request.Body.ToString();

                using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyString = await reader.ReadToEndAsync();
                }
                service.SaveLogData(requester, path, permissionGranted, method, queryString, bodyString);
            }

            await _next(context);
        }
    }
}
