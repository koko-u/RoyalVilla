using Common.Types;
using Microsoft.AspNetCore.Identity;

namespace RoyalVilla.Api.Features.Shared;

/// <summary>
/// Password Hasher
/// </summary>
public sealed class CustomPasswordHasher : IPasswordHasher<Unit>
{
    private readonly IPasswordHasher<Unit> _inner = new PasswordHasher<Unit>();

    /// <summary>
    /// Hash Password
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public string HashPassword(string password) =>
        ((IPasswordHasher<Unit>)this).HashPassword(Unit.Value, password);

    /// <summary>
    /// Verify Hashed Password
    /// </summary>
    /// <param name="hashedPassword"></param>
    /// <param name="providedPassword"></param>
    /// <returns></returns>
    public PasswordVerificationResult VerifyHashedPassword(
        string hashedPassword,
        string providedPassword
    ) =>
        ((IPasswordHasher<Unit>)this).VerifyHashedPassword(
            Unit.Value,
            hashedPassword,
            providedPassword
        );

    /// <inheritdoc />
    string IPasswordHasher<Unit>.HashPassword(Unit value, string password) =>
        _inner.HashPassword(value, password);

    /// <inheritdoc />
    PasswordVerificationResult IPasswordHasher<Unit>.VerifyHashedPassword(
        Unit value,
        string hashedPassword,
        string providedPassword
    ) => _inner.VerifyHashedPassword(value, hashedPassword, providedPassword);
}
