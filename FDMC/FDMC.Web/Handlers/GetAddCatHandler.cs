using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FDMC.Web.Handlers
{
    public class GetAddCatHandler : IHandler
    {
        public int Order => 2;

        public Func<HttpContext, bool> Condition => ctx =>
            (ctx.Request.Path.Value == "/cats/add" && ctx.Request.Method == "GET");

        public RequestDelegate RequestHandler => (context) =>
        {
            context.Response.StatusCode = 302;
            context.Response.WriteAsync("<h1>Add Cat</h1>");

            context.Response.WriteAsync(@"<form action=""/cats/add"" method=""POST"">");
            context.Response.WriteAsync(@"  Name: <input type=""text"" name=""Name""> <br/>");
            context.Response.WriteAsync(@"  Age: <input type=""text"" name=""Age""> <br/>");
            context.Response.WriteAsync(@"  Breed: <input type=""text"" name=""Breed""> <br/>");
            context.Response.WriteAsync(@"  Image Url: <input type=""text"" name=""ImageUrl""> <br/>");
            context.Response.WriteAsync(@"  <input type=""submit"" value=""Add Cat""> <br/>");
            context.Response.WriteAsync(@"</form>");

            return Task.CompletedTask;
        };
    }
}
