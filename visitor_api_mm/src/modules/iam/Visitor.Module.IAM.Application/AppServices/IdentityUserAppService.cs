namespace Visitor.Module.IAM.Application.AppServices;

public interface IIdentityUserAppService
{
    Task<Result<PaginatedList<IdentityUserList>>> GetAllAsync(GetUserListQuery query);
    
    Task<Result<IdentityUserDetail>> GetByIdAsync(GetUserQuery query);
    
    Task<Result<bool>> CreateAsync(CreateUserCommand command);
    
    Task<Result<bool>> UpdateAsync(UpdateUserCommand command);

    Task<Result<bool>> DeleteAsync(DeleteUserCommand command);

    // Password Change, Forgot Password, Reset Password methods can be added here
    // OTP methods can also be added here

}

public class IdentityUserAppService : IIdentityUserAppService
{
    private readonly IIdentityUserBusinessService _businessService;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;

    public IdentityUserAppService(IIdentityUserBusinessService businessService,IValidationService validationService, IMapper mapper)
    {
        _businessService = businessService;
        _validationService = validationService;
        _mapper = mapper;
    }
    
    public async Task<Result<bool>> CreateAsync(CreateUserCommand command)
    {
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Error);

        var data = await _businessService.GetByNameAsync(command.UserName);
        if (data is not null)
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.DUPLICATE_DATA} : {command.UserName!}", propertyName: nameof(IdentityUser)));

        var User = _mapper.Map<CreateUserCommand, IdentityUser>(command);

        return await _businessService.CreateAsync(User);
    }
    
    public async Task<Result<bool>> UpdateAsync(UpdateUserCommand command)
    {
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Error);

        var data = await _businessService.GetByIdAsync(command.Id);
        if (data is null)
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {command.Id!}", propertyName: nameof(IdentityUser)));

        var User = _mapper.Map<UpdateUserCommand, IdentityUser>(command);
        User.UserName = data.UserName; 
        User.CreatedAt = data.UpdatedAt;
        User.CreatedBy = data.UpdatedBy; 

        return await _businessService.UpdateAsync(User);
    }
    
    public async Task<Result<bool>> DeleteAsync(DeleteUserCommand command)
    {
        var data = await _businessService.GetByIdAsync(command.Id);
        if (data is null)
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {command.Id!}", propertyName: nameof(IdentityUser)));

        return await _businessService.DeleteAsync(command.Id);
    }

    public async Task<Result<PaginatedList<IdentityUserList>>> GetAllAsync(GetUserListQuery query)
    {
        var User = _mapper.Map<GetUserListQuery, IdentityUser>(query);

        var data = await _businessService.GetAllAsync(User, query.index, query.size);
        if (data is null)
            return Result<PaginatedList<IdentityUserList>>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} ", propertyName: nameof(IdentityUser)));
        
        return Result<PaginatedList<IdentityUserList>>.Success(data);
    }
    
    public async Task<Result<IdentityUserDetail>> GetByIdAsync(GetUserQuery query)
    {
        var data = await _businessService.GetByIdAsync(query.Id);
        if (data is null)
            return Result<IdentityUserDetail>.Failure(ErrorDetail.NotFound(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {query.Id} ", propertyName: nameof(IdentityUser)));

        return Result<IdentityUserDetail>.Success(data);
    }
}
