using Microsoft.EntityFrameworkCore;
using CitiesWebApplication.Models;

namespace CitiesWebApplication
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            :base(options)
        {
        }

        public DbSet<City> Cities { get; set; }
    }
}
