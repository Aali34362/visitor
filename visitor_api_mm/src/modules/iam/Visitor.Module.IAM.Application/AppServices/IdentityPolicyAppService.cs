namespace Visitor.Module.IAM.Application.AppServices;

public interface IIdentityPolicyAppService
{
    Task<Result<PaginatedList<IdentityPolicyList>>> GetAllAsync(GetPolicyListQuery query);
    
    Task<Result<IdentityPolicyDetail>> GetByIdAsync(GetPolicyQuery query);
    
    Task<Result<bool>> CreateAsync(CreatePolicyCommand command);
    
    Task<Result<bool>> UpdateAsync(UpdatePolicyCommand command);

    Task<Result<bool>> DeleteAsync(DeletePolicyCommand command);
}

public class IdentityPolicyAppService : IIdentityPolicyAppService
{
    private readonly IIdentityPolicyBusinessService _businessService;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;

    public IdentityPolicyAppService(IIdentityPolicyBusinessService businessService,IValidationService validationService, IMapper mapper)
    {
        _businessService = businessService;
        _validationService = validationService;
        _mapper = mapper;
    }
    
    public async Task<Result<bool>> CreateAsync(CreatePolicyCommand command)
    {
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Error);

        var data = await _businessService.GetByNameAsync(command.Name);
        if (data is not null)
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.DUPLICATE_DATA} : {command.Name!}", propertyName: nameof(IdentityPolicy)));

        var Policy = _mapper.Map<CreatePolicyCommand, IdentityPolicy>(command);

        return await _businessService.CreateAsync(Policy);
    }
    
    public async Task<Result<bool>> UpdateAsync(UpdatePolicyCommand command)
    {
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Error);

        var data = await _businessService.GetByIdAsync(command.Id);
        if (data is null)
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {command.Id!}", propertyName: nameof(IdentityPolicy)));

        var Policy = _mapper.Map<UpdatePolicyCommand, IdentityPolicy>(command);
        Policy.Name = data.Name; 
        Policy.CreatedAt = data.UpdatedAt;
        Policy.CreatedBy = data.UpdatedBy; 

        return await _businessService.UpdateAsync(Policy);
    }
    
    public async Task<Result<bool>> DeleteAsync(DeletePolicyCommand command)
    {
        var data = await _businessService.GetByIdAsync(command.Id);
        if (data is null)
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {command.Id!}", propertyName: nameof(IdentityPolicy)));

        return await _businessService.DeleteAsync(command.Id);
    }

    public async Task<Result<PaginatedList<IdentityPolicyList>>> GetAllAsync(GetPolicyListQuery query)
    {
        var Policy = _mapper.Map<GetPolicyListQuery, IdentityPolicy>(query);

        var data = await _businessService.GetAllAsync(Policy, query.index, query.size);
        if (data is null)
            return Result<PaginatedList<IdentityPolicyList>>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} ", propertyName: nameof(IdentityPolicy)));
        
        return Result<PaginatedList<IdentityPolicyList>>.Success(data);
    }
    
    public async Task<Result<IdentityPolicyDetail>> GetByIdAsync(GetPolicyQuery query)
    {
        var data = await _businessService.GetByIdAsync(query.Id);
        if (data is null)
            return Result<IdentityPolicyDetail>.Failure(ErrorDetail.NotFound(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {query.Id} ", propertyName: nameof(IdentityPolicy)));

        return Result<IdentityPolicyDetail>.Success(data);
    }
}
