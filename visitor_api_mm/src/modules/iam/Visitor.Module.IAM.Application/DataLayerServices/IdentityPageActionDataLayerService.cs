namespace Visitor.Module.IAM.Application.DataLayerServices;

public interface IIdentityPageActionDataLayerService
{
    Task<PaginatedList<IdentityPageActionList>> GetAllAsync(IdentityPageAction dto, int index, int size);

    Task<List<IdentityPageActionList>> GetListAsync(IdentityPageAction dto);

    Task<IdentityPageActionDetail> GetByIdAsync(Guid id);
    
    Task<IdentityPageAction> GetByNameAsync(string PageAction_Nm);

    Task<bool> ParentIdExistAsync(Guid Parent_Id);
    
    Task CreateAsync(IdentityPageAction dto);
    
    Task UpdateAsync(IdentityPageAction dto);
    
    Task DeleteAsync(Guid id);
}

public class IdentityPageActionDataLayerService : IIdentityPageActionDataLayerService
{
    private readonly IDbContextFactory<IAMApplicationDbContext> _factory;
    private readonly ICascadeDeleteManager _cascadeDeleteManager;
    private readonly IAMApplicationDbContext _dbContext;
    private readonly ILogger<IdentityPageActionDataLayerService> _logger;

    public IdentityPageActionDataLayerService(IDbContextFactory<IAMApplicationDbContext> factory, ILogger<IdentityPageActionDataLayerService> logger, ICascadeDeleteManager cascadeDeleteManager)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _dbContext = _factory.CreateDbContext();
        _logger = logger;
        _cascadeDeleteManager = cascadeDeleteManager;
    }

    public async Task<PaginatedList<IdentityPageActionList>> GetAllAsync(IdentityPageAction dto, int index, int size)
    {
        return await _dbContext.GetPageActionListAsync(dto, index, size);
    }

    public Task<List<IdentityPageActionList>> GetListAsync(IdentityPageAction dto)
    {
        return _dbContext.GetPageActionListAsync(dto);
    }

    public async Task<IdentityPageActionDetail> GetByIdAsync(Guid id)
    {
        return await _dbContext.GetPageActionByIdAsync(id);
    }

    public async Task<IdentityPageAction> GetByNameAsync(string PageAction_Nm)
    {
        return await _dbContext.GetPageActionByNameAsync(PageAction_Nm);
    }

    public async Task<bool> ParentIdExistAsync(Guid Parent_Id)
    {
        return await _dbContext.ParentIdExistAsync(Parent_Id);
    }

    public async Task CreateAsync(IdentityPageAction dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.CreatePageAction(dto);
            await _dbContext.CommitTransactionAsync(transaction!);
        }
        catch (Exception ex)
        {
            _dbContext.RollbackTransaction();
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(IdentityPageAction dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.UpdatePageAction(dto);
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
            await _cascadeDeleteManager.DeactivatePageActionAsync(id);
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
