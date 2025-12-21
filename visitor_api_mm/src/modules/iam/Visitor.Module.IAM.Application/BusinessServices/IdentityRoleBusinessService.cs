namespace Visitor.Module.IAM.Application.BusinessServices;

public interface IIdentityRoleBusinessService
{
    Task<PaginatedList<IdentityRoleList>> GetAllAsync(IdentityRole dto, int index, int size);
    
    Task<IdentityRoleDetail> GetByIdAsync(Guid id);
    
    Task<IdentityRole> GetByNameAsync(string Role_Nm);

    Task<Result<bool>> CreateAsync(IdentityRole dto);

    Task<Result<bool>> UpdateAsync(IdentityRole dto);

    Task<Result<bool>> DeleteAsync(Guid id);
}

public class IdentityRoleBusinessService : IIdentityRoleBusinessService
{
    private readonly IIdentityRoleDataLayerService _dataLayerService;
    private readonly ILogger<IdentityRoleBusinessService> _logger;

    public IdentityRoleBusinessService(IIdentityRoleDataLayerService dataLayerService, ILogger<IdentityRoleBusinessService> logger)
    {
        _dataLayerService = dataLayerService;
        _logger = logger;
    }

    public Task<PaginatedList<IdentityRoleList>> GetAllAsync(IdentityRole dto, int index, int size)
    {
        return _dataLayerService.GetAllAsync(dto, index, size);
    }
    
    public Task<IdentityRoleDetail> GetByIdAsync(Guid id)
    {
        return _dataLayerService.GetByIdAsync(id);
    }
    
    public async Task<IdentityRole> GetByNameAsync(string Role_Nm)
    {
        return await _dataLayerService.GetByNameAsync(Role_Nm);
    }

    public async Task<Result<bool>> CreateAsync(IdentityRole dto)
    {
        try
        {
            await _dataLayerService.CreateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_CREATED} : {dto.Name!}", propertyName: nameof(IdentityRole)));
        }
    }
    
    public async Task<Result<bool>> UpdateAsync(IdentityRole dto)
    {        
        try
        {
            await _dataLayerService.UpdateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_UPDATED} : {dto.Name!}", propertyName: nameof(IdentityRole)));
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
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_DELETED} : {id!}", propertyName: nameof(IdentityRole)));
        }
    }
}
