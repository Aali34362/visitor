namespace Visitor.Module.IAM.Application.DataLayerServices;

public interface IIdentityRoleDataLayerService
{
    Task<PaginatedList<IdentityRoleList>> GetAllAsync(IdentityRole dto, int index, int size);
    
    Task<IdentityRoleDetail> GetByIdAsync(Guid id);
    
    Task<IdentityRole> GetByNameAsync(string Role_Nm);
        
    Task CreateAsync(IdentityRole dto);
    
    Task UpdateAsync(IdentityRole dto);
    
    Task DeleteAsync(Guid id);
}

public class IdentityRoleDataLayerService : IIdentityRoleDataLayerService
{
    private readonly IDbContextFactory<IAMApplicationDbContext> _factory;
    private readonly IAMApplicationDbContext _dbContext;
    private readonly ICascadeDeleteManager _cascadeDeleteManager;
    private readonly ILogger<IdentityRoleDataLayerService> _logger;

    public IdentityRoleDataLayerService(IDbContextFactory<IAMApplicationDbContext> factory, ILogger<IdentityRoleDataLayerService> logger, ICascadeDeleteManager cascadeDeleteManager)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _dbContext = _factory.CreateDbContext();
        _logger = logger;
        _cascadeDeleteManager = cascadeDeleteManager;
    }

    public async Task<PaginatedList<IdentityRoleList>> GetAllAsync(IdentityRole dto, int index, int size)
    {
        return await _dbContext.GetRoleListAsync(dto, index, size);
    }

    public async Task<IdentityRoleDetail> GetByIdAsync(Guid id)
    {
        return await _dbContext.GetRoleByIdAsync(id);
    }

    public async Task<IdentityRole> GetByNameAsync(string Role_Nm)
    {
        return await _dbContext.GetRoleByNameAsync(Role_Nm);
    }

    public async Task CreateAsync(IdentityRole dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.CreateRole(dto);
            await _dbContext.CommitTransactionAsync(transaction!);
        }
        catch (Exception ex)
        {
            _dbContext.RollbackTransaction();
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(IdentityRole dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.UpdateRole(dto);
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
            await _cascadeDeleteManager.DeactivateRoleAsync(id);
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
