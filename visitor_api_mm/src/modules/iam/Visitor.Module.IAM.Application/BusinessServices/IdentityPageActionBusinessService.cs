namespace Visitor.Module.IAM.Application.BusinessServices;

public interface IIdentityPageActionBusinessService
{
    Task<PaginatedList<IdentityPageActionList>> GetAllAsync(IdentityPageAction dto, int index, int size);

    Task<List<IdentityPageActionList>> GetListAsync(IdentityPageAction dto);
    
    Task<IdentityPageActionDetail> GetByIdAsync(Guid id);
    
    Task<IdentityPageAction> GetByNameAsync(string PageAction_Nm);

    Task<bool> ParentIdExistAsync(Guid Parent_Id);

    Task<Result<bool>> CreateAsync(IdentityPageAction dto);

    Task<Result<bool>> UpdateAsync(IdentityPageAction dto);

    Task<Result<bool>> DeleteAsync(Guid id);
}

public class IdentityPageActionBusinessService : IIdentityPageActionBusinessService
{
    private readonly IIdentityPageActionDataLayerService _dataLayerService;
    private readonly ILogger<IdentityPageActionBusinessService> _logger;

    public IdentityPageActionBusinessService(IIdentityPageActionDataLayerService dataLayerService, ILogger<IdentityPageActionBusinessService> logger)
    {
        _dataLayerService = dataLayerService;
        _logger = logger;
    }

    public Task<PaginatedList<IdentityPageActionList>> GetAllAsync(IdentityPageAction dto, int index, int size)
    {
        return _dataLayerService.GetAllAsync(dto, index, size);
    }

    public Task<List<IdentityPageActionList>> GetListAsync(IdentityPageAction dto)
    {
        return _dataLayerService.GetListAsync(dto);
    }

    public Task<IdentityPageActionDetail> GetByIdAsync(Guid id)
    {
        return _dataLayerService.GetByIdAsync(id);
    }
    
    public async Task<IdentityPageAction> GetByNameAsync(string PageAction_Nm)
    {
        return await _dataLayerService.GetByNameAsync(PageAction_Nm);
    }

    public Task<bool> ParentIdExistAsync(Guid Parent_Id)
    {
        return _dataLayerService.ParentIdExistAsync(Parent_Id);
    }

    public async Task<Result<bool>> CreateAsync(IdentityPageAction dto)
    {
        try
        {
            await _dataLayerService.CreateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_CREATED} : {dto.Name!}", propertyName: nameof(IdentityPageAction)));
        }
    }
    
    public async Task<Result<bool>> UpdateAsync(IdentityPageAction dto)
    {        
        try
        {
            await _dataLayerService.UpdateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_UPDATED} : {dto.Name!}", propertyName: nameof(IdentityPageAction)));
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
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_DELETED} : {id!}", propertyName: nameof(IdentityPageAction)));
        }
    }
}
