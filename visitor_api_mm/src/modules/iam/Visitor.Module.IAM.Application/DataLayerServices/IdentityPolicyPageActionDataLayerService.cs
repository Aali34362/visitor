namespace Visitor.Module.IAM.Application.DataLayerServices;

public interface IIdentityPolicyPageActionDataLayerService
{
    Task<PaginatedList<IdentityPolicyPageActionMappingList>> GetAllAsync(IdentityPolicyPageActionMapping dto, int index, int size);
    
    Task<IdentityPolicyPageActionMappingDetail> GetByIdAsync(Guid id);

    Task<bool> IsPolicyPageActionMappingExistsAsync(Guid policy_id, Guid pageAction_id);

    Task CreateAsync(IdentityPolicyPageActionMapping dto);
    
    Task UpdateAsync(IdentityPolicyPageActionMapping dto);
    
    Task DeleteAsync(Guid id);
}

public class IdentityPolicyPageActionDataLayerService : IIdentityPolicyPageActionDataLayerService
{
    private readonly IDbContextFactory<IAMApplicationDbContext> _factory;
    private readonly IAMApplicationDbContext _dbContext;
    private readonly ICascadeDeleteManager _cascadeDeleteManager;
    private readonly ILogger<IdentityPolicyPageActionDataLayerService> _logger;

    public IdentityPolicyPageActionDataLayerService(IDbContextFactory<IAMApplicationDbContext> factory, ILogger<IdentityPolicyPageActionDataLayerService> logger, ICascadeDeleteManager cascadeDeleteManager)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _dbContext = _factory.CreateDbContext();
        _logger = logger;
        _cascadeDeleteManager = cascadeDeleteManager;
    }

    public async Task<PaginatedList<IdentityPolicyPageActionMappingList>> GetAllAsync(IdentityPolicyPageActionMapping dto, int index, int size)
    {
        return await _dbContext.GetPolicyPageActionListAsync(dto, index, size);
    }

    public async Task<IdentityPolicyPageActionMappingDetail> GetByIdAsync(Guid id)
    {
        return await _dbContext.GetPolicyPageActionByIdAsync(id);
    }

    public async Task<bool> IsPolicyPageActionMappingExistsAsync(Guid policy_id, Guid pageAction_id)
    {
        return await _dbContext.IsPolicyPageActionMappingExistsAsync(policy_id, pageAction_id);
    }

    public async Task CreateAsync(IdentityPolicyPageActionMapping dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.CreatePolicyPageAction(dto);
            await _dbContext.CommitTransactionAsync(transaction!);
        }
        catch (Exception ex)
        {
            _dbContext.RollbackTransaction();
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(IdentityPolicyPageActionMapping dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.UpdatePolicyPageAction(dto);
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
            _dbContext.DeletePolicyPageAction(id);
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
