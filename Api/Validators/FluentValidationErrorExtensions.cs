using System.Collections.Generic;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RoyalVilla.Api.Validators;

/// <summary>
/// Add ModelState errors from FluentValidation results
/// </summary>
public static class FluentValidationErrorExtensions
{
    /// <summary>
    /// Add All validation errors to ModelState
    /// </summary>
    /// <param name="modelState"></param>
    /// <param name="errors"></param>
    public static void AddFluentErrorsToModelState(this ModelStateDictionary modelState, IEnumerable<ValidationFailure> errors)
    {
        foreach (var error in errors)
        {
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }
    }
}