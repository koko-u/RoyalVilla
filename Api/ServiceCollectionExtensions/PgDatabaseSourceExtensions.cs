using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace RoyalVilla.Api.ServiceCollectionExtensions;

/// <summary>
/// Extension methods for configuring PostgreSQL database source in the application.
/// </summary>
public static class PgDatabaseSourceExtensions
{
    /// <summary>
    /// Add Npgsql Database Source Dependency
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddPgDatabaseSource(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var builder = new NpgsqlConnectionStringBuilder()
        {
            Host = configuration.GetValue<string>("DATABASE_HOST"),
            Port = configuration.GetValue<int>("DATABASE_PORT"),
            Database = configuration.GetValue<string>("DATABASE_NAME"),
            Username = configuration.GetValue<string>("DATABASE_USER"),
            Password = configuration.GetValue<string>("DATABASE_PASSWORD"),
            SslMode = configuration.GetValue<SslMode>("DATABASE_SSLMODE"),
        };
        services.AddNpgsqlDataSource(builder.ConnectionString);

        return services;
    }
}
