using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Npgsql;
using RoyalVilla.Api.Annotations;
using RoyalVilla.Api.Dto;
using RoyalVilla.Api.Services.Auth;

namespace RoyalVilla.Api.Validators;

/// <summary>
/// Login credential validator
/// </summary>
[AutoRegisterService]
public sealed class LoginValidator : AbstractValidator<LoginDto>
{
    private readonly NpgsqlDataSource _dataSource;
    private readonly CustomPasswordHasher _passwordHasher;

    /// <summary>
    /// Default constructor for LoginValidator
    /// </summary>
    /// <param name="dataSource">Npgsql data source for database operations</param>
    /// <param name="passwordHasher">Custom password hasher for password validation</param>
    public LoginValidator(NpgsqlDataSource dataSource, CustomPasswordHasher passwordHasher)
    {
        _dataSource = dataSource;
        _passwordHasher = passwordHasher;

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(ExistsUserByEmail)
            .WithMessage("User with provided email and password is invalid");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MustAsync(IsValidPassword)
            .WithMessage("User with provided email and password is invalid");
    }

    private async Task<bool> ExistsUserByEmail(string? email, CancellationToken cancellationToken)
    {
        if (email is null)
        {
            // skip null email check
            return true;
        }

        await using var conn = await _dataSource.OpenConnectionAsync(cancellationToken);
        var cmd = new CommandDefinition(
            commandText: """
            SELECT EXISTS (
                SELECT 1
                FROM "users"
                WHERE "email" = @Email
            )
            """,
            parameters: new { Email = email },
            cancellationToken: cancellationToken
        );

        var exists = await conn.ExecuteScalarAsync<bool>(cmd);

        return exists;
    }

    private async Task<bool> IsValidPassword(
        LoginDto dto,
        string? password,
        CancellationToken cancellationToken
    )
    {
        if (password is null)
        {
            // skip null password check
            return true;
        }

        await using var conn = await _dataSource.OpenConnectionAsync(cancellationToken);
        var cmd = new CommandDefinition(
            commandText: """
            SELECT "password"
            FROM "users"
            WHERE "email" = @Email
            """,
            parameters: new { dto.Email },
            cancellationToken: cancellationToken
        );

        var hashedPassword = await conn.QuerySingleOrDefaultAsync<string>(cmd);
        if (hashedPassword is null)
        {
            // no records of Email
            return false;
        }

        var result = _passwordHasher.VerifyHashedPassword(hashedPassword, password);

        return result == PasswordVerificationResult.Success;
    }
}
