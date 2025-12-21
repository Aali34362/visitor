using FluentValidation.Results;

namespace Visitor.Core.DesignPatterns.ResultPattern;

public class Result
{
    public bool IsSuccess { get; set; }
    public ErrorDetail Error { get; set; }

    public static Result Success() => 
        new Result { IsSuccess = true };

    public static Result Failure(ErrorDetail error) => 
        new Result { IsSuccess = false, Error = error };
}
public class Result<T> : Result
{
    public T Value { get; set; }

    public static new Result<T> Success(T value) => 
        new Result<T> { IsSuccess = true, Value = value };
    public static new Result<T> Failure(ErrorDetail error) => 
        new Result<T> { IsSuccess = false, Error = error };
}

public static class ResultExtension
{
    public static T Match<T>(this Result result, Func<T> onSuccess, Func<ErrorDetail, T> onFailure) => 
        result.IsSuccess ? onSuccess() : onFailure(result.Error);

    public static TResult Match<T, TResult>(  this Result<T> result, Func<T, TResult> onSuccess,  Func<ErrorDetail, TResult> onFailure)  => 
        result.IsSuccess ? onSuccess(result.Value!) : onFailure(result.Error!);

    public static Result<U> Map<T, U>(this Result<T> result, Func<T, U> map) => 
        result.IsSuccess ? Result<U>.Success(map(result.Value!)) : Result<U>.Failure(result.Error!);

    public static async Task<Result<U>> BindAsync<T, U>(this Result<T> result, Func<T, Task<Result<U>>> bind) => 
        result.IsSuccess ? await bind(result.Value!) : Result<U>.Failure(result.Error!);
}

public static class ValidationResultExtensions
{
    public static ErrorDetail ToErrorDetail(this ValidationResult result)
    {
        var errors = result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        return ErrorDetail.Domain(errors);
    }
}