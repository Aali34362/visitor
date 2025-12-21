namespace Visitor.Module.IAM.Application.DataLayerServices;

public interface IIdentityUserDataLayerService
{
    Task<PaginatedList<IdentityUserList>> GetAllAsync(IdentityUser dto, int index, int size);
    
    Task<IdentityUserDetail> GetByIdAsync(Guid id);
    
    Task<IdentityUser> GetByNameAsync(string User_Nm);

    Task<IdentityUser> ValidateUserPasswordAsync(string User_Nm, string password);

    Task CreateAsync(IdentityUser dto);
    
    Task UpdateAsync(IdentityUser dto);
    
    Task DeleteAsync(Guid id);

    Task<bool> emailExistsAsync(string email);
}

public class IdentityUserDataLayerService : IIdentityUserDataLayerService
{
    private readonly IDbContextFactory<IAMApplicationDbContext> _factory;
    private readonly IAMApplicationDbContext _dbContext;
    private readonly ICascadeDeleteManager _cascadeDeleteManager;
    private readonly ILogger<IdentityUserDataLayerService> _logger;

    public IdentityUserDataLayerService(IDbContextFactory<IAMApplicationDbContext> factory, ILogger<IdentityUserDataLayerService> logger, ICascadeDeleteManager cascadeDeleteManager)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _dbContext = _factory.CreateDbContext();
        _logger = logger;
        _cascadeDeleteManager = cascadeDeleteManager;
    }

    public async Task<PaginatedList<IdentityUserList>> GetAllAsync(IdentityUser dto, int index, int size)
    {
        return await _dbContext.GetUserListAsync(dto, index, size);
    }

    public async Task<IdentityUserDetail> GetByIdAsync(Guid id)
    {
        return await _dbContext.GetUserByIdAsync(id);
    }

    public async Task<IdentityUser> GetByNameAsync(string User_Nm)
    {
        return await _dbContext.GetUserByNameAsync(User_Nm);
    }

    public async Task<IdentityUser> ValidateUserPasswordAsync(string User_Nm, string password) =>
        await _dbContext.ValidateUserPasswordAsync(User_Nm, password);

    public async Task CreateAsync(IdentityUser dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.CreateUser(dto);
            await _dbContext.CommitTransactionAsync(transaction!);
        }
        catch (Exception ex)
        {
            _dbContext.RollbackTransaction();
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(IdentityUser dto)
    {
        try
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            _dbContext.UpdateUser(dto);
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
            await _cascadeDeleteManager.DeactivateUserAsync(id);
            await _dbContext.CommitTransactionAsync(transaction!);
        }
        catch (Exception ex)
        {
            _dbContext.RollbackTransaction();
            _logger.Log(LogLevel.Error, $"{ex.Message}");
            throw;
        }
    }

    public async Task<bool> emailExistsAsync(string email)
    {
        return await _dbContext.emailExistsAsync(email);
    }
}
