namespace Visitor.Module.IAM.Application.BusinessServices;

public interface IIdentityPolicyBusinessService
{
    Task<PaginatedList<IdentityPolicyList>> GetAllAsync(IdentityPolicy dto, int index, int size);
    
    Task<IdentityPolicyDetail> GetByIdAsync(Guid id);
    
    Task<IdentityPolicy> GetByNameAsync(string Policy_Nm);

    Task<Result<bool>> CreateAsync(IdentityPolicy dto);

    Task<Result<bool>> UpdateAsync(IdentityPolicy dto);

    Task<Result<bool>> DeleteAsync(Guid id);
}

public class IdentityPolicyBusinessService : IIdentityPolicyBusinessService
{
    private readonly IIdentityPolicyDataLayerService _dataLayerService;
    private readonly ILogger<IdentityPolicyBusinessService> _logger;

    public IdentityPolicyBusinessService(IIdentityPolicyDataLayerService dataLayerService, ILogger<IdentityPolicyBusinessService> logger)
    {
        _dataLayerService = dataLayerService;
        _logger = logger;
    }

    public Task<PaginatedList<IdentityPolicyList>> GetAllAsync(IdentityPolicy dto, int index, int size)
    {
        return _dataLayerService.GetAllAsync(dto, index, size);
    }
    
    public Task<IdentityPolicyDetail> GetByIdAsync(Guid id)
    {
        return _dataLayerService.GetByIdAsync(id);
    }
    
    public async Task<IdentityPolicy> GetByNameAsync(string Policy_Nm)
    {
        return await _dataLayerService.GetByNameAsync(Policy_Nm);
    }

    public async Task<Result<bool>> CreateAsync(IdentityPolicy dto)
    {
        try
        {
            await _dataLayerService.CreateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_CREATED} : {dto.Name!}", propertyName: nameof(IdentityPolicy)));
        }
    }
    
    public async Task<Result<bool>> UpdateAsync(IdentityPolicy dto)
    {        
        try
        {
            await _dataLayerService.UpdateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_UPDATED} : {dto.Name!}", propertyName: nameof(IdentityPolicy)));
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
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_DELETED} : {id!}", propertyName: nameof(IdentityPolicy)));
        }
    }
}
