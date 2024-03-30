using Castle.Core.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Serilog.Events;
using Serilog;
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
                var serviceDescriptors = services
                                .Where(descriptor => descriptor.ServiceType == typeof(ILoggingBuilder))
                                .ToList();

                foreach (var descriptor in serviceDescriptors)
                {
                    services.Remove(descriptor);
                }

                services.AddLogging(loggingBuilder =>
               loggingBuilder.AddSerilog(
                   logger: new LoggerConfiguration()
                       .WriteTo.Console(LogEventLevel.Information)
                       .CreateLogger(),
                   dispose: true));


                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ApplicationDbContext>));

                services.Remove(dbContextDescriptor);

                var dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbConnection));

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
