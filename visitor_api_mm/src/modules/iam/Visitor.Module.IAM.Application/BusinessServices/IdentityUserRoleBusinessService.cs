namespace Visitor.Module.IAM.Application.BusinessServices;

public interface IIdentityUserRoleBusinessService
{
    Task<PaginatedList<IdentityUserRoleMappingList>> GetAllAsync(IdentityUserRoleMapping dto, int index, int size);
    
    Task<IdentityUserRoleMappingDetail> GetByIdAsync(Guid id);

    Task<bool> IsUserRoleMappingExistsAsync(Guid policy_id, Guid role_id);
    
    Task<Result<bool>> CreateAsync(IdentityUserRoleMapping dto);

    Task<Result<bool>> UpdateAsync(IdentityUserRoleMapping dto);

    Task<Result<bool>> DeleteAsync(Guid id);
}

public class IdentityUserRoleBusinessService : IIdentityUserRoleBusinessService
{
    private readonly IIdentityUserRoleDataLayerService _dataLayerService;
    private readonly ILogger<IdentityUserRoleBusinessService> _logger;

    public IdentityUserRoleBusinessService(IIdentityUserRoleDataLayerService dataLayerService, ILogger<IdentityUserRoleBusinessService> logger)
    {
        _dataLayerService = dataLayerService;
        _logger = logger;
    }

    public Task<PaginatedList<IdentityUserRoleMappingList>> GetAllAsync(IdentityUserRoleMapping dto, int index, int size)
    {
        return _dataLayerService.GetAllAsync(dto, index, size);
    }
    
    public Task<IdentityUserRoleMappingDetail> GetByIdAsync(Guid id)
    {
        return _dataLayerService.GetByIdAsync(id);
    }

    public Task<bool> IsUserRoleMappingExistsAsync(Guid policy_id, Guid role_id)
    {
        return _dataLayerService.IsUserRoleMappingExistsAsync(policy_id, role_id);
    }

    public async Task<Result<bool>> CreateAsync(IdentityUserRoleMapping dto)
    {
        try
        {
            await _dataLayerService.CreateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_CREATED}", propertyName: nameof(IdentityUserRoleMapping)));
        }
    }
    
    public async Task<Result<bool>> UpdateAsync(IdentityUserRoleMapping dto)
    {        
        try
        {
            await _dataLayerService.UpdateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_UPDATED}", propertyName: nameof(IdentityUserRoleMapping)));
        }
    }
    
    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        try
        {
            await _dataLayerService.DeleteAsync(id);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_DELETED} : {id!}", propertyName: nameof(IdentityUserRoleMapping)));
        }
    }
}
