//using LexiBridge.Shared.Results;
//using Microsoft.AspNetCore.Mvc;

//namespace LexiBridge.ApiService.Extensions;

//public static class ResultExtensions
//{
//    public static IResult ToHttpResult<T>(this Result<T> result, Func<T?, IResult>? onSuccess = null)
//    {
//        if (result.IsSuccess)
//            return onSuccess?.Invoke(result.Value) ?? TypedResults.Ok(result.Value);

//        switch (result.ErrorType)
//        {
//            case ErrorType.Validation:
//                {
//                    var errors = result.ValidationErrors
//                        .GroupBy(e => e.Property)
//                        .ToDictionary(
//                            g => g.Key,
//                            g => g.Select(x => x.Message).ToArray());

//                    return TypedResults.BadRequest(new ValidationProblemDetails(errors)
//                    {
//                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
//                        Title = "Bad Request",
//                        Status = StatusCodes.Status400BadRequest,
//                        Detail = "One or more validation errors occurred.",
//                    });
//                }

//            case ErrorType.NotFound:
//                return TypedResults.NotFound(new ProblemDetails
//                {
//                    Type = "https://tools.ietf.org/html/rfc7807#section-6.5.4",
//                    Title = "Not Found",
//                    Status = StatusCodes.Status404NotFound,
//                    Detail = result.Error?.Message,
//                });

//            case ErrorType.Conflict:
//                return TypedResults.Conflict(new ProblemDetails
//                {
//                    Type = "https://tools.ietf.org/html/rfc7807#section-6.5.8",
//                    Title = "Conflict",
//                    Status = StatusCodes.Status409Conflict,
//                    Detail = result.Error?.Message,
//                });

//            default:
//                return TypedResults.Problem(new ProblemDetails
//                {
//                    Type = "https://tools.ietf.org/html/rfc7807#section-6.6.1",
//                    Title = "Unknown Error",
//                    Status = StatusCodes.Status500InternalServerError,
//                    Detail = result.Error?.Message ?? "An unknown error occurred.",
//                });
//        }
//    }
//}