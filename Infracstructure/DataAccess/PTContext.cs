using Microsoft.EntityFrameworkCore;

namespace Infracstructure.DataAccess
{
    public class PTContext : DbContext
    {
        public PTContext(DbContextOptions<PTContext> options) : base(options)
        {

        }
    }
}

