using System.Threading.Tasks;
using Dapper;
using Easy_Password_Validator;
using FluentValidation;
using Npgsql;
using RoyalVilla.Api.Dto;

namespace RoyalVilla.Api.Validators;

/// <summary>
/// Validator for user registration data
/// </summary>
public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
{
    /// <summary>
    /// Default constructor for RegisterUserValidator
    /// </summary>
    public RegisterUserValidator(PasswordValidatorService passwordValidator, NpgsqlDataSource dataSource)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(255)
            .EmailAddress();
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .Must((rootObj, password) => passwordValidator.TestAndScore(password, [rootObj.Email]));

        RuleFor(x => x.PasswordConfirm)
            .NotEmpty()
            .Equal(x => x.Password);
        
        RuleFor(x => x.DisplayName).MaximumLength(255);

        RuleFor(x => x.Roles)
            .MustAsync(async (roles, cancellationToken) =>
            {
                var conn = await dataSource.OpenConnectionAsync(cancellationToken);
                var cmd = new CommandDefinition(
                    commandText: """
                                 SELECT EXISTS (
                                     SELECT 1
                                     FROM "roles"
                                     WHERE "name" = ANY(@roles)
                                 )
                                 """,
                    parameters: new { roles },
                    cancellationToken: cancellationToken
                );
                var existsAllRoles = await conn.ExecuteScalarAsync<bool>(cmd);

                return existsAllRoles;
            })
            .WithMessage("One or more roles are invalid");
    }    
}