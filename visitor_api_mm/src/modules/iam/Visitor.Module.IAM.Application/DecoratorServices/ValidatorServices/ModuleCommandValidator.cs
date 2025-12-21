namespace Visitor.Module.IAM.Application.DecoratorServices.ValidatorServices;

public class CreateModuleCommandValidator : AbstractValidator<CreateModuleCommand>
{
    public CreateModuleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Module name is required");

        RuleFor(x => x.Tags)
            .NotNull().WithMessage("Tags must not be null")
            .Must(t => t.Count > 0).WithMessage("At least one tag is required");
    }
}

public class UpdateModuleCommandValidator : AbstractValidator<UpdateModuleCommand>
{
    public UpdateModuleCommandValidator()
    {
        RuleFor(x => x.Tags)
            .NotNull().WithMessage("Tags must not be null")
            .Must(t => t.Count > 0).WithMessage("At least one tag is required");
    }
}