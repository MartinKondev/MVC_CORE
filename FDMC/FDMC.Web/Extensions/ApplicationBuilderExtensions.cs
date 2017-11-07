using FDMC.Web.Handlers;
using FDMC.Web.Middleware;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FDMC.Web.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHttpResponseSetup(this IApplicationBuilder builder)
            => builder.UseMiddleware<ConfigureHttpResponseMiddleware>();

        public static IApplicationBuilder UseRequestHandlers(this IApplicationBuilder builder)
        {
            var handlers = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && typeof(IHandler).IsAssignableFrom(t))
                .Select(Activator.CreateInstance)
                .Cast<IHandler>()
                .OrderBy(h => h.Order);

            foreach (var handler in handlers)
            {
                builder.MapWhen(handler.Condition, app =>
                {
                    app.Run(handler.RequestHandler);
                });
            }

            return builder;
        }
    }
}
