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
using System.Net;
using FDMC.Web.Extensions;

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
            app.UseHttpResponseSetup();
            app.UseRequestHandlers();

            app.Run(async (context) =>
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("404 Ashkolsum!");
            });
        }
    }
}
