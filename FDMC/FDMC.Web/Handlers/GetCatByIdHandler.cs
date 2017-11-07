using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using FDMC.Web.DAL;
using Microsoft.Extensions.DependencyInjection;

namespace FDMC.Web.Handlers
{
    public class GetCatByIdHandler : IHandler
    {
        const string catGetByIdUrlRegex = @"\/cats\/\d+";

        public int Order => 4;

        public Func<HttpContext, bool> Condition => (context) => Regex.IsMatch(context.Request.Path.Value, catGetByIdUrlRegex);

        public RequestDelegate RequestHandler => async (context) =>
        {
            var catId = Convert.ToInt32(context.Request.Path.Value.Split("/").Last());
            var db = context.RequestServices.GetRequiredService<CatsDbContext>();

            var cat = await db.Cats.FindAsync(catId);
            if (cat != null)
            {
                await context.Response.WriteAsync($"<h1>{cat.Name}</h1> </br>");
                await context.Response.WriteAsync($@"<img src=""{cat.ImageUrl}"" alt=""{cat.Name}""'s photo style=""max-width:300px;""> </br>");
                await context.Response.WriteAsync($"Age: {cat.Age} </br>");
                await context.Response.WriteAsync($"Breed: {cat.Breed} </br>");
            }
        };
    }
}
