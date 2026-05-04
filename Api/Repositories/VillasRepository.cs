using Microsoft.Extensions.Logging;
using Npgsql;

namespace RoyalVilla.Api.Repositories;

public sealed class VillasRepository(NpgsqlConnection conn, ILogger<VillasRepository> logger)
{
    
}