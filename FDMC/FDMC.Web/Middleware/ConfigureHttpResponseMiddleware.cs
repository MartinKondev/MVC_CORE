using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FDMC.Web.Middleware
{
    public class ConfigureHttpResponseMiddleware
    {
        private readonly RequestDelegate next;

        public ConfigureHttpResponseMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext context)
        {
            context.Response.Headers.Add("Content-Type", "text/html");
            return next(context);
        }
    }
}
