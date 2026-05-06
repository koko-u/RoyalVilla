using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RoyalVilla.Api.Annotations;
using RoyalVilla.Api.Dto;

namespace RoyalVilla.Api.Repositories;

/// <summary>
/// Users operations repository
/// </summary>
/// <param name="dataSource"></param>
/// <param name="logger"></param>
[AutoRegisterService]
public sealed class UsersRepository(NpgsqlDataSource dataSource, ILogger<UsersRepository> logger) : IRepository
{
    /// <summary>
    /// Create new User and assign whom roles
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<UserData> CreateUserAsync(CreateUserDto dto, CancellationToken cancellationToken)
    {
        var insertSql = await File.ReadAllTextAsync("Sql/Users/insert_one.sql", cancellationToken);
        var assignSql = await File.ReadAllTextAsync("Sql/UserRoles/assign_roles.sql", cancellationToken);
        var sql = await File.ReadAllTextAsync("Sql/Users/select_with_roles.sql", cancellationToken);

        await using var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var tx = await conn.BeginTransactionAsync(cancellationToken);
        try
        {
            var insertCmd = new CommandDefinition(
                commandText: insertSql,
                parameters: new
                {
                    dto.Email,
                    dto.PasswordHash,
                    dto.DisplayName,
                },
                cancellationToken: cancellationToken,
                transaction: tx
            );
            var userRow = await conn.QuerySingleAsync<UserRow>(insertCmd);

            var assignCmd = new CommandDefinition(
                commandText: assignSql,
                parameters: new
                {
                    UserId = userRow.Id,
                    RoleNames = dto.Roles
                },
                cancellationToken: cancellationToken,
                transaction: tx
            );
            await conn.ExecuteAsync(assignCmd);

            var cmd = new CommandDefinition(
                commandText: sql,
                parameters: new
                {
                    userRow.Id
                },
                cancellationToken: cancellationToken,
                transaction: tx
            );
            var userAndRoleRows = await conn.QueryAsync<UserAndRoleRow>(cmd);

            var users = userAndRoleRows.GroupBy(x => x.ExtractUserRow())
                .Select(g => new UserData()
                {
                    Id = g.Key.Id,
                    DisplayName = g.Key.DisplayName,
                    Email = g.Key.Email,
                    IsActive = g.Key.IsActive,
                    Roles =
                        g.Where(x => x.RoleId.HasValue)
                            .Select(x => new RoleData
                            {
                                Id = x.RoleId.GetValueOrDefault(),
                                Name = x.RoleName ?? string.Empty
                            })
                            .ToArray(),
                }).Single();
            
            await tx.CommitAsync(cancellationToken);

            return users;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create user with email {Email}", dto.Email);
            await tx.RollbackAsync(cancellationToken);
            throw;
        }
    }
}