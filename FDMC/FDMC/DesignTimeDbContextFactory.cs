using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FDMC
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CatContext>
    {
        public CatContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<CatContext>();
            var cccconnectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(cccconnectionString);
            return new CatContext(builder.Options);
        }
    }
}
