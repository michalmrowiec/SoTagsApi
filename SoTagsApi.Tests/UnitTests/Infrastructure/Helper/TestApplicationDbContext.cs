using Microsoft.EntityFrameworkCore;
using SoTagsApi.Infrastructure;

namespace SoTagsApi.Tests.UnitTests.Infrastructure.Helper
{
    public class TestApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : ApplicationDbContext(dbContextOptions)
    {
        public override void Dispose()
        {
            Database.EnsureDeleted();
            base.Dispose();
        }

        public override ValueTask DisposeAsync()
        {
            Database.EnsureDeletedAsync();
            return base.DisposeAsync();
        }
    }
}
