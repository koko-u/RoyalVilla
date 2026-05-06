using System;
using System.Globalization;
using Easy_Password_Validator;
using Easy_Password_Validator.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoyalVilla.Api.Dto;
using RoyalVilla.Api.Extensions;
using RoyalVilla.Api.OpenApiConfiguration;
using RoyalVilla.Api.Services.Auth;
using RoyalVilla.Api.Services.Startup;
using RoyalVilla.Api.Settings;
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

    // Add JwtOptions Configuration class Instance
    builder.Services.AddOptions<JwtOptions>()
        .BindConfiguration("Jwt")
        .ValidateDataAnnotations()
        .ValidateOnStart();

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
    // Add Jwt Bearer Authentication
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opts =>
        {
            var jwtOptions =
                builder.Configuration
                    .GetSection("Jwt")
                    .Get<JwtOptions>() ?? throw new InvalidOperationException("Jwt options are missing.");
            opts.TokenValidationParameters = jwtOptions.GenerateValidationParameters();
        });
    builder.Services.AddPolicyBasedAuthorization();
    // Password complexity
    builder.Services.AddTransient(_ => new PasswordValidatorService(new PasswordRequirements()));
    // Password Hasher
    builder.Services.AddTransient<CustomPasswordHasher>();

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

    app.UseAuthentication();
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