namespace LexiBridge.Shared.Results;

/// <summary>
/// Represents a result of an operation with a value
/// </summary>
public class Result<T> : Result
{
    private readonly T? _value;

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access value of a failure result.");

    #region Constructors

    private Result(
        bool isSuccess,
        T? value,
        Error error,
        ErrorType errorType,
        IReadOnlyList<ValidationError>? validationErrors)
        : base(isSuccess, error, errorType, validationErrors)
    {
        _value = value;
    }

    #endregion

    #region Factory Methods

    public static Result<T> Success(T value)
        => new(true, value, Error.None, ErrorType.None, []);

    public static new Result<T> Failure(Error error, ErrorType type = ErrorType.Failure)
    {
        if (error.IsNone)
            throw new ArgumentException("Failure must have a valid error.", nameof(error));

        return new(false, default, error, type, []);
    }

    public static new Result<T> Failure(string message, ErrorType type = ErrorType.Failure)
        => Failure(new Error(message), type);

    public static new Result<T> ValidationFailure(IEnumerable<ValidationError> errors)
    {
        var list = errors?.ToList() ?? [];

        if (list.Count == 0)
            throw new ArgumentException("Validation errors cannot be empty.", nameof(errors));

        return new(false, default, Error.None, ErrorType.Validation, list);
    }

    #endregion

    #region Match Methods

    public TResult Match<TResult>(
    Func<T, TResult> onSuccess,
        Func<Error, TResult> onFailure)
    {
        return IsSuccess
            ? onSuccess(Value)
            : onFailure(Error);
    }

    public async Task<TResult> Match<TResult>(
        Func<T, Task<TResult>> onSuccess,
        Func<Error, Task<TResult>> onFailure)
    {
        return IsSuccess
            ? await onSuccess(Value)
            : await onFailure(Error);
    }

    #endregion

    #region Map Methods

    public Result<TOut> Map<TOut>(Func<T, TOut> map)
    {
        return IsSuccess
            ? Result<TOut>.Success(map(Value))
            : Result<TOut>.Failure(Error, ErrorType);
    }

    #endregion

    #region Tap Methods

    public Result<T> Tap(Action<T> action)
    {
        if (IsSuccess)
            action(Value);

        return this;
    }

    public new Result<T> TapError(Action<Error> action)
    {
        if (IsFailure)
            action(Error);

        return this;
    }

    #endregion

    #region Implicit

    public static implicit operator Result<T>(T value)
        => Success(value);

    #endregion
}