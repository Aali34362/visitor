using Microsoft.EntityFrameworkCore;

namespace Visitor.Module.IAM.Application.BusinessServices;

public interface IIdentityUserBusinessService
{
    Task<PaginatedList<IdentityUserList>> GetAllAsync(IdentityUser dto, int index, int size);
    
    Task<IdentityUserDetail> GetByIdAsync(Guid id);
    
    Task<IdentityUser> GetByNameAsync(string User_Nm);

    Task<IdentityUser> ValidateUserPasswordAsync(string User_Nm, string password);

    Task<Result<bool>> CreateAsync(IdentityUser dto);

    Task<Result<bool>> UpdateAsync(IdentityUser dto);

    Task<Result<bool>> DeleteAsync(Guid id);

    Task<bool> emailExistsAsync(string email);
}

public class IdentityUserBusinessService : IIdentityUserBusinessService
{
    private readonly IIdentityUserDataLayerService _dataLayerService;
    private readonly ILogger<IdentityUserBusinessService> _logger;

    public IdentityUserBusinessService(IIdentityUserDataLayerService dataLayerService, ILogger<IdentityUserBusinessService> logger)
    {
        _dataLayerService = dataLayerService;
        _logger = logger;
    }

    public Task<PaginatedList<IdentityUserList>> GetAllAsync(IdentityUser dto, int index, int size)
    {
        return _dataLayerService.GetAllAsync(dto, index, size);
    }
    
    public Task<IdentityUserDetail> GetByIdAsync(Guid id)
    {
        return _dataLayerService.GetByIdAsync(id);
    }

    public Task<IdentityUser> ValidateUserPasswordAsync(string User_Nm, string password) =>
        _dataLayerService.ValidateUserPasswordAsync(User_Nm,  password);
    
    public async Task<IdentityUser> GetByNameAsync(string User_Nm)
    {
        return await _dataLayerService.GetByNameAsync(User_Nm);
    }

    public async Task<Result<bool>> CreateAsync(IdentityUser dto)
    {
        try
        {
            await _dataLayerService.CreateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_CREATED} : {dto.UserName!}", propertyName: nameof(IdentityUser)));
        }
    }
    
    public async Task<Result<bool>> UpdateAsync(IdentityUser dto)
    {        
        try
        {
            await _dataLayerService.UpdateAsync(dto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_UPDATED} : {dto.UserName!}", propertyName: nameof(IdentityUser)));
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
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_DELETED} : {id!}", propertyName: nameof(IdentityUser)));
        }
    }

    public async Task<bool> emailExistsAsync(string email)
    {
        return await _dataLayerService.emailExistsAsync(email);
    }
}
