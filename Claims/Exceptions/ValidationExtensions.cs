using FluentValidation.Results;

namespace Claims.API.Exceptions;

/// <summary>
/// Extension methods for working with validation results and creating API validation error responses.
/// </summary>
public static class ValidationExtensions
{
    public static ApiValidationErrorResponse CreateValidationErrorsResponse(this ValidationResult result)
    {
        return new ApiValidationErrorResponse
        {
            ValidationErrors = result.Errors.Select(error => new ValidationError
            {
                ValidationErrorCode = error.ErrorCode,
                ValidationErrorMessage = error.ErrorMessage,
            }).ToList()
        };
    }
}