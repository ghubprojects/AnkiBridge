namespace LexiBridge.Shared.Results;

/// <summary>
/// Represents a result of an operation with success/failure indication
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public Error Error { get; }
    public ErrorType ErrorType { get; }
    public IReadOnlyList<ValidationError> ValidationErrors { get; } = [];

    #region Constructors

    protected Result(
        bool isSuccess,
        Error error,
        ErrorType errorType,
        IReadOnlyList<ValidationError>? validationErrors)
    {
        if (isSuccess && !error.IsNone)
            throw new InvalidOperationException("Success result cannot have an error.");

        if (!isSuccess && error.IsNone && errorType != ErrorType.Validation)
            throw new InvalidOperationException("Failure result must have an error.");

        IsSuccess = isSuccess;
        Error = error;
        ErrorType = errorType;
        ValidationErrors = validationErrors ?? [];
    }

    #endregion

    #region Factory Methods

    public static Result Success()
        => new(true, Error.None, ErrorType.None, []);

    public static Result Failure(Error error, ErrorType type = ErrorType.Failure)
    {
        if (error.IsNone)
            throw new ArgumentException("Failure must have a valid error.", nameof(error));

        return new(false, error, type, []);
    }

    public static Result Failure(string message, ErrorType type = ErrorType.Failure)
       => Failure(new Error(message), type);

    public static Result ValidationFailure(IEnumerable<ValidationError> errors)
    {
        var list = errors?.ToList() ?? [];

        if (list.Count == 0)
            throw new ArgumentException("Validation errors cannot be empty.", nameof(errors));

        return new(false, Error.None, ErrorType.Validation, list);
    }

    #endregion

    #region Generic Factory Methods

    public static Result<T> Success<T>(T value)
      => Result<T>.Success(value);

    public static Result<T> Failure<T>(Error error, ErrorType type = ErrorType.Failure)
        => Result<T>.Failure(error, type);

    public static Result<T> Failure<T>(string errorMessage, ErrorType type = ErrorType.Failure)
        => Result<T>.Failure(errorMessage, type);

    public static Result<T> ValidationFailure<T>(IEnumerable<ValidationError> errors)
        => Result<T>.ValidationFailure([.. errors]);

    #endregion

    #region Match Methods

    public void Match(Action onSuccess, Action<Error> onFailure)
    {
        if (IsSuccess)
            onSuccess();
        else
            onFailure(Error);
    }

    public async Task MatchAsync(Func<Task> onSuccess, Func<Error, Task> onFailure)
    {
        if (IsSuccess)
            await onSuccess();
        else
            await onFailure(Error);
    }

    #endregion

    #region Tap Methods

    public Result Tap(Action action)
    {
        if (IsSuccess)
            action();

        return this;
    }

    public Result TapError(Action<Error> action)
    {
        if (IsFailure)
            action(Error);

        return this;
    }

    #endregion

    public Result<TOut> ToFailure<TOut>()
    {
        if (IsSuccess)
            throw new InvalidOperationException("Cannot convert Success to Failure.");

        return Result<TOut>.Failure(Error, ErrorType);
    }
}