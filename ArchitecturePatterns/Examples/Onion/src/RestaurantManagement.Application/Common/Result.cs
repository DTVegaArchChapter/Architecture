namespace RestaurantManagement.Application.Common;

public enum ResultType
{
    Success,
    NotFound,
    Conflict,
    Failure
}

/// <summary>
/// Marker interface for Result types to enable compile-time validation behavior constraints
/// </summary>
public interface IResult
{
    bool IsSuccess { get; }
    string? ErrorMessage { get; }
    ResultType ResultType { get; }
    Dictionary<string, object> ErrorDetails { get; }
}

public sealed class Result<T> : IResult
{
    private Dictionary<string, object>? _errorDetails;
    public bool IsSuccess { get; private init; }
    public T? Data { get; private set; }
    public string? ErrorMessage { get; private init; }
    public ResultType ResultType { get; private init; }

    public Dictionary<string, object> ErrorDetails
    {
        get
        {
            return _errorDetails ??= [];
        }
    }

    private Result() { }

    public static Result<T> Success(T data)
    {
        return new Result<T>
        {
            IsSuccess = true,
            Data = data,
            ResultType = ResultType.Success
        };
    }

    public static Result<T> Failure(string errorMessage, ResultType resultType = ResultType.Failure, Dictionary<string, object>? errorDetails = null)
    {
        return new Result<T>
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            ResultType = resultType,
            _errorDetails = errorDetails
        };
    }

    public static Result<T> NotFound(string errorMessage, Dictionary<string, object>? errorDetails = null)
    {
        return Failure(errorMessage, ResultType.NotFound, errorDetails);
    }

    public static Result<T> Conflict(string errorMessage, Dictionary<string, object>? errorDetails = null)
    {
        return Failure(errorMessage, ResultType.Conflict, errorDetails);
    }

    /// <summary>
    /// Creates a validation failure result with structured error details
    /// </summary>
    /// <param name="errorMessages">List of validation error messages</param>
    /// <param name="propertyErrors">Dictionary of property-specific errors</param>
    /// <returns>A failure result with validation errors</returns>
    public static Result<T> ValidationFailure(List<string> errorMessages, Dictionary<string, string[]> propertyErrors)
    {
        var validationErrorMessage = $"Validation failed: {string.Join("; ", errorMessages)}";
        var errorDetails = propertyErrors.ToDictionary<KeyValuePair<string, string[]>, string, object>(
            kvp => kvp.Key,
            kvp => kvp.Value);

        return Failure(validationErrorMessage, ResultType.Failure, errorDetails);
    }
}