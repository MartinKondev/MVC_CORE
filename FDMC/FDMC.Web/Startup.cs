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
using System.Text.RegularExpressions;

namespace FDMC.Web
{
    public class Startup
    {
        const string catGetByIdUrlRegex = @"\/cats\/\d+";
        const string serverAddress = "Server=(localdb)\\mssqllocaldb;Database=CatsDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CatsDbContext>(
                options => 
                options.UseSqlServer(serverAddress));
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();

            app.Use((context, next) =>
            {
                context.Response.Headers.Add("Content-Type", "text/html");

                var a = Regex.IsMatch(context.Request.Path.Value, catGetByIdUrlRegex);

                return next();
            });

            //GET
            //Index Page
            app.MapWhen(
                ctx => ctx.Request.Path.Value == "/" && ctx.Request.Method == "GET",
                home =>
                {

                    home.Run(async (context) =>
                    {
                        await context.Response.WriteAsync($"<h1>Fluffy Duffy Munchkin Cats</h1>");
                        
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

                        await context.Response.WriteAsync(
                            $@"
                            <form method=""GET"" action=""/cats/add"">
                                <input type=""submit"" value = ""Add Cat""></input>
                            </form>
                            ");

                    });
                });

            //GET
            //Cat Add Page
            app.MapWhen(
                ctx => ctx.Request.Path.Value == "/cats/add"
                        && ctx.Request.Method == "GET",
                addCat =>
                {
                    addCat.Run((context) =>
                    {
                        context.Response.StatusCode = 302;
                        context.Response.Headers.Add("Locaton", "/cats-add-form.html");

                        context.Response.WriteAsync("<h1>Add Cat</h1>");

                        context.Response.WriteAsync(@"<form action=""/cats/add"" method=""POST"">");
                        context.Response.WriteAsync(@"  Name: <input type=""text"" name=""Name""> <br/>");
                        context.Response.WriteAsync(@"  Age: <input type=""text"" name=""Age""> <br/>");
                        context.Response.WriteAsync(@"  Breed: <input type=""text"" name=""Breed""> <br/>");
                        context.Response.WriteAsync(@"  Image Url: <input type=""text"" name=""ImageUrl""> <br/>");
                        context.Response.WriteAsync(@"  <input type=""submit"" value=""Add Cat""> <br/>");
                        context.Response.WriteAsync(@"</form>");

                        return Task.CompletedTask;
                    });
                });

            //POST
            //Cats Add Page
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
                            ImageUrl = context.Request.Form["ImageUrl"]
                        };
                        db.Add(catToBeAdded);
                        try
                        {
                            await db.SaveChangesAsync();
                        }
                        catch
                        {
                            await context.Response.WriteAsync("<h2>Invalid Cat</h2>");
                            await context.Response.WriteAsync(@"<a href=""/cat/add"">Back to the form</a>");
                        }
                    });
                });

            //GET
            //Cat Get by Id
            app.MapWhen(
                context => Regex.IsMatch(context.Request.Path.Value, catGetByIdUrlRegex),
                    getCat =>
                    {
                        getCat.Run(async(context) =>
                        {
                            var catId = Convert.ToInt32(context.Request.Path.Value.Split("/").Last());
                            var db = context.RequestServices.GetService<CatsDbContext>();
                            var cat = await db.Cats.FindAsync(catId);
                            if (cat != null)
                            {
                                await context.Response.WriteAsync($"<h1>{cat.Name}</h1> </br>");
                                await context.Response.WriteAsync($@"<img src=""{cat.ImageUrl}"" alt=""{cat.Name}""'s photo> </br>");
                                await context.Response.WriteAsync($"Age: {cat.Age} </br>");
                                await context.Response.WriteAsync($"Breed: {cat.Breed} </br>");
                            }
                        });
                    }
                );

            app.Run(async (context) =>
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("404 Ashkolsum!");
            });
        }
    }
}
