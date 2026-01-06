namespace Visitor.Module.IAM.Application.AppServices;

public interface IIdentityRolePolicyAppService
{
    Task<Result<PaginatedList<IdentityRolePolicyMappingList>>> GetAllAsync(GetRolePolicyMappingListQuery query);
    
    Task<Result<IdentityRolePolicyMappingDetail>> GetByIdAsync(GetRolePolicyMappingQuery query);
    
    Task<Result<bool>> CreateAsync(CreateRolePolicyMappingCommand command);
    
    Task<Result<bool>> UpdateAsync(UpdateRolePolicyMappingCommand command);

    Task<Result<bool>> DeleteAsync(DeleteRolePolicyMappingCommand command);
}

public class IdentityRolePolicyAppService : IIdentityRolePolicyAppService
{
    private readonly IIdentityRolePolicyBusinessService _businessService;
    private readonly IIdentityPolicyBusinessService _policyBusinessService;
    private readonly IIdentityRoleBusinessService _roleBusinessService;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;

    public IdentityRolePolicyAppService(
        IIdentityRolePolicyBusinessService businessService,
        IIdentityPolicyBusinessService policyBusinessService,
        IIdentityRoleBusinessService roleBusinessService,
        IValidationService validationService,
        IMapper mapper)
    {
        _businessService = businessService;
        _policyBusinessService = policyBusinessService;
        _roleBusinessService = roleBusinessService;
        _validationService = validationService;
        _mapper = mapper;
    }

    public async Task<Result<bool>> CreateAsync(CreateRolePolicyMappingCommand command)
    {
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Error);

        var policy = await _policyBusinessService.GetByNameAsync(command.Policy_Nm);
        var role = await _roleBusinessService.GetByNameAsync(command.Role_Nm);

        var mapping = new IdentityRolePolicyMapping
        {
            policy_Id = policy.id,
            role_Id = role.id
        };

        return await _businessService.CreateAsync(mapping);
    }

    public async Task<Result<bool>> UpdateAsync(UpdateRolePolicyMappingCommand command)
    {
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Error);

        var existing = await _businessService.GetByIdAsync(command.Id);
        if (existing is null)
            return Result<bool>.Failure(ErrorDetail.Business(
                $"{CustomMessages.RECORD_NOT_FOUND} : {command.Id}",
                nameof(IdentityRolePolicyMapping)));

        var policy = await _policyBusinessService.GetByNameAsync(command.Policy_Nm);
        var role = await _roleBusinessService.GetByNameAsync(command.Role_Nm);

        var updated = new IdentityRolePolicyMapping
        {
            id = command.Id,
            policy_Id = policy.id,
            role_Id = role.id,
            created_At = existing.updated_At,
            created_By = existing.updated_By
        };

        return await _businessService.UpdateAsync(updated);
    }

    public async Task<Result<bool>> DeleteAsync(DeleteRolePolicyMappingCommand command)
    {
        var existing = await _businessService.GetByIdAsync(command.Id);
        if (existing is null)
            return Result<bool>.Failure(ErrorDetail.Business(
                $"{CustomMessages.RECORD_NOT_FOUND} : {command.Id}",
                nameof(IdentityRolePolicyMapping)));

        return await _businessService.DeleteAsync(command.Id);
    }

    public async Task<Result<PaginatedList<IdentityRolePolicyMappingList>>> GetAllAsync(GetRolePolicyMappingListQuery query)
    {
        Guid policyId = Guid.Empty, roleId = Guid.Empty;

        if (!string.IsNullOrWhiteSpace(query.Policy_Nm))
        {
            var policy = await _policyBusinessService.GetByNameAsync(query.Policy_Nm);
            if (policy is null)
                return Result<PaginatedList<IdentityRolePolicyMappingList>>.Failure(ErrorDetail.Business(
                    $"{CustomMessages.RECORD_NOT_FOUND} : {query.Policy_Nm}",
                    nameof(IdentityRolePolicyMapping)));

            policyId = policy.id;
        }

        if (!string.IsNullOrWhiteSpace(query.Role_Nm))
        {
            var role = await _roleBusinessService.GetByNameAsync(query.Role_Nm);
            if (role is null)
                return Result<PaginatedList<IdentityRolePolicyMappingList>>.Failure(ErrorDetail.Business(
                    $"{CustomMessages.RECORD_NOT_FOUND} : {query.Role_Nm}",
                    nameof(IdentityRolePolicyMapping)));

            roleId = role.id;
        }

        var data = await _businessService.GetAllAsync(new IdentityRolePolicyMapping
        {
            policy_Id = policyId,
            role_Id = roleId
        }, query.index, query.size);

        if (data is null)
            return Result<PaginatedList<IdentityRolePolicyMappingList>>.Failure(ErrorDetail.Business(
                CustomMessages.RECORD_NOT_FOUND,
                nameof(IdentityRolePolicyMapping)));

        return Result<PaginatedList<IdentityRolePolicyMappingList>>.Success(data);
    }

    public async Task<Result<IdentityRolePolicyMappingDetail>> GetByIdAsync(GetRolePolicyMappingQuery query)
    {
        var data = await _businessService.GetByIdAsync(query.Id);
        if (data is null)
            return Result<IdentityRolePolicyMappingDetail>.Failure(ErrorDetail.NotFound(
                $"{CustomMessages.RECORD_NOT_FOUND} : {query.Id}",
                nameof(IdentityRolePolicyMapping)));

        return Result<IdentityRolePolicyMappingDetail>.Success(data);
    }
}
