using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FDMC.Web.DAL;
using Microsoft.Extensions.DependencyInjection;

namespace FDMC.Web.Handlers
{
    public class HomeHandler : IHandler
    {
        public int Order => 1;

        public Func<HttpContext, bool> Condition =>
            (context => context.Request.Path.Value == "/" && context.Request.Method == "GET");

        public RequestDelegate RequestHandler => async (context) =>
        {
            await context.Response.WriteAsync($"<h1>Fluffy Duffy Munchkin Cats</h1>");

            var db = context.RequestServices.GetRequiredService<CatsDbContext>();
            var cats = db
                .Cats
                .Select(c => new
                {
                    c.Id,
                    c.Name
                })
                .ToList();

            await context.Response.WriteAsync("<ul>");
            foreach (var cat in cats)
            {
                await context.Response.WriteAsync($@"<li><a href=""cats/{cat.Id}"">{cat.Name}</a></li>");
            }

            await context.Response.WriteAsync("</ul>");

            await context.Response.WriteAsync(
                $@"
                <form method=""GET"" action=""/cats/add"">
                    <input type=""submit"" value = ""Add Cat""></input>
                </form>
            ");
        };
    }
}
