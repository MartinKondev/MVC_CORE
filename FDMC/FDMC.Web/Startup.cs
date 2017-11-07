using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FDMC.Web.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace FDMC.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CatsDbContext>(opt => opt.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CatsDb;Trusted_Connection=True;MultipleActiveResultSets=true"));
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }

            app.UseStaticFiles();

            app.Use((ctx, next) =>
            {
                ctx.Response.Headers.Add("Content-Type", "text/html");
                return next();
            });

            app.MapWhen(
                ctx => ctx.Request.Path.Value == "/"
                       && ctx.Request.Method == "GET",
                h =>
                {
                    h.Run(async (context) =>
                    {
                        var db = context.RequestServices.GetService<CatsDbContext>();
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

                        await context.Response.WriteAsync($@"<a href=""{context.Request.Host.Value}/cats/add"">Add Cat</a>");

                    });
                });

            app.MapWhen(
                ctx => ctx.Request.Path.Value == "/cats/add"
                        && ctx.Request.Method == "GET",
                addCat =>
                {
                    addCat.Run( (context) =>
                    {
                        context.Response.StatusCode = 302;
                        context.Response.Headers.Add("Locaton", "/cats-add-form.html");

                        //context.Response.WriteAsync(@"<form action=""cats/add"" method=""POST"">");

                        //context.Response.WriteAsync("<h1>Add Cat</h1>");

                        //context.Response.WriteAsync(@"Name: <input type=""text"" name=""Name"">");
                        //context.Response.WriteAsync(@"Age: <input type=""text"" name=""Age"">");
                        //context.Response.WriteAsync(@"Breed: <input type=""text"" name=""Breed"">");
                        //context.Response.WriteAsync(@"Image Url: <input type=""text"" name=""ImageUrl"">");

                        //context.Response.WriteAsync(@"<input type=""submit"" value=""Add Cat"">");

                        return Task.CompletedTask;
                    });
                });

            app.MapWhen(
                ctx => ctx.Request.Path.Value == "/cats/add"
                    && ctx.Request.Method == "POST",
                postCat =>
                {
                    postCat.Run(async (context) =>
                    {
                        var db = context.RequestServices.GetService<CatsDbContext>();
                        var catToBeAdded = new Cat
                        {
                            Name = context.Request.Form["Name"],
                            Age = Convert.ToInt32(context.Request.Form["Age"]),
                            Breed = context.Request.Form["Breed"],
                            ImageUrl = context.Request.Form["Image Url"]
                        };
                        db.Add(catToBeAdded);
                    });
                });

            app.Run(async (context) =>
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("404 Ashkolsum!");
            });
        }
    }
}
