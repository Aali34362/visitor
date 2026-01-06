namespace Visitor.Module.IAM.Application.DecoratorServices.ValidatorServices;

public class CreateUserRoleCommandValidator : AbstractValidator<CreateUserRoleMappingCommand>
{
    private readonly IIdentityUserRoleBusinessService _businessService;
    private readonly IIdentityUserBusinessService _userBusinessService;
    private readonly IIdentityRoleBusinessService _rolebusinessService;

    public CreateUserRoleCommandValidator(
        IIdentityUserBusinessService userBusinessService,
        IIdentityRoleBusinessService rolebusinessService,
        IIdentityUserRoleBusinessService businessService)
    {
        _businessService = businessService;
        _userBusinessService = userBusinessService;
        _rolebusinessService = rolebusinessService;


        RuleFor(x => x.User_Nm)
            .NotEmpty().WithMessage("User name is required.")
            .MustAsync(async (userName, ct) =>
            {
                var user = await _userBusinessService.GetByNameAsync(userName);
                return user is not null;
            }).WithMessage(x => $"Policy '{x.User_Nm}' does not exist.");

        RuleFor(x => x.Role_Nm)
            .NotEmpty().WithMessage("Role name is required.")
            .MustAsync(async (roleName, ct) =>
            {
                var pageAction = await _rolebusinessService.GetByNameAsync(roleName);
                return pageAction is not null;
            }).WithMessage(x => $"PageAction '{x.Role_Nm}' does not exist.");

        RuleFor(x => x)
            .MustAsync(async (command, ct) =>
            {
                var user = await _userBusinessService.GetByNameAsync(command.User_Nm);
                var role = await _rolebusinessService.GetByNameAsync(command.Role_Nm);

                if (user is null || role is null)
                    return true; 

                var exists = await _businessService.IsUserRoleMappingExistsAsync(user.id, role.id);
                return !exists;
            }).WithMessage(x => $"Mapping for '{x.User_Nm}' and '{x.Role_Nm}' already exists.");
    }
}

public class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleMappingCommand>
{
    private readonly IIdentityUserRoleBusinessService _businessService;
    private readonly IIdentityUserBusinessService _userBusinessService;
    private readonly IIdentityRoleBusinessService _rolebusinessService;

    public UpdateUserRoleCommandValidator(
        IIdentityUserBusinessService userBusinessService,
        IIdentityRoleBusinessService rolebusinessService,
        IIdentityUserRoleBusinessService businessService)
    {
        _businessService = businessService;
        _userBusinessService = userBusinessService;
        _rolebusinessService = rolebusinessService;


        RuleFor(x => x.User_Nm)
            .NotEmpty().WithMessage("User name is required.")
            .MustAsync(async (userName, ct) =>
            {
                var user = await _userBusinessService.GetByNameAsync(userName);
                return user is not null;
            }).WithMessage(x => $"Policy '{x.User_Nm}' does not exist.");

        RuleFor(x => x.Role_Nm)
            .NotEmpty().WithMessage("Role name is required.")
            .MustAsync(async (roleName, ct) =>
            {
                var pageAction = await _rolebusinessService.GetByNameAsync(roleName);
                return pageAction is not null;
            }).WithMessage(x => $"PageAction '{x.Role_Nm}' does not exist.");

        RuleFor(x => x)
            .MustAsync(async (command, ct) =>
            {
                var user = await _userBusinessService.GetByNameAsync(command.User_Nm);
                var role = await _rolebusinessService.GetByNameAsync(command.Role_Nm);

                if (user is null || role is null)
                    return true;

                var exists = await _businessService.IsUserRoleMappingExistsAsync(user.id, role.id);
                return !exists;
            }).WithMessage(x => $"Mapping for '{x.User_Nm}' and '{x.Role_Nm}' already exists.");
    }
}