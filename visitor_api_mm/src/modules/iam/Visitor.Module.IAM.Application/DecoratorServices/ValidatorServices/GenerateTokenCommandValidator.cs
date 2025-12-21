namespace Visitor.Module.IAM.Application.DecoratorServices.ValidatorServices;

public class GenerateTokenCommandValidator : AbstractValidator<GenerateTokenCommand>
{
    public GenerateTokenCommandValidator()
    {
        RuleFor(p => p.GrantType).NotEmpty().
           WithMessage("Grant Type is required");
        RuleFor(p => p.ClientId).NotEmpty().
          WithMessage("Client Id is required");
        RuleFor(p => p.ClientSecret).NotEmpty().
          WithMessage("Client Secret is required");
    }
}
