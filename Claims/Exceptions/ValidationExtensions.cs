using FluentValidation.Results;

namespace Claims.API.Exceptions;

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