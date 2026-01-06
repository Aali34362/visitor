namespace Visitor.Module.IAM.Application.DecoratorServices.ValidatorServices;

public class CreatePolicyPageActionCommandValidator : AbstractValidator<CreatePolicyPageActionMappingCommand>
{
    private readonly IIdentityPolicyPageActionBusinessService _businessService;
    private readonly IIdentityPolicyBusinessService _policybusinessService;
    private readonly IIdentityPageActionBusinessService _pageActionbusinessService;

    public CreatePolicyPageActionCommandValidator(
        IIdentityPolicyBusinessService policybusinessService,
        IIdentityPageActionBusinessService pageActionbusinessService,
        IIdentityPolicyPageActionBusinessService businessService)
    {
        _businessService = businessService;
        _policybusinessService = policybusinessService;
        _pageActionbusinessService = pageActionbusinessService;


        RuleFor(x => x.Policy_Nm)
            .NotEmpty().WithMessage("Policy name is required.")
            .MustAsync(async (policyName, ct) =>
            {
                var policy = await _policybusinessService.GetByNameAsync(policyName);
                return policy is not null;
            }).WithMessage(x => $"Policy '{x.Policy_Nm}' does not exist.");

        RuleFor(x => x.PageAction_Nm)
            .NotEmpty().WithMessage("Page action name is required.")
            .MustAsync(async (pageActionName, ct) =>
            {
                var pageAction = await _pageActionbusinessService.GetByNameAsync(pageActionName);
                return pageAction is not null;
            }).WithMessage(x => $"PageAction '{x.PageAction_Nm}' does not exist.");

        RuleFor(x => x)
            .MustAsync(async (command, ct) =>
            {
                var policy = await _policybusinessService.GetByNameAsync(command.Policy_Nm);
                var pageAction = await _pageActionbusinessService.GetByNameAsync(command.PageAction_Nm);

                if (policy is null || pageAction is null)
                    return true; // Let other rules handle existence error

                var exists = await _businessService.IsPolicyPageActionMappingExistsAsync(policy.id, pageAction.id);
                return !exists;
            }).WithMessage(x => $"Mapping for '{x.Policy_Nm}' and '{x.PageAction_Nm}' already exists.");
    }
}


public class UpdatePolicyPageActionCommandValidator : AbstractValidator<UpdatePolicyPageActionMappingCommand>
{
    private readonly IIdentityPolicyPageActionBusinessService _businessService;
    private readonly IIdentityPolicyBusinessService _policybusinessService;
    private readonly IIdentityPageActionBusinessService _pageActionbusinessService;

    public UpdatePolicyPageActionCommandValidator(
        IIdentityPolicyBusinessService policybusinessService,
        IIdentityPageActionBusinessService pageActionbusinessService,
        IIdentityPolicyPageActionBusinessService businessService)
    {
        _businessService = businessService;
        _policybusinessService = policybusinessService;
        _pageActionbusinessService = pageActionbusinessService;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Mapping ID is required.");

        RuleFor(x => x.Policy_Nm)
            .NotEmpty().WithMessage("Policy name is required.")
            .MustAsync(async (policyName, ct) =>
            {
                var policy = await _policybusinessService.GetByNameAsync(policyName);
                return policy is not null;
            }).WithMessage(x => $"Policy '{x.Policy_Nm}' does not exist.");

        RuleFor(x => x.PageAction_Nm)
            .NotEmpty().WithMessage("Page action name is required.")
            .MustAsync(async (pageActionName, ct) =>
            {
                var pageAction = await _pageActionbusinessService.GetByNameAsync(pageActionName);
                return pageAction is not null;
            }).WithMessage(x => $"PageAction '{x.PageAction_Nm}' does not exist.");

        RuleFor(x => x)
            .MustAsync(async (command, ct) =>
            {
                var policy = await _policybusinessService.GetByNameAsync(command.Policy_Nm);
                var pageAction = await _pageActionbusinessService.GetByNameAsync(command.PageAction_Nm);

                if (policy is null || pageAction is null)
                    return true; // Let other rules handle existence error

                var exists = await _businessService.IsPolicyPageActionMappingExistsAsync(policy.id, pageAction.id);
                return !exists;
            }).WithMessage(x => $"Mapping for '{x.Policy_Nm}' and '{x.PageAction_Nm}' already exists.");
    }
}