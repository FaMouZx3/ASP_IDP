using Microsoft.EntityFrameworkCore;
using ResourceServer.Models;

namespace ResourceServer.Data
{
    public class AnimalDbContext : DbContext
    {
        public AnimalDbContext(DbContextOptions<AnimalDbContext> options)
            : base(options)
        {
        }

        public DbSet<TbAnimal> TbAnimal { get; set; }
    }
}
