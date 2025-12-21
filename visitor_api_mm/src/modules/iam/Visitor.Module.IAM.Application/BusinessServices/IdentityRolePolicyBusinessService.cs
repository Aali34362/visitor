namespace Visitor.Module.IAM.Application.BusinessServices;

public interface IIdentityRolePolicyBusinessService
{
    Task<PaginatedList<IdentityRolePolicyMappingList>> GetAllAsync(IdentityRolePolicyMapping dto, int index, int size);
    
    Task<IdentityRolePolicyMappingDetail> GetByIdAsync(Guid id);

    Task<bool> IsRolePolicyMappingExistsAsync(Guid policy_id, Guid role_id);
    
    Task<Result<bool>> CreateAsync(IdentityRolePolicyMapping dto);

    Task<Result<bool>> UpdateAsync(IdentityRolePolicyMapping dto);

    Task<Result<bool>> DeleteAsync(Guid id);
}

public class IdentityRolePolicyBusinessService : IIdentityRolePolicyBusinessService
{
    private readonly IIdentityRolePolicyDataLayerService _dataLayerService;
    private readonly ILogger<IdentityRolePolicyBusinessService> _logger;

    public IdentityRolePolicyBusinessService(IIdentityRolePolicyDataLayerService dataLayerService, ILogger<IdentityRolePolicyBusinessService> logger)
    {
        _dataLayerService = dataLayerService;
        _logger = logger;
    }

    public Task<PaginatedList<IdentityRolePolicyMappingList>> GetAllAsync(IdentityRolePolicyMapping dto, int index, int size)
    {
        return _dataLayerService.GetAllAsync(dto, index, size);
    }
    
    public Task<IdentityRolePolicyMappingDetail> GetByIdAsync(Guid id)
    {
        return _dataLayerService.GetByIdAsync(id);
    }

    public Task<bool> IsRolePolicyMappingExistsAsync(Guid policy_id, Guid role_id)
    {
        return _dataLayerService.IsRolePolicyMappingExistsAsync(policy_id, role_id);
    }

    public async Task<Result<bool>> CreateAsync(IdentityRolePolicyMapping dto)
    {
        try
        {
            await _dataLayerService.CreateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_CREATED}", propertyName: nameof(IdentityRolePolicyMapping)));
        }
    }
    
    public async Task<Result<bool>> UpdateAsync(IdentityRolePolicyMapping dto)
    {        
        try
        {
            await _dataLayerService.UpdateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_UPDATED}", propertyName: nameof(IdentityRolePolicyMapping)));
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
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_DELETED} : {id!}", propertyName: nameof(IdentityRolePolicyMapping)));
        }
    }
}
