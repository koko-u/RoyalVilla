using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.MakerInterfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RoyalVilla.Api.Annotations;
using RoyalVilla.Api.Features.Auth.RequestData;
using RoyalVilla.Api.Features.Auth.ResponseData;
using RoyalVilla.Api.Features.Auth.RowData;

namespace RoyalVilla.Api.Features.Auth;

/// <summary>
/// Users operations repository
/// </summary>
/// <param name="dataSource"></param>
/// <param name="logger"></param>
[AutoRegisterService]
public sealed class UsersRepository(NpgsqlDataSource dataSource, ILogger<UsersRepository> logger)
    : IRepository
{
    /// <summary>
    /// Create new User and assign whom roles
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<UserData> CreateUserAsync(
        CreateUserDto dto,
        CancellationToken cancellationToken
    )
    {
        var insertSql = await File.ReadAllTextAsync("Sql/Users/insert_one.sql", cancellationToken);
        var assignSql = await File.ReadAllTextAsync(
            "Sql/UserRoles/assign_roles.sql",
            cancellationToken
        );
        var sql = await File.ReadAllTextAsync(
            "Sql/Users/select_by_id_with_roles.sql",
            cancellationToken
        );

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
                parameters: new { UserId = userRow.Id, RoleNames = dto.Roles },
                cancellationToken: cancellationToken,
                transaction: tx
            );
            await conn.ExecuteAsync(assignCmd);

            var cmd = new CommandDefinition(
                commandText: sql,
                parameters: new { userRow.Id },
                cancellationToken: cancellationToken,
                transaction: tx
            );
            var userAndRoleRows = await conn.QueryAsync<UserAndRoleRow>(cmd);

            var users = userAndRoleRows.GroupedByRole().Single();

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

    /// <summary>
    /// Get user by id
    /// </summary>
    /// <param name="email"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<UserData?> GetUserByEmailAsync(
        string email,
        CancellationToken cancellationToken
    )
    {
        var sql = await File.ReadAllTextAsync(
            "Sql/Users/select_by_email_with_roles.sql",
            cancellationToken
        );
        await using var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        var cmd = new CommandDefinition(
            commandText: sql,
            parameters: new { Email = email },
            cancellationToken: cancellationToken
        );
        var rows = await conn.QueryAsync<UserAndRoleRow>(cmd);

        return rows.GroupedByRole().SingleOrDefault();
    }
}
