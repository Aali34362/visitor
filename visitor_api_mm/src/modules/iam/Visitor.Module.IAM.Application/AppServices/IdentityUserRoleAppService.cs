namespace Visitor.Module.IAM.Application.AppServices;

public interface IIdentityUserRoleAppService
{
    Task<Result<PaginatedList<IdentityUserRoleMappingList>>> GetAllAsync(GetUserRoleMappingListQuery query);
    
    Task<Result<IdentityUserRoleMappingDetail>> GetByIdAsync(GetUserRoleMappingQuery query);
    
    Task<Result<bool>> CreateAsync(CreateUserRoleMappingCommand command);
    
    Task<Result<bool>> UpdateAsync(UpdateUserRoleMappingCommand command);

    Task<Result<bool>> DeleteAsync(DeleteUserRoleMappingCommand command);
}

public class IdentityUserRoleAppService : IIdentityUserRoleAppService
{
    private readonly IIdentityUserRoleBusinessService _businessService;
    private readonly IIdentityUserBusinessService _userBusinessService;
    private readonly IIdentityRoleBusinessService _roleBusinessService;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;

    public IdentityUserRoleAppService(
        IIdentityUserRoleBusinessService businessService,
        IIdentityUserBusinessService userBusinessService,
        IIdentityRoleBusinessService roleBusinessService,
        IValidationService validationService,
        IMapper mapper)
    {
        _businessService = businessService;
        _userBusinessService = userBusinessService;
        _roleBusinessService = roleBusinessService;
        _validationService = validationService;
        _mapper = mapper;
    }

    public async Task<Result<bool>> CreateAsync(CreateUserRoleMappingCommand command)
    {
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Error);

        var user = await _userBusinessService.GetByNameAsync(command.User_Nm);
        var role = await _roleBusinessService.GetByNameAsync(command.Role_Nm);

        var mapping = new IdentityUserRoleMapping
        {
            user_Id = user.id,
            role_Id = role.id
        };

        return await _businessService.CreateAsync(mapping);
    }

    public async Task<Result<bool>> UpdateAsync(UpdateUserRoleMappingCommand command)
    {
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Error);

        var existing = await _businessService.GetByIdAsync(command.Id);
        if (existing is null)
            return Result<bool>.Failure(ErrorDetail.Business(
                $"{CustomMessages.RECORD_NOT_FOUND} : {command.Id}",
                nameof(IdentityUserRoleMapping)));

        var user = await _userBusinessService.GetByNameAsync(command.User_Nm);
        var role = await _roleBusinessService.GetByNameAsync(command.Role_Nm);

        var updated = new IdentityUserRoleMapping
        {
            id = command.Id,
            user_Id = user.id,
            role_Id = role.id,
            created_At = existing.updated_At,
            created_By = existing.updated_By
        };

        return await _businessService.UpdateAsync(updated);
    }

    public async Task<Result<bool>> DeleteAsync(DeleteUserRoleMappingCommand command)
    {
        var existing = await _businessService.GetByIdAsync(command.Id);
        if (existing is null)
            return Result<bool>.Failure(ErrorDetail.Business(
                $"{CustomMessages.RECORD_NOT_FOUND} : {command.Id}",
                nameof(IdentityUserRoleMapping)));

        return await _businessService.DeleteAsync(command.Id);
    }

    public async Task<Result<PaginatedList<IdentityUserRoleMappingList>>> GetAllAsync(GetUserRoleMappingListQuery query)
    {
        Guid userId = Guid.Empty, roleId = Guid.Empty;

        if (!string.IsNullOrWhiteSpace(query.User_Nm))
        {
            var user = await _userBusinessService.GetByNameAsync(query.User_Nm);
            if (user is null)
                return Result<PaginatedList<IdentityUserRoleMappingList>>.Failure(ErrorDetail.Business(
                    $"{CustomMessages.RECORD_NOT_FOUND} : {query.User_Nm}",
                    nameof(IdentityUserRoleMapping)));

            userId = user.id;
        }

        if (!string.IsNullOrWhiteSpace(query.Role_Nm))
        {
            var role = await _roleBusinessService.GetByNameAsync(query.Role_Nm);
            if (role is null)
                return Result<PaginatedList<IdentityUserRoleMappingList>>.Failure(ErrorDetail.Business(
                    $"{CustomMessages.RECORD_NOT_FOUND} : {query.Role_Nm}",
                    nameof(IdentityUserRoleMapping)));

            roleId = role.id;
        }

        var data = await _businessService.GetAllAsync(new IdentityUserRoleMapping
        {
            user_Id = userId,
            role_Id = roleId
        }, query.index, query.size);

        if (data is null)
            return Result<PaginatedList<IdentityUserRoleMappingList>>.Failure(ErrorDetail.Business(
                CustomMessages.RECORD_NOT_FOUND,
                nameof(IdentityUserRoleMapping)));

        return Result<PaginatedList<IdentityUserRoleMappingList>>.Success(data);
    }

    public async Task<Result<IdentityUserRoleMappingDetail>> GetByIdAsync(GetUserRoleMappingQuery query)
    {
        var data = await _businessService.GetByIdAsync(query.Id);
        if (data is null)
            return Result<IdentityUserRoleMappingDetail>.Failure(ErrorDetail.NotFound(
                $"{CustomMessages.RECORD_NOT_FOUND} : {query.Id}",
                nameof(IdentityUserRoleMapping)));

        return Result<IdentityUserRoleMappingDetail>.Success(data);
    }
}
