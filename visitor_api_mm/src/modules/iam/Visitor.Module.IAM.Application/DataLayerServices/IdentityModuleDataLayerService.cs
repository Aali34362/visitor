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
    private readonly ILogger<IdentityModuleDataLayerService> _logger;

    public IdentityModuleDataLayerService(IDbContextFactory<IAMApplicationDbContext> factory, ICascadeDeleteManager cascadeDeleteManager, ILogger<IdentityModuleDataLayerService> logger)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _cascadeDeleteManager = cascadeDeleteManager ?? throw new ArgumentNullException(nameof(cascadeDeleteManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
    }

    public async Task<PaginatedList<IdentityModuleList>> GetAllAsync(IdentityModule dto, int index, int size)
    {
        var _dbContext = await _factory.CreateDbContextAsync();
        return await _dbContext.GetModuleListAsync(dto, index, size);
    }

    public async Task<IdentityModuleDetail> GetByIdAsync(Guid id)
    {
        var _dbContext = await _factory.CreateDbContextAsync();
        return await _dbContext.GetModuleByIdAsync(id);
    }

    public async Task<IdentityModule> GetByNameAsync(string module_Nm)
    {
        var _dbContext = await _factory.CreateDbContextAsync();
        return await _dbContext.GetByNameForValidationAsync(module_Nm);
    }

    public async Task CreateAsync(IdentityModule dto)
    {
        try
        {
            var _dbContext = await _factory.CreateDbContextAsync();
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.Create(dto);
            await _dbContext.CommitTransactionAsync(transaction!);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(IdentityModule dto)
    {
        try
        {
            var _dbContext = await _factory.CreateDbContextAsync();
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.Update(dto);
            await _dbContext.CommitTransactionAsync(transaction!);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {       
        try
        {
            var _dbContext = await _factory.CreateDbContextAsync();
            await using var transaction = await _dbContext.BeginTransactionAsync();
            await _cascadeDeleteManager.DeactivateModuleAsync(id);
            await _dbContext.CommitTransactionAsync(transaction!);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            throw;
        }
    }
}
