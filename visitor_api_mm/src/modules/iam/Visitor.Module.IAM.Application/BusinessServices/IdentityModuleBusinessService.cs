namespace Visitor.Module.IAM.Application.BusinessServices;

public interface IIdentityModuleBusinessService
{
    Task<PaginatedList<IdentityModuleList>> GetAllAsync(IdentityModule dto, int index, int size);
    
    Task<IdentityModuleDetail> GetByIdAsync(Guid id);
    
    Task<IdentityModule> GetByNameAsync(string module_Nm);
    
    Task<Result<bool>> CreateAsync(IdentityModule dto);

    Task<Result<bool>> UpdateAsync(IdentityModule dto);

    Task<Result<bool>> DeleteAsync(Guid id);
}

public class IdentityModuleBusinessService : IIdentityModuleBusinessService
{
    private readonly IIdentityModuleDataLayerService _dataLayerService;
    private readonly ILogger<IdentityModuleBusinessService> _logger;

    public IdentityModuleBusinessService(IIdentityModuleDataLayerService dataLayerService, ILogger<IdentityModuleBusinessService> logger)
    {
        _dataLayerService = dataLayerService;
        _logger = logger;
    }

    public Task<PaginatedList<IdentityModuleList>> GetAllAsync(IdentityModule dto, int index, int size)
    {
        return _dataLayerService.GetAllAsync(dto, index, size);
    }
    
    public Task<IdentityModuleDetail> GetByIdAsync(Guid id)
    {
        return _dataLayerService.GetByIdAsync(id);
    }
    
    public async Task<IdentityModule> GetByNameAsync(string module_Nm)
    {
        return await _dataLayerService.GetByNameAsync(module_Nm);
    }
    
    public async Task<Result<bool>> CreateAsync(IdentityModule dto)
    {
        try
        {
            await _dataLayerService.CreateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_CREATED} : {dto.Name!}", propertyName: nameof(IdentityModule)));
        }
    }
    
    public async Task<Result<bool>> UpdateAsync(IdentityModule dto)
    {        
        try
        {
            await _dataLayerService.UpdateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_UPDATED} : {dto.Name!}", propertyName: nameof(IdentityModule)));
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
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_DELETED} : {id!}", propertyName: nameof(IdentityModule)));
        }
    }
}
