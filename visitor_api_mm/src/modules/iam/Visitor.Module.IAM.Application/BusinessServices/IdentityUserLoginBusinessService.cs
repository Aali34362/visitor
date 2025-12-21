using Microsoft.EntityFrameworkCore;

namespace Visitor.Module.IAM.Application.BusinessServices;

public interface IIdentityUserLoginBusinessService
{
    Task<PaginatedList<IdentityUserLoginList>> GetAllAsync(IdentityUserLogin dto, int index, int size);

    Task<IdentityUserLogin> GetByIdAsync(Guid Id);
    
    Task<Result<bool>> CreateAsync(IdentityUserLogin dto);

    Task<Result<bool>> UpdateAsync(IdentityUserLogin dto);
}

public class IdentityUserLoginBusinessService : IIdentityUserLoginBusinessService
{
    private readonly IIdentityUserLoginDataLayerService _dataLayerService;
    private readonly ILogger<IdentityUserLoginBusinessService> _logger;

    public IdentityUserLoginBusinessService(IIdentityUserLoginDataLayerService dataLayerService, ILogger<IdentityUserLoginBusinessService> logger)
    {
        _dataLayerService = dataLayerService;
        _logger = logger;
    }

    public Task<PaginatedList<IdentityUserLoginList>> GetAllAsync(IdentityUserLogin dto, int index, int size)
    {
        return _dataLayerService.GetAllAsync(dto, index, size);
    }

    public Task<IdentityUserLogin> GetByIdAsync(Guid Id)
    {
        return _dataLayerService.GetByIdAsync(Id);
    }

    public async Task<Result<bool>> CreateAsync(IdentityUserLogin dto)
    {
        try
        {
            await _dataLayerService.CreateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_CREATED} : {dto.User_Id!}", propertyName: nameof(IdentityUserLogin)));
        }
    }
    
    public async Task<Result<bool>> UpdateAsync(IdentityUserLogin dto)
    {        
        try
        {
            await _dataLayerService.UpdateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_UPDATED} : {dto.User_Id!}", propertyName: nameof(IdentityUserLogin)));
        }
    }
}
