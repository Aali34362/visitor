using System.Drawing;

namespace Visitor.Module.IAM.Application.DataLayerServices;

public interface IIdentityUserLoginDataLayerService
{
    Task<PaginatedList<IdentityUserLoginList>> GetAllAsync(IdentityUserLogin dto, int index, int size);

    Task<IdentityUserLogin> GetByIdAsync(Guid Id);

    Task CreateAsync(IdentityUserLogin dto);
    
    Task UpdateAsync(IdentityUserLogin dto);
    
}

public class IdentityUserLoginDataLayerService : IIdentityUserLoginDataLayerService
{
    private readonly IDbContextFactory<IAMApplicationDbContext> _factory;
    private readonly IAMApplicationDbContext _dbContext;
    private readonly ILogger<IdentityUserLoginDataLayerService> _logger;

    public IdentityUserLoginDataLayerService(IDbContextFactory<IAMApplicationDbContext> factory, ILogger<IdentityUserLoginDataLayerService> logger)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _dbContext = _factory.CreateDbContext();
        _logger = logger;
    }

    public async Task<PaginatedList<IdentityUserLoginList>> GetAllAsync(IdentityUserLogin dto, int index, int size)
    {
        return await _dbContext.GetUserLoginListAsync(dto, index, size);
    }

    public async Task<IdentityUserLogin> GetByIdAsync(Guid Id)
    {
        return await _dbContext.GetUserLoginByIdAsync(Id);
    }

    public async Task CreateAsync(IdentityUserLogin dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.CreateUserLogin(dto);
            await _dbContext.CommitTransactionAsync(transaction!);
        }
        catch (Exception ex)
        {
            _dbContext.RollbackTransaction();
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(IdentityUserLogin dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.UpdateUserLogin(dto);
            await _dbContext.CommitTransactionAsync(transaction!);
        }
        catch (Exception ex)
        {
            _dbContext.RollbackTransaction();
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            throw;
        }
    }
}
