using Microsoft.EntityFrameworkCore;
using SoTagsApi.Infrastructure;

namespace SoTagsApi.Tests.UnitTests.Infrastructure.Helper
{
    public class TestApplicationDbContextInMemoryFactory
    {
        public static TestApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new TestApplicationDbContext(options);
        }
    }
}
