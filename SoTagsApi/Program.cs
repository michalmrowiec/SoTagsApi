using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using SoTagsApi.Domain.Interfaces;
using SoTagsApi.Infrastructure;
using SoTagsApi.Infrastructure.Repositories;
using SoTagsApi.Infrastructure.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var dbConnectionString = builder.Configuration.GetConnectionString("ContainerDb");

//Log.Logger = new LoggerConfiguration()
//    .WriteTo.
//    Console(LogEventLevel.Information)
//    .WriteTo
//    .MSSqlServer(
//        restrictedToMinimumLevel: LogEventLevel.Warning,
//        connectionString: dbConnectionString,
//        sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true })
//    .CreateLogger();

//builder.Services.AddLogging(loggingBuilder =>
//        loggingBuilder.AddSerilog(
//            logger: new LoggerConfiguration()
//                .WriteTo.
//                Console(LogEventLevel.Information)
//                .WriteTo
//                .MSSqlServer(
//                    restrictedToMinimumLevel: LogEventLevel.Warning,
//                    connectionString: dbConnectionString,
//                    sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true })
//                .CreateLogger(),
//            dispose: true));

var loggerConfiguration = new LoggerConfiguration()
    .WriteTo.Console(LogEventLevel.Information);

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Test")
{
    loggerConfiguration.WriteTo.MSSqlServer(
        restrictedToMinimumLevel: LogEventLevel.Warning,
        connectionString: dbConnectionString,
        sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true });
}

Log.Logger = loggerConfiguration.CreateLogger();

builder.Services.AddLogging(loggingBuilder =>
        loggingBuilder.AddSerilog(logger: Log.Logger, dispose: true));


builder.Services.AddHttpClient();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(dbConnectionString));

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITagsRepository, TagsRepository>();
builder.Services.AddScoped<ISoTagService, SoTagService>();

var app = builder.Build();

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Test")
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var pendingMigrations = dbContext.Database.GetPendingMigrations();
    if (pendingMigrations.Any())
    {
        dbContext.Database.Migrate();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }