using System;
using System.Globalization;
using Easy_Password_Validator;
using Easy_Password_Validator.Models;
using FluentValidation;
using MicroElements.AspNetCore.OpenApi.FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoyalVilla.Api.Features.Shared;
using RoyalVilla.Api.Features.Startup;
using RoyalVilla.Api.OpenApiConfiguration;
using RoyalVilla.Api.ServiceCollectionExtensions;
using RoyalVilla.Api.Settings;
using Scalar.AspNetCore;
using Serilog;

// Bootstrap Logger
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

// Dapper snake_case to PascalCase property mapping configuration
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// FluentValidation culture settings
ValidatorOptions.Global.LanguageManager.Culture = CultureInfo.InvariantCulture;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Configure Logger to Serilog
    builder.Host.UseSerilog(
        (context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        }
    );

    // Add Controllers and OpenAPI configuration
    builder.Services.AddControllers();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi(opts =>
    {
        opts.AddOperationTransformer<WithoutSummaryAndDescriptionOperationTransformer>();
        opts.AddOperationTransformer<BearerSecurityOperationTransformer>();
        opts.AddDocumentTransformer<TagsDescriptionDocumentTransformer>();
        opts.AddDocumentTransformer<BearerSecurityDocumentTransformer>();

        // for fluent validation rules
        opts.AddFluentValidationRules();
    });

    // Add JwtOptions Configuration class Instance
    builder
        .Services.AddOptions<JwtOptions>()
        .BindConfiguration("Jwt")
        .ValidateDataAnnotations()
        .ValidateOnStart();

    // Add PostgreSQL Database Source
    builder.Services.AddPgDatabaseSource(builder.Configuration);
    // Add Repositories
    builder.Services.AddRepositories<Program>();
    // Add Services
    builder.Services.AddServices<Program>();
    // Add Problem Details
    builder.Services.AddProblemDetails();
    // Add Fluent Validations
    builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
    // Add Fluent Validation rules to OpenAPI
    builder.Services.AddFluentValidationRulesToOpenApi();
    // Add Jwt Bearer Authentication
    builder
        .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opts =>
        {
            var jwtOptions =
                builder.Configuration.GetSection("Jwt").Get<JwtOptions>()
                ?? throw new InvalidOperationException("Jwt options are missing.");
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
            opts.EnableDarkMode().WithTheme(ScalarTheme.BluePlanet).ShowOperationId();
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
