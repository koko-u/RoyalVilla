using FluentValidation;

namespace RoyalVilla.Api.Features.Villas.RequestData;

/// <summary>
/// pagination query parameter validator
/// </summary>
public sealed class PageQueryValidator : AbstractValidator<PageQuery>
{
    /// <summary>
    /// constructor
    /// </summary>
    public PageQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0).WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("PageSize must be greater than 0")
            .LessThanOrEqualTo(1000)
            .WithMessage("PageSize must be less than or equal to 1000");
    }
}
