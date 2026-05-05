using System;
using System.Globalization;
using Common.OpenApiConfiguration;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoyalVilla.Api.Extensions;
using RoyalVilla.Api.OpenApiConfiguration;
using RoyalVilla.Api.Services.Startup;
using Scalar.AspNetCore;
using Serilog;

// Bootstrap Logger
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

// Dapper snake_case to PascalCase property mapping configuration
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// FluentValidation culture settings
ValidatorOptions.Global.LanguageManager.Culture = CultureInfo.InvariantCulture;


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
        opts.AddOperationTransformer(OperationTransformers.WithoutSummaryAndDescription);
        opts.AddDocumentTransformer(DocumentTransformers.TagDescriptions);
    });

    // Add PostgreSQL Database Source
    builder.Services.AddPgDatabaseSource(builder.Configuration);
    // Add Repositories
    builder.Services.AddRepositories(typeof(Program));
    // AutoMapper
    builder.Services.AddMappingProfiles(typeof(Program));
    // Add Problem Details
    builder.Services.AddProblemDetails();
    // Add Fluent Validations
    builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
    
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
        
        // Unhandled Exception behavior
        app.UseDeveloperExceptionPage();
    }
    else
    {
        // Unhandled Exception behavior in Production
        app.UseExceptionHandler();
        app.UseHsts();
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