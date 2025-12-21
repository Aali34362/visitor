namespace Visitor.Module.IAM.Application.DecoratorServices.ValidatorServices;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role name is required");

        RuleFor(x => x.Tags)
            .NotNull().WithMessage("Tags must not be null")
            .Must(t => t.Count > 0).WithMessage("At least one tag is required");
    }
}

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Tags)
            .NotNull().WithMessage("Tags must not be null")
            .Must(t => t.Count > 0).WithMessage("At least one tag is required");
    }
}