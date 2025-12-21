namespace Visitor.Module.IAM.Application.DataLayerServices;

public interface IIdentityRolePolicyDataLayerService
{
    Task<PaginatedList<IdentityRolePolicyMappingList>> GetAllAsync(IdentityRolePolicyMapping dto, int index, int size);
    
    Task<IdentityRolePolicyMappingDetail> GetByIdAsync(Guid id);

    Task<bool> IsRolePolicyMappingExistsAsync(Guid policy_id, Guid role_id);

    Task CreateAsync(IdentityRolePolicyMapping dto);
    
    Task UpdateAsync(IdentityRolePolicyMapping dto);
    
    Task DeleteAsync(Guid id);
}

public class IdentityRolePolicyDataLayerService : IIdentityRolePolicyDataLayerService
{
    private readonly IDbContextFactory<IAMApplicationDbContext> _factory;
    private readonly IAMApplicationDbContext _dbContext;
    private readonly ICascadeDeleteManager _cascadeDeleteManager;
    private readonly ILogger<IdentityRolePolicyDataLayerService> _logger;

    public IdentityRolePolicyDataLayerService(IDbContextFactory<IAMApplicationDbContext> factory, ILogger<IdentityRolePolicyDataLayerService> logger, ICascadeDeleteManager cascadeDeleteManager)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _dbContext = _factory.CreateDbContext();
        _logger = logger;
        _cascadeDeleteManager = cascadeDeleteManager;
    }

    public async Task<PaginatedList<IdentityRolePolicyMappingList>> GetAllAsync(IdentityRolePolicyMapping dto, int index, int size)
    {
        return await _dbContext.GetRolePolicyListAsync(dto, index, size);
    }

    public async Task<IdentityRolePolicyMappingDetail> GetByIdAsync(Guid id)
    {
        return await _dbContext.GetRolePolicyByIdAsync(id);
    }

    public async Task<bool> IsRolePolicyMappingExistsAsync(Guid policy_id, Guid role_id)
    {
        return await _dbContext.IsRolePolicyMappingExistsAsync(policy_id, role_id);
    }

    public async Task CreateAsync(IdentityRolePolicyMapping dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.CreateRolePolicy(dto);
            await _dbContext.CommitTransactionAsync(transaction!);
        }
        catch (Exception ex)
        {
            _dbContext.RollbackTransaction();
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(IdentityRolePolicyMapping dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.UpdateRolePolicy(dto);
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
            _dbContext.DeleteRolePolicy(id);
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
