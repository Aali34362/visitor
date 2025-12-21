namespace Visitor.Module.IAM.Application.DecoratorServices.ValidatorServices;

public class CreatePolicyCommandValidator : AbstractValidator<CreatePolicyCommand>
{
    public CreatePolicyCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Policy name is required");

        RuleFor(x => x.Tags)
            .NotNull().WithMessage("Tags must not be null")
            .Must(t => t.Count > 0).WithMessage("At least one tag is required");
    }
}

public class UpdatePolicyCommandValidator : AbstractValidator<UpdatePolicyCommand>
{
    public UpdatePolicyCommandValidator()
    {
        RuleFor(x => x.Tags)
            .NotNull().WithMessage("Tags must not be null")
            .Must(t => t.Count > 0).WithMessage("At least one tag is required");
    }
}