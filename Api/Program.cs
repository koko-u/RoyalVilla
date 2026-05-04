using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoyalVilla.Api.Extensions;
using RoyalVilla.Api.Services.Startup;
using Scalar.AspNetCore;
using Serilog;

// Bootstrap Logger
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Configure Logger to Serilog
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext();
    });

    // Add Controllers and OpenAPI configuration
    builder.Services.AddControllers();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi(opts =>
    {
        opts.AddOperationTransformer( (operation, _, _) =>
        {
            operation.Summary = null;
            operation.Description = null;
            
            return Task.CompletedTask;
        });
    });
    
    // Add PostgreSQL Database Source
    builder.Services.AddPgDatabaseSource(builder.Configuration);
    
    // Register startup service
    builder.Services.AddHostedService<StartupTask>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(opts =>
        {
            opts
                .EnableDarkMode()
                .WithTheme(ScalarTheme.BluePlanet)
                .ShowOperationId();
        });
    }

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}