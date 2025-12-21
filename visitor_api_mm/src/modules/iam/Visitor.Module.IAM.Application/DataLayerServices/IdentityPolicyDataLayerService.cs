namespace Visitor.Module.IAM.Application.DataLayerServices;

public interface IIdentityPolicyDataLayerService
{
    Task<PaginatedList<IdentityPolicyList>> GetAllAsync(IdentityPolicy dto, int index, int size);
    
    Task<IdentityPolicyDetail> GetByIdAsync(Guid id);
    
    Task<IdentityPolicy> GetByNameAsync(string Policy_Nm);
        
    Task CreateAsync(IdentityPolicy dto);
    
    Task UpdateAsync(IdentityPolicy dto);
    
    Task DeleteAsync(Guid id);
}

public class IdentityPolicyDataLayerService : IIdentityPolicyDataLayerService
{
    private readonly IDbContextFactory<IAMApplicationDbContext> _factory;
    private readonly IAMApplicationDbContext _dbContext;
    private readonly ICascadeDeleteManager _cascadeDeleteManager;
    private readonly ILogger<IdentityPolicyDataLayerService> _logger;

    public IdentityPolicyDataLayerService(IDbContextFactory<IAMApplicationDbContext> factory, ILogger<IdentityPolicyDataLayerService> logger, ICascadeDeleteManager cascadeDeleteManager)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _dbContext = _factory.CreateDbContext();
        _logger = logger;
        _cascadeDeleteManager = cascadeDeleteManager;
    }

    public async Task<PaginatedList<IdentityPolicyList>> GetAllAsync(IdentityPolicy dto, int index, int size)
    {
        return await _dbContext.GetPolicyListAsync(dto, index, size);
    }

    public async Task<IdentityPolicyDetail> GetByIdAsync(Guid id)
    {
        return await _dbContext.GetPolicyByIdAsync(id);
    }

    public async Task<IdentityPolicy> GetByNameAsync(string Policy_Nm)
    {
        return await _dbContext.GetPolicyByNameAsync(Policy_Nm);
    }

    public async Task CreateAsync(IdentityPolicy dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.CreatePolicy(dto);
            await _dbContext.CommitTransactionAsync(transaction!);
        }
        catch (Exception ex)
        {
            _dbContext.RollbackTransaction();
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(IdentityPolicy dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.UpdatePolicy(dto);
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
            await _cascadeDeleteManager.DeactivatePolicyAsync(id);
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
