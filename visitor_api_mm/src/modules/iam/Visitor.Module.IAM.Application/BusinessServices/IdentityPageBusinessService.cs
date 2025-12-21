namespace Visitor.Module.IAM.Application.BusinessServices;

public interface IIdentityPageBusinessService
{
    Task<PaginatedList<IdentityPageList>> GetAllAsync(IdentityPage dto, int index, int size);
    
    Task<IdentityPageDetail> GetByIdAsync(Guid id);
    
    Task<IdentityPage> GetByNameAsync(string Page_Nm);

    Task<bool> ParentIdExistAsync(Guid Parent_Id);

    Task<Result<bool>> CreateAsync(IdentityPage dto);

    Task<Result<bool>> UpdateAsync(IdentityPage dto);

    Task<Result<bool>> DeleteAsync(Guid id);
}

public class IdentityPageBusinessService : IIdentityPageBusinessService
{
    private readonly IIdentityPageDataLayerService _dataLayerService;
    private readonly ILogger<IdentityPageBusinessService> _logger;

    public IdentityPageBusinessService(IIdentityPageDataLayerService dataLayerService, ILogger<IdentityPageBusinessService> logger)
    {
        _dataLayerService = dataLayerService;
        _logger = logger;
    }

    public Task<PaginatedList<IdentityPageList>> GetAllAsync(IdentityPage dto, int index, int size)
    {
        return _dataLayerService.GetAllAsync(dto, index, size);
    }
    
    public Task<IdentityPageDetail> GetByIdAsync(Guid id)
    {
        return _dataLayerService.GetByIdAsync(id);
    }
    
    public async Task<IdentityPage> GetByNameAsync(string Page_Nm)
    {
        return await _dataLayerService.GetByNameAsync(Page_Nm);
    }

    public Task<bool> ParentIdExistAsync(Guid Parent_Id)
    {
        return _dataLayerService.ParentIdExistAsync(Parent_Id);
    }

    public async Task<Result<bool>> CreateAsync(IdentityPage dto)
    {
        try
        {
            await _dataLayerService.CreateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_CREATED} : {dto.Page_Nm!}", propertyName: nameof(IdentityPage)));
        }
    }
    
    public async Task<Result<bool>> UpdateAsync(IdentityPage dto)
    {        
        try
        {
            await _dataLayerService.UpdateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_UPDATED} : {dto.Page_Nm!}", propertyName: nameof(IdentityPage)));
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
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_DELETED} : {id!}", propertyName: nameof(IdentityPage)));
        }
    }
}
