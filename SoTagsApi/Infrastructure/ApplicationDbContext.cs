using Microsoft.EntityFrameworkCore;

namespace SoTagsApi.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Tag> Tags { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
    }
}
