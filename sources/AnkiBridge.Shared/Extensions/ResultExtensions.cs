using AnkiBridge.Shared.Results;

namespace AnkiBridge.Shared.Extensions;

public static class ResultExtensions
{
    // async success, async error
    public static async Task Match<T>(
        this Task<Result<T>> resultTask,
        Func<T, Task> onSuccess,
        Func<Error, Task> onFailure)
    {
        var result = await resultTask;
        await (result.IsSuccess 
            ? onSuccess(result.Value) 
            : onFailure(result.Error));
    }

    // async success, sync error
    public static async Task Match<T>(
        this Task<Result<T>> resultTask,
        Func<T, Task> onSuccess,
        Action<Error> onFailure)
    {
        var result = await resultTask;
        if (result.IsSuccess)
            await onSuccess(result.Value);
        else
            onFailure(result.Error);
    }

    // sync → trả về TOut
    public static async Task<TOut> Match<T, TOut>(
        this Task<Result<T>> resultTask,
        Func<T, TOut> onSuccess,
        Func<Error, TOut> onFailure)
    {
        var result = await resultTask;
        return result.IsSuccess 
            ? onSuccess(result.Value) 
            : onFailure(result.Error);
    }

    public static async Task Match(
        this Task<Result> resultTask,
        Func<Task> onSuccess,
        Func<Error, Task> onFailure)
    {
        var result = await resultTask;
        await (result.IsSuccess
            ? onSuccess()
            : onFailure(result.Error));
    }
}