namespace GoalManager.Core;

public static class ResultExtensions
{
  public static Result ToResult<T>(this Result<T> result)
  {
    switch (result.Status)
    {
      case ResultStatus.Ok:
        return result.SuccessMessage != null ? Result.SuccessWithMessage(result.SuccessMessage) : Result.Success();
      case ResultStatus.Error:
        return Result.Error(new ErrorList(result.Errors));
      case ResultStatus.Forbidden:
        return Result.Forbidden(result.Errors.ToArray());
      case ResultStatus.Unauthorized:
        return Result.Unauthorized(result.Errors.ToArray());
      case ResultStatus.Invalid:
        return Result.Invalid(result.ValidationErrors);
      case ResultStatus.NotFound:
        return Result.NotFound(result.Errors.ToArray());
      case ResultStatus.NoContent:
        return Result.NoContent();
      case ResultStatus.Conflict:
        return Result.Conflict(result.Errors.ToArray());
      case ResultStatus.CriticalError:
        return Result.CriticalError(result.Errors.ToArray());
      case ResultStatus.Unavailable:
        return Result.Unavailable(result.Errors.ToArray());
      default:
        throw new ArgumentOutOfRangeException();
    }
  }
}
