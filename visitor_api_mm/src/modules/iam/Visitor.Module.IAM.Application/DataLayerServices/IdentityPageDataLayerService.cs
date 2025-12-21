namespace Visitor.Module.IAM.Application.DataLayerServices;

public interface IIdentityPageDataLayerService
{
    Task<PaginatedList<IdentityPageList>> GetAllAsync(IdentityPage dto, int index, int size);
    
    Task<IdentityPageDetail> GetByIdAsync(Guid id);
    
    Task<IdentityPage> GetByNameAsync(string Page_Nm);

    Task<bool> ParentIdExistAsync(Guid Parent_Id);
    
    Task CreateAsync(IdentityPage dto);
    
    Task UpdateAsync(IdentityPage dto);
    
    Task DeleteAsync(Guid id);
}

public class IdentityPageDataLayerService : IIdentityPageDataLayerService
{
    private readonly IDbContextFactory<IAMApplicationDbContext> _factory;
    private readonly ICascadeDeleteManager _cascadeDeleteManager;
    private readonly IAMApplicationDbContext _dbContext;
    private readonly ILogger<IdentityPageDataLayerService> _logger;

    public IdentityPageDataLayerService(IDbContextFactory<IAMApplicationDbContext> factory, ILogger<IdentityPageDataLayerService> logger, ICascadeDeleteManager cascadeDeleteManager)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _dbContext = _factory.CreateDbContext();
        _logger = logger;
        _cascadeDeleteManager = cascadeDeleteManager;
    }

    public async Task<PaginatedList<IdentityPageList>> GetAllAsync(IdentityPage dto, int index, int size)
    {
        return await _dbContext.GetPageListAsync(dto, index, size);
    }

    public async Task<IdentityPageDetail> GetByIdAsync(Guid id)
    {
        return await _dbContext.GetPageByIdAsync(id);
    }

    public async Task<IdentityPage> GetByNameAsync(string Page_Nm)
    {
        return await _dbContext.GetPageByNameAsync(Page_Nm);
    }

    public async Task<bool> ParentIdExistAsync(Guid Parent_Id)
    {
        return await _dbContext.ParentIdExistAsync(Parent_Id);
    }

    public async Task CreateAsync(IdentityPage dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.CreatePage(dto);
            await _dbContext.CommitTransactionAsync(transaction!);
        }
        catch (Exception ex)
        {
            _dbContext.RollbackTransaction();
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(IdentityPage dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.UpdatePage(dto);
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
