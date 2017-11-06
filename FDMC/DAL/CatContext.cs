using Microsoft.EntityFrameworkCore;
using System;

namespace DAL
{
    public class CatContext : DbContext
    {
        public DbSet<Cat> Cats { get; set; }

        public CatContext(DbContextOptions<CatContext> options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("Data Source=Cats.db");
            base.OnConfiguring(optionsBuilder);
        }
    }
}

