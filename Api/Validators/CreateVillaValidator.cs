using FluentValidation;
using RoyalVilla.Api.Annotations;
using RoyalVilla.Api.Dto;

namespace RoyalVilla.Api.Validators;

/// <summary>
/// Validator for creating a new villa
/// </summary>
[AutoRegisterService]
public sealed class CreateVillaValidator : AbstractValidator<CreateOrUpdateVillaDto>
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public CreateVillaValidator()
    {
        RuleFor(villa => villa.Name).NotEmpty().MaximumLength(255);
        RuleFor(villa => villa.Details).MaximumLength(4000);
        RuleFor(villa => villa.Rate).NotNull().InclusiveBetween(from: 0.0m, to: 10.0m);
        RuleFor(villa => villa.SquareFeet).NotNull().GreaterThan(0);
        RuleFor(villa => villa.Occupancy).NotNull().GreaterThan(0);
        RuleFor(villa => villa.ImageUrl).MaximumLength(255);
    }
}
