namespace Visitor.Module.IAM.Application.AppServices;

public interface IIdentityModuleAppService
{
    Task<Result<PaginatedList<IdentityModuleList>>> GetAllAsync(GetModuleListQuery query);
    
    Task<Result<IdentityModuleDetail>> GetByIdAsync(GetModuleQuery query);
    
    Task<Result<bool>> CreateAsync(CreateModuleCommand command);
    
    Task<Result<bool>> UpdateAsync(UpdateModuleCommand command);

    Task<Result<bool>> DeleteAsync(DeleteModuleCommand command);
}

public class IdentityModuleAppService : IIdentityModuleAppService
{
    private readonly IIdentityModuleBusinessService _businessService;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;

    public IdentityModuleAppService(IIdentityModuleBusinessService businessService, IValidationService validationService, IMapper mapper)
    {
        _businessService = businessService;
        _validationService = validationService;
        _mapper = mapper;
    }
    
    public async Task<Result<bool>> CreateAsync(CreateModuleCommand command)
    {
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Error);

        var data = await _businessService.GetByNameAsync(command.Name);
        if (data is not null)
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.DUPLICATE_DATA} : {command.Name!}", propertyName: nameof(IdentityModule)));

        var module = _mapper.Map<CreateModuleCommand,IdentityModule>(command);
        return await _businessService.CreateAsync(module);
    }
    
    public async Task<Result<bool>> UpdateAsync(UpdateModuleCommand command)
    {
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Error);

        var data = await _businessService.GetByIdAsync(command.Id);
        if (data is null)
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {command.Id!}", propertyName: nameof(IdentityModule)));

        var module = _mapper.Map<UpdateModuleCommand, IdentityModule>(command);
        module.name = data.name; // Preserve the original name if not changed
        module.created_At = data.updated_At; // Preserve the original creation date
        module.created_By = data.updated_By; // Preserve the original creator

        return await _businessService.UpdateAsync(module);
    }
    
    public async Task<Result<bool>> DeleteAsync(DeleteModuleCommand command)
    {
        // DeActivate all other records in those tables where this module is used as a foreign key
        var data = await _businessService.GetByIdAsync(command.Id);
        if (data is null)
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {command.Id!}", propertyName: nameof(IdentityModule)));

        return await _businessService.DeleteAsync(command.Id);
    }

    public async Task<Result<PaginatedList<IdentityModuleList>>> GetAllAsync(GetModuleListQuery query)
    {
        var module = _mapper.Map<GetModuleListQuery, IdentityModule>(query);
        var data = await _businessService.GetAllAsync(module, query.index, query.size);
        if (data is null)
            return Result<PaginatedList<IdentityModuleList>>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} ", propertyName: nameof(IdentityModule)));
        
        return Result<PaginatedList<IdentityModuleList>>.Success(data);
    }
    
    public async Task<Result<IdentityModuleDetail>> GetByIdAsync(GetModuleQuery query)
    {
        var data = await _businessService.GetByIdAsync(query.Id);
        if (data is null)
            return Result<IdentityModuleDetail>.Failure(ErrorDetail.NotFound(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {query.Id} ", propertyName: nameof(IdentityModule)));

        return Result<IdentityModuleDetail>.Success(data);
    }
}
