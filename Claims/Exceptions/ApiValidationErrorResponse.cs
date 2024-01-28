using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Claims.API.Exceptions;

public class ApiValidationErrorResponse : ProblemDetails
{
    public List<ValidationError> ValidationErrors { get; set; }

    public ApiValidationErrorResponse()
    {
        ValidationErrors = [];
        Title = "Validation error";
        Status = (int)HttpStatusCode.BadRequest;
    }
}

public class ValidationError
{
    public string ValidationErrorCode { get; set; } = string.Empty;
    public string ValidationErrorMessage { get; set; } = string.Empty;
}