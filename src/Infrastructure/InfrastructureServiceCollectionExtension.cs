using System;
using KozLibraries.DapperSqlHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoyalVilla.Application.Features.Villas.Repositories;
using RoyalVilla.Application.Shared.Tx;
using RoyalVilla.Infrastructure.Options;
using RoyalVilla.Infrastructure.Repositories;
using RoyalVilla.Infrastructure.Tx;

namespace RoyalVilla.Infrastructure;

public static class InfrastructureServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Get Database settings
        var databaseSetting =
            configuration.GetRequiredSection("Database").Get<DatabaseOption>()
            ?? throw new InvalidOperationException("Database settings are required");

        services.AddNpgsqlDataSource(databaseSetting.ConnectionString);
        services.AddSqlResource(opts =>
        {
            opts.SqlBasePath = "sql";
            opts.Assembly = typeof(Infrastructure).Assembly;
        });

        // Repositories
        services.AddScoped<IVillasRepository, VillasRepository>();
        // Shared
        services.AddScoped<ITxRunner, TxRunner>();

        return services;
    }
}
