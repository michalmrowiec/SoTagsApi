using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SoTagsApi.Infrastructure;
using System.Data.Common;

namespace SoTagsApi.Tests.IntegrationTests
{
    public class CustomWebApplicationFactory<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ApplicationDbContext>));

                if(dbContextDescriptor != null)
                    services.Remove(dbContextDescriptor);

                var dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbConnection));

                if(dbConnectionDescriptor != null)
                    services.Remove(dbConnectionDescriptor);

                var dbName = Guid.NewGuid().ToString();
                services.AddDbContext<ApplicationDbContext>((options) =>
                {
                    options.UseInMemoryDatabase(dbName);
                });

                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: dbName)
                    .Options;

                using var context = new ApplicationDbContext(options);
                context.SaveChanges();
            });

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        }
    }
}
