namespace Visitor.Module.IAM.Application.AppServices;

public interface IIdentityUserLoginAppService
{
    Task<Result<PaginatedList<IdentityUserLoginList>>> GetAllAsync(GetUserLoginListQuery query);
        
    Task<Result<bool>> CreateAsync(CreateUserLoginCommand command);
    
    Task<Result<bool>> UpdateAsync(UpdateUserLoginCommand command);
}

public class IdentityUserLoginAppService : IIdentityUserLoginAppService
{
    private readonly IIdentityUserLoginBusinessService _businessService;
    private readonly IMapper _mapper;

    public IdentityUserLoginAppService(IIdentityUserLoginBusinessService businessService, IMapper mapper)
    {
        _businessService = businessService;
        _mapper = mapper;
    }
    
    public async Task<Result<bool>> CreateAsync(CreateUserLoginCommand command)
    {
        var UserLogin = _mapper.Map<CreateUserLoginCommand, IdentityUserLogin>(command);

        return await _businessService.CreateAsync(UserLogin);
    }
    
    public async Task<Result<bool>> UpdateAsync(UpdateUserLoginCommand command)
    {
        var data = await _businessService.GetByIdAsync(command.Id);
        if (data is null)
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {command.Id!}", propertyName: nameof(IdentityUserLogin)));

        data.Logout_Date = command.Logout_Date;
        return await _businessService.UpdateAsync(data);
    }

    public async Task<Result<PaginatedList<IdentityUserLoginList>>> GetAllAsync(GetUserLoginListQuery query)
    {
        var UserLogin = _mapper.Map<GetUserLoginListQuery, IdentityUserLogin>(query);

        var data = await _businessService.GetAllAsync(UserLogin, query.index, query.size);
        if (data is null)
            return Result<PaginatedList<IdentityUserLoginList>>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} ", propertyName: nameof(IdentityUserLogin)));
        
        return Result<PaginatedList<IdentityUserLoginList>>.Success(data);
    }
}
