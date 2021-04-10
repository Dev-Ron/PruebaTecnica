using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infracstructure.DataAccess
{
    public class PTContext : DbContext
    {
        public PTContext(DbContextOptions<PTContext> options) : base(options)
        {

        }

        public DbSet<Autor> Autor { get; set; }
        public DbSet<Libro> Libro { get; set; }
    }
}

