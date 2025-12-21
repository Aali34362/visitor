namespace Visitor.Module.IAM.Application.DecoratorServices.ValidatorServices;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IIdentityUserBusinessService _businessService;
    public CreateUserCommandValidator(IIdentityUserBusinessService businessService)
    {
        _businessService = businessService;

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MustAsync(async (username, ct) =>
            {
                var user = await _businessService.GetByNameAsync(username);
                return user is null;
            }).WithMessage("Username is already taken.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.")
            .MustAsync(async (email, ct) =>
            {
                return !(await _businessService.emailExistsAsync(email));
            }).WithMessage("Email is already in use.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?\d{7,15}$").WithMessage("Invalid phone number format.");

        RuleFor(x => x.PasswordHash)
            .NotEmpty().WithMessage("Password is required.");
    }
}

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IIdentityUserBusinessService _businessService;

    public UpdateUserCommandValidator(IIdentityUserBusinessService businessService)
    {
        _businessService = businessService;

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.")
            .MustAsync(async (email, ct) =>
            {
                return await _businessService.emailExistsAsync(email);
            }).WithMessage("Email is already in use.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?\d{7,15}$").WithMessage("Invalid phone number format.");

        RuleFor(x => x.PasswordHash)
            .NotEmpty().WithMessage("Password is required.");
    }
}