using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using FDMC.Web.DAL;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace FDMC.Web.Handlers
{
    public class PostCatHandler : IHandler
    {
        public int Order => 3;

        public Func<HttpContext, bool> Condition =>
                (ctx => ctx.Request.Path.Value == "/cats/add"
                && ctx.Request.Method == "POST");

        public RequestDelegate RequestHandler => async (context) =>
        {
            var db = context.RequestServices.GetRequiredService<CatsDbContext>();
            var catToBeAdded = new Cat
            {
                Name = context.Request.Form["Name"],
                Age = Convert.ToInt32(context.Request.Form["Age"]),
                Breed = context.Request.Form["Breed"],
                ImageUrl = context.Request.Form["ImageUrl"]
            };
            db.Add(catToBeAdded);
            try
            {
                await db.SaveChangesAsync();
                context.Response.StatusCode = (int)HttpStatusCode.Found;
                context.Response.Redirect("/");
            }
            catch
            {
                await context.Response.WriteAsync("<h2>Invalid Cat</h2>");
                await context.Response.WriteAsync(@"<a href=""/cat/add"">Back to the form</a>");
            }
        };
    }
}
