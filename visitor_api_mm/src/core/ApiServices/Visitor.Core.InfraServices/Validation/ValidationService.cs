using Visitor.Core.DesignPatterns.ResultPattern;

namespace Visitor.Core.InfraServices.Validation;

public interface IValidationService
{
    Task<Result> ValidateAsync<T>(T model);
}

public class ValidationService : IValidationService
{
    private readonly IServiceProvider _provider;

    public ValidationService(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task<Result> ValidateAsync<T>(T model)
    {
        var validator = _provider.GetRequiredService<IValidator<T>>();
        var result = await validator.ValidateAsync(model);

        return result.IsValid
            ? Result.Success()
            : Result.Failure(result.ToErrorDetail());
    }
}