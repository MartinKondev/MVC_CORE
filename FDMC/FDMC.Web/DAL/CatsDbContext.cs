using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FDMC.Web.DAL
{
    public class CatsDbContext : DbContext
    {
        public DbSet<Cat> Cats { get; set; }

        public CatsDbContext(DbContextOptions<CatsDbContext> opts) : base(opts)
        {

        }
    }
}
