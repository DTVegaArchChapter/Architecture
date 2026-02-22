using RestaurantManagement.Application.Common;

namespace RestaurantManagement.Api.Common;

public static class ResultHelper
{
    public static Microsoft.AspNetCore.Http.IResult ToApiResult<T>(this Result<T> result, Func<T?, Microsoft.AspNetCore.Http.IResult>? onSuccess = null)
    {
        if (result.IsSuccess)
        {
            return onSuccess?.Invoke(result.Data) ?? Results.Ok(result.Data);
        }

        var errorDetails = new
                               {
                                   error = result.ErrorMessage,
                                   errorDetails = result.ErrorDetails
                               };

        return result.ResultType switch
            {
                ResultType.NotFound => Results.NotFound(errorDetails),
                ResultType.Conflict => Results.Conflict(errorDetails),
                ResultType.Failure => Results.BadRequest(errorDetails),
                _ => Results.BadRequest(errorDetails)
            };
    }
}