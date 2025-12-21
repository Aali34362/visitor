namespace Visitor.Module.IAM.Application.DataLayerServices;

public interface IIdentityUserRoleDataLayerService
{
    Task<PaginatedList<IdentityUserRoleMappingList>> GetAllAsync(IdentityUserRoleMapping dto, int index, int size);
    
    Task<IdentityUserRoleMappingDetail> GetByIdAsync(Guid id);

    Task<bool> IsUserRoleMappingExistsAsync(Guid policy_id, Guid role_id);

    Task CreateAsync(IdentityUserRoleMapping dto);
    
    Task UpdateAsync(IdentityUserRoleMapping dto);
    
    Task DeleteAsync(Guid id);
}

public class IdentityUserRoleDataLayerService : IIdentityUserRoleDataLayerService
{
    private readonly IDbContextFactory<IAMApplicationDbContext> _factory;
    private readonly IAMApplicationDbContext _dbContext;
    private readonly ICascadeDeleteManager _cascadeDeleteManager;
    private readonly ILogger<IdentityUserRoleDataLayerService> _logger;

    public IdentityUserRoleDataLayerService(IDbContextFactory<IAMApplicationDbContext> factory, ILogger<IdentityUserRoleDataLayerService> logger, ICascadeDeleteManager cascadeDeleteManager)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _dbContext = _factory.CreateDbContext();
        _logger = logger;
        _cascadeDeleteManager = cascadeDeleteManager;
    }

    public async Task<PaginatedList<IdentityUserRoleMappingList>> GetAllAsync(IdentityUserRoleMapping dto, int index, int size)
    {
        return await _dbContext.GetUserRoleListAsync(dto, index, size);
    }

    public async Task<IdentityUserRoleMappingDetail> GetByIdAsync(Guid id)
    {
        return await _dbContext.GetUserRoleByIdAsync(id);
    }

    public async Task<bool> IsUserRoleMappingExistsAsync(Guid user_id, Guid role_id)
    {
        return await _dbContext.IsUserRoleMappingExistsAsync(user_id, role_id);
    }

    public async Task CreateAsync(IdentityUserRoleMapping dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.CreateUserRole(dto);
            await _dbContext.CommitTransactionAsync(transaction!);
        }
        catch (Exception ex)
        {
            _dbContext.RollbackTransaction();
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(IdentityUserRoleMapping dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.UpdateUserRole(dto);
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
            _dbContext.DeleteUserRole(id);
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
