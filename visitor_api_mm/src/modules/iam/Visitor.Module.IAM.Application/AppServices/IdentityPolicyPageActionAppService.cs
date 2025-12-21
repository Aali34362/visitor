namespace Visitor.Module.IAM.Application.AppServices;

public interface IIdentityPolicyPageActionAppService
{
    Task<Result<PaginatedList<IdentityPolicyPageActionMappingList>>> GetAllAsync(GetPolicyPageActionMappingListQuery query);
    
    Task<Result<IdentityPolicyPageActionMappingDetail>> GetByIdAsync(GetPolicyPageActionMappingQuery query);
    
    Task<Result<bool>> CreateAsync(CreatePolicyPageActionMappingCommand command);
    
    Task<Result<bool>> UpdateAsync(UpdatePolicyPageActionMappingCommand command);

    Task<Result<bool>> DeleteAsync(DeletePolicyPageActionMappingCommand command);
}

public class IdentityPolicyPageActionAppService : IIdentityPolicyPageActionAppService
{
    private readonly IIdentityPolicyPageActionBusinessService _businessService;
    private readonly IIdentityPolicyBusinessService _policyBusinessService;
    private readonly IIdentityPageActionBusinessService _pageActionBusinessService;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;

    public IdentityPolicyPageActionAppService(
        IIdentityPolicyPageActionBusinessService businessService,
        IIdentityPolicyBusinessService policyBusinessService,
        IIdentityPageActionBusinessService pageActionBusinessService,
        IValidationService validationService,
        IMapper mapper)
    {
        _businessService = businessService;
        _policyBusinessService = policyBusinessService;
        _pageActionBusinessService = pageActionBusinessService;
        _validationService = validationService;
        _mapper = mapper;
    }

    public async Task<Result<bool>> CreateAsync(CreatePolicyPageActionMappingCommand command)
    {
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Error);

        var policy = await _policyBusinessService.GetByNameAsync(command.Policy_Nm);
        var pageAction = await _pageActionBusinessService.GetByNameAsync(command.PageAction_Nm);

        var mapping = new IdentityPolicyPageActionMapping
        {
            Policy_Id = policy.Id,
            PageAction_Id = pageAction.Id
        };

        return await _businessService.CreateAsync(mapping);
    }

    public async Task<Result<bool>> UpdateAsync(UpdatePolicyPageActionMappingCommand command)
    {
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Error);

        var existing = await _businessService.GetByIdAsync(command.Id);
        if (existing is null)
            return Result<bool>.Failure(ErrorDetail.Business(
                $"{CustomMessages.RECORD_NOT_FOUND} : {command.Id}",
                nameof(IdentityPolicyPageActionMapping)));

        var policy = await _policyBusinessService.GetByNameAsync(command.Policy_Nm);
        var pageAction = await _pageActionBusinessService.GetByNameAsync(command.PageAction_Nm);

        var updated = new IdentityPolicyPageActionMapping
        {
            Id = command.Id,
            Policy_Id = policy.Id,
            PageAction_Id = pageAction.Id,
            CreatedAt = existing.UpdatedAt,
            CreatedBy = existing.UpdatedBy
        };

        return await _businessService.UpdateAsync(updated);
    }

    public async Task<Result<bool>> DeleteAsync(DeletePolicyPageActionMappingCommand command)
    {
        var existing = await _businessService.GetByIdAsync(command.Id);
        if (existing is null)
            return Result<bool>.Failure(ErrorDetail.Business(
                $"{CustomMessages.RECORD_NOT_FOUND} : {command.Id}",
                nameof(IdentityPolicyPageActionMapping)));

        return await _businessService.DeleteAsync(command.Id);
    }

    public async Task<Result<PaginatedList<IdentityPolicyPageActionMappingList>>> GetAllAsync(GetPolicyPageActionMappingListQuery query)
    {
        Guid policyId = Guid.Empty, pageActionId = Guid.Empty;

        if (!string.IsNullOrWhiteSpace(query.Policy_Nm))
        {
            var policy = await _policyBusinessService.GetByNameAsync(query.Policy_Nm);
            if (policy is null)
                return Result<PaginatedList<IdentityPolicyPageActionMappingList>>.Failure(ErrorDetail.Business(
                    $"{CustomMessages.RECORD_NOT_FOUND} : {query.Policy_Nm}",
                    nameof(IdentityPolicyPageActionMapping)));

            policyId = policy.Id;
        }

        if (!string.IsNullOrWhiteSpace(query.PageAction_Nm))
        {
            var pageAction = await _pageActionBusinessService.GetByNameAsync(query.PageAction_Nm);
            if (pageAction is null)
                return Result<PaginatedList<IdentityPolicyPageActionMappingList>>.Failure(ErrorDetail.Business(
                    $"{CustomMessages.RECORD_NOT_FOUND} : {query.PageAction_Nm}",
                    nameof(IdentityPolicyPageActionMapping)));

            pageActionId = pageAction.Id;
        }

        var data = await _businessService.GetAllAsync(new IdentityPolicyPageActionMapping
        {
            Policy_Id = policyId,
            PageAction_Id = pageActionId
        }, query.index, query.size);

        if (data is null)
            return Result<PaginatedList<IdentityPolicyPageActionMappingList>>.Failure(ErrorDetail.Business(
                CustomMessages.RECORD_NOT_FOUND,
                nameof(IdentityPolicyPageActionMapping)));

        return Result<PaginatedList<IdentityPolicyPageActionMappingList>>.Success(data);
    }

    public async Task<Result<IdentityPolicyPageActionMappingDetail>> GetByIdAsync(GetPolicyPageActionMappingQuery query)
    {
        var data = await _businessService.GetByIdAsync(query.Id);
        if (data is null)
            return Result<IdentityPolicyPageActionMappingDetail>.Failure(ErrorDetail.NotFound(
                $"{CustomMessages.RECORD_NOT_FOUND} : {query.Id}",
                nameof(IdentityPolicyPageActionMapping)));

        return Result<IdentityPolicyPageActionMappingDetail>.Success(data);
    }
}
