namespace Visitor.Module.IAM.Application.DecoratorServices.ValidatorServices;

public class CreateRolePolicyCommandValidator : AbstractValidator<CreateRolePolicyMappingCommand>
{
    private readonly IIdentityRolePolicyBusinessService _businessService;
    private readonly IIdentityPolicyBusinessService _policybusinessService;
    private readonly IIdentityRoleBusinessService _rolebusinessService;

    public CreateRolePolicyCommandValidator(
        IIdentityPolicyBusinessService policybusinessService,
        IIdentityRoleBusinessService rolebusinessService,
        IIdentityRolePolicyBusinessService businessService)
    {
        _businessService = businessService;
        _policybusinessService = policybusinessService;
        _rolebusinessService = rolebusinessService;


        RuleFor(x => x.Policy_Nm)
            .NotEmpty().WithMessage("Policy name is required.")
            .MustAsync(async (policyName, ct) =>
            {
                var policy = await _policybusinessService.GetByNameAsync(policyName);
                return policy is not null;
            }).WithMessage(x => $"Policy '{x.Policy_Nm}' does not exist.");

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
                var policy = await _policybusinessService.GetByNameAsync(command.Policy_Nm);
                var role = await _rolebusinessService.GetByNameAsync(command.Role_Nm);

                if (policy is null || role is null)
                    return true; 

                var exists = await _businessService.IsRolePolicyMappingExistsAsync(policy.Id, role.Id);
                return !exists;
            }).WithMessage(x => $"Mapping for '{x.Policy_Nm}' and '{x.Role_Nm}' already exists.");
    }
}

public class UpdateRolePolicyCommandValidator : AbstractValidator<UpdateRolePolicyMappingCommand>
{
    private readonly IIdentityRolePolicyBusinessService _businessService;
    private readonly IIdentityPolicyBusinessService _policybusinessService;
    private readonly IIdentityRoleBusinessService _rolebusinessService;

    public UpdateRolePolicyCommandValidator(
        IIdentityPolicyBusinessService policybusinessService,
        IIdentityRoleBusinessService rolebusinessService,
        IIdentityRolePolicyBusinessService businessService)
    {
        _businessService = businessService;
        _policybusinessService = policybusinessService;
        _rolebusinessService = rolebusinessService;


        RuleFor(x => x.Policy_Nm)
            .NotEmpty().WithMessage("Policy name is required.")
            .MustAsync(async (policyName, ct) =>
            {
                var policy = await _policybusinessService.GetByNameAsync(policyName);
                return policy is not null;
            }).WithMessage(x => $"Policy '{x.Policy_Nm}' does not exist.");

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
                var policy = await _policybusinessService.GetByNameAsync(command.Policy_Nm);
                var role = await _rolebusinessService.GetByNameAsync(command.Role_Nm);

                if (policy is null || role is null)
                    return true;

                var exists = await _businessService.IsRolePolicyMappingExistsAsync(policy.Id, role.Id);
                return !exists;
            }).WithMessage(x => $"Mapping for '{x.Policy_Nm}' and '{x.Role_Nm}' already exists.");
    }
}