namespace Visitor.Module.IAM.Application.AppServices;

public interface IIdentityRoleAppService
{
    Task<Result<PaginatedList<IdentityRoleList>>> GetAllAsync(GetRoleListQuery query);
    
    Task<Result<IdentityRoleDetail>> GetByIdAsync(GetRoleQuery query);
    
    Task<Result<bool>> CreateAsync(CreateRoleCommand command);
    
    Task<Result<bool>> UpdateAsync(UpdateRoleCommand command);

    Task<Result<bool>> DeleteAsync(DeleteRoleCommand command);
}

public class IdentityRoleAppService : IIdentityRoleAppService
{
    private readonly IIdentityRoleBusinessService _businessService;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;

    public IdentityRoleAppService(IIdentityRoleBusinessService businessService,IValidationService validationService, IMapper mapper)
    {
        _businessService = businessService;
        _validationService = validationService;
        _mapper = mapper;
    }
    
    public async Task<Result<bool>> CreateAsync(CreateRoleCommand command)
    {
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Error);

        var data = await _businessService.GetByNameAsync(command.Name);
        if (data is not null)
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.DUPLICATE_DATA} : {command.Name!}", propertyName: nameof(IdentityRole)));

        var Role = _mapper.Map<CreateRoleCommand, IdentityRole>(command);

        return await _businessService.CreateAsync(Role);
    }
    
    public async Task<Result<bool>> UpdateAsync(UpdateRoleCommand command)
    {
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Error);

        var data = await _businessService.GetByIdAsync(command.Id);
        if (data is null)
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {command.Id!}", propertyName: nameof(IdentityRole)));

        var Role = _mapper.Map<UpdateRoleCommand, IdentityRole>(command);
        Role.name = data.name; 
        Role.created_At = data.updated_At;
        Role.created_By = data.updated_By; 

        return await _businessService.UpdateAsync(Role);
    }
    
    public async Task<Result<bool>> DeleteAsync(DeleteRoleCommand command)
    {
        var data = await _businessService.GetByIdAsync(command.Id);
        if (data is null)
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {command.Id!}", propertyName: nameof(IdentityRole)));

        return await _businessService.DeleteAsync(command.Id);
    }

    public async Task<Result<PaginatedList<IdentityRoleList>>> GetAllAsync(GetRoleListQuery query)
    {
        var Role = _mapper.Map<GetRoleListQuery, IdentityRole>(query);

        var data = await _businessService.GetAllAsync(Role, query.index, query.size);
        if (data is null)
            return Result<PaginatedList<IdentityRoleList>>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} ", propertyName: nameof(IdentityRole)));
        
        return Result<PaginatedList<IdentityRoleList>>.Success(data);
    }
    
    public async Task<Result<IdentityRoleDetail>> GetByIdAsync(GetRoleQuery query)
    {
        var data = await _businessService.GetByIdAsync(query.Id);
        if (data is null)
            return Result<IdentityRoleDetail>.Failure(ErrorDetail.NotFound(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {query.Id} ", propertyName: nameof(IdentityRole)));

        return Result<IdentityRoleDetail>.Success(data);
    }
}
