namespace Visitor.Module.IAM.Application.BusinessServices;

public interface IIdentityPolicyPageActionBusinessService
{
    Task<PaginatedList<IdentityPolicyPageActionMappingList>> GetAllAsync(IdentityPolicyPageActionMapping dto, int index, int size);
    
    Task<IdentityPolicyPageActionMappingDetail> GetByIdAsync(Guid id);

    Task<bool> IsPolicyPageActionMappingExistsAsync(Guid policy_id, Guid pageAction_id);
    
    Task<Result<bool>> CreateAsync(IdentityPolicyPageActionMapping dto);

    // Create List Async method

    Task<Result<bool>> UpdateAsync(IdentityPolicyPageActionMapping dto);

    Task<Result<bool>> DeleteAsync(Guid id);
}

public class IdentityPolicyPageActionBusinessService : IIdentityPolicyPageActionBusinessService
{
    private readonly IIdentityPolicyPageActionDataLayerService _dataLayerService;
    private readonly ILogger<IdentityPolicyPageActionBusinessService> _logger;

    public IdentityPolicyPageActionBusinessService(IIdentityPolicyPageActionDataLayerService dataLayerService, ILogger<IdentityPolicyPageActionBusinessService> logger)
    {
        _dataLayerService = dataLayerService;
        _logger = logger;
    }

    public Task<PaginatedList<IdentityPolicyPageActionMappingList>> GetAllAsync(IdentityPolicyPageActionMapping dto, int index, int size)
    {
        return _dataLayerService.GetAllAsync(dto, index, size);
    }
    
    public Task<IdentityPolicyPageActionMappingDetail> GetByIdAsync(Guid id)
    {
        return _dataLayerService.GetByIdAsync(id);
    }

    public Task<bool> IsPolicyPageActionMappingExistsAsync(Guid policy_id, Guid pageAction_id)
    {
        return _dataLayerService.IsPolicyPageActionMappingExistsAsync(policy_id, pageAction_id);
    }

    public async Task<Result<bool>> CreateAsync(IdentityPolicyPageActionMapping dto)
    {
        try
        {
            await _dataLayerService.CreateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_CREATED}", propertyName: nameof(IdentityPolicyPageActionMapping)));
        }
    }
    
    public async Task<Result<bool>> UpdateAsync(IdentityPolicyPageActionMapping dto)
    {        
        try
        {
            await _dataLayerService.UpdateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_UPDATED}", propertyName: nameof(IdentityPolicyPageActionMapping)));
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
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_DELETED} : {id!}", propertyName: nameof(IdentityPolicyPageActionMapping)));
        }
    }
}
