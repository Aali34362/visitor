namespace Visitor.Module.IAM.Application.DataLayerServices;

public interface IIdentityModuleDataLayerService
{
    Task<PaginatedList<IdentityModuleList>> GetAllAsync(IdentityModule dto, int index, int size);
    
    Task<IdentityModuleDetail> GetByIdAsync(Guid id);
    
    Task<IdentityModule> GetByNameAsync(string module_Nm);
    
    Task CreateAsync(IdentityModule dto);
    
    Task UpdateAsync(IdentityModule dto);
    
    Task DeleteAsync(Guid id);
}

public class IdentityModuleDataLayerService : IIdentityModuleDataLayerService
{
    private readonly IDbContextFactory<IAMApplicationDbContext> _factory;
    private readonly ICascadeDeleteManager _cascadeDeleteManager;
    private readonly IAMApplicationDbContext _dbContext;
    private readonly ILogger<IdentityModuleDataLayerService> _logger;

    public IdentityModuleDataLayerService(IDbContextFactory<IAMApplicationDbContext> factory, ICascadeDeleteManager cascadeDeleteManager, ILogger<IdentityModuleDataLayerService> logger)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _dbContext = _factory.CreateDbContext();
        _cascadeDeleteManager = cascadeDeleteManager ?? throw new ArgumentNullException(nameof(cascadeDeleteManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
    }

    public async Task<PaginatedList<IdentityModuleList>> GetAllAsync(IdentityModule dto, int index, int size)
    {
        return await _dbContext.GetModuleListAsync(dto, index, size);
    }

    public async Task<IdentityModuleDetail> GetByIdAsync(Guid id)
    {
        return await _dbContext.GetModuleByIdAsync(id);
    }

    public async Task<IdentityModule> GetByNameAsync(string module_Nm)
    {
        return await _dbContext.GetModuleByNameAsync(module_Nm);
    }

    public async Task CreateAsync(IdentityModule dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.CreateModule(dto);
            await _dbContext.CommitTransactionAsync(transaction!);
        }
        catch (Exception ex)
        {
            _dbContext.RollbackTransaction();
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(IdentityModule dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.UpdateModule(dto);
            await _dbContext.CommitTransactionAsync(transaction!);
        }
        catch (Exception ex)
        {
            _dbContext.RollbackTransaction();
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {       
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            await _cascadeDeleteManager.DeactivateModuleAsync(id);
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
