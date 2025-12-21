using Visitor.Core.Domain.Configurations;
using Visitor.Module.IAM.Application.DataLayerServices.Contexts;

namespace Visitor.Module.IAM.Application.DecoratorServices.CascadeManager;

public interface ICascadeDeleteManager
{
    Task DeactivateModuleAsync(Guid moduleId);
    Task DeactivatePageAsync(Guid pageId);
    Task DeactivatePageActionAsync(Guid pageActionId);
    Task DeactivatePolicyAsync(Guid moduleId);
    Task DeactivateRoleAsync(Guid roleId);
    Task DeactivateUserAsync(Guid userId);
}

public class CascadeDeleteManager : ICascadeDeleteManager
{
    private readonly IDbContextFactory<IAMApplicationDbContext> _factory;
    private readonly IAMApplicationDbContext _dbContext;
    private readonly ILogger<CascadeDeleteManager> _logger;
    public CascadeDeleteManager(IDbContextFactory<IAMApplicationDbContext> factory, ILogger<CascadeDeleteManager> logger)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _dbContext = _factory.CreateDbContext();
        _logger = logger;
    }

    public async Task DeactivateModuleAsync(Guid moduleId)
    {
        try
        {
            var pageIds = await _dbContext.IdentityPage
                .Where(p => p.Module_Id == moduleId)
                .Select(p => p.Id)
                .ToListAsync();

            foreach (var pageId in pageIds)
                await DeactivatePageAsync(pageId);

            await _dbContext.IdentityModule
                .Where(m => m.Id == moduleId)
                .ExecuteUpdateAsync(m => m
                    .SetProperty(x => x.Act_Ind, 0)
                    .SetProperty(x => x.IsDeleted, true)
                    .SetProperty(x => x.UpdatedAt, BaseService.GetLocalNow())
                    .SetProperty(x => x.UpdatedBy, BaseService.UserInfo().UserName)
                );
        }
        catch (Exception ex)
        {
            _dbContext.RollbackTransaction();
            _logger.LogError(ex, $"Failed to deactivate module: {moduleId}");
            throw;
        }
    }

    public async Task DeactivatePageAsync(Guid pageId)
    {
        var pageActionIds = await _dbContext.IdentityPageAction
            .Where(pa => pa.Page_Id == pageId)
            .Select(pa => pa.Id)
            .ToListAsync();

        foreach (var actionId in pageActionIds)
            await DeactivatePageActionAsync(actionId);

        await _dbContext.IdentityPage
            .Where(p => p.Id == pageId)
            .ExecuteUpdateAsync(p => p
                .SetProperty(x => x.Act_Ind, 0)
                .SetProperty(x => x.IsDeleted, true)
                .SetProperty(x => x.UpdatedAt, BaseService.GetLocalNow())
                .SetProperty(x => x.UpdatedBy, BaseService.UserInfo().UserName)
            );
    }

    public async Task DeactivatePageActionAsync(Guid pageActionId)
    {
        await _dbContext.IdentityPolicyPageActionMapping
            .Where(m => m.PageAction_Id == pageActionId)
            .ExecuteUpdateAsync(m => m
                .SetProperty(x => x.Act_Ind, 0)
                .SetProperty(x => x.IsDeleted, true)
                .SetProperty(x => x.UpdatedAt, BaseService.GetLocalNow())
                .SetProperty(x => x.UpdatedBy, BaseService.UserInfo().UserName)
            );

        await _dbContext.IdentityPageAction
            .Where(p => p.Id == pageActionId)
            .ExecuteUpdateAsync(p => p
                .SetProperty(x => x.Act_Ind, 0)
                .SetProperty(x => x.IsDeleted, true)
                .SetProperty(x => x.UpdatedAt, BaseService.GetLocalNow())
                .SetProperty(x => x.UpdatedBy, BaseService.UserInfo().UserName)
            );
    }

    public async Task DeactivatePolicyAsync(Guid policyId)
    {
        
            await _dbContext.IdentityPolicyPageActionMapping
            .Where(m => m.Policy_Id == policyId)
            .ExecuteUpdateAsync(m => m
                .SetProperty(x => x.Act_Ind, 0)
                .SetProperty(x => x.IsDeleted, true)
                .SetProperty(x => x.UpdatedAt, BaseService.GetLocalNow())
                .SetProperty(x => x.UpdatedBy, BaseService.UserInfo().UserName)
            );
        
            await _dbContext.IdentityRolePolicyMapping
            .Where(m => m.Policy_Id == policyId)
            .ExecuteUpdateAsync(m => m
                .SetProperty(x => x.Act_Ind, 0)
                .SetProperty(x => x.IsDeleted, true)
                .SetProperty(x => x.UpdatedAt, BaseService.GetLocalNow())
                .SetProperty(x => x.UpdatedBy, BaseService.UserInfo().UserName)
            );        

        await _dbContext.IdentityPolicy
            .Where(p => p.Id == policyId)
            .ExecuteUpdateAsync(p => p
                .SetProperty(x => x.Act_Ind, 0)
                .SetProperty(x => x.IsDeleted, true)
                .SetProperty(x => x.UpdatedAt, BaseService.GetLocalNow())
                .SetProperty(x => x.UpdatedBy, BaseService.UserInfo().UserName)
            );
    }

    public async Task DeactivateRoleAsync(Guid roleId)
    {

        await _dbContext.IdentityUserRoleMapping
        .Where(m => m.Role_Id == roleId)
        .ExecuteUpdateAsync(m => m
            .SetProperty(x => x.Act_Ind, 0)
            .SetProperty(x => x.IsDeleted, true)
            .SetProperty(x => x.UpdatedAt, BaseService.GetLocalNow())
            .SetProperty(x => x.UpdatedBy, BaseService.UserInfo().UserName)
        );

        await _dbContext.IdentityRolePolicyMapping
        .Where(m => m.Role_Id == roleId)
        .ExecuteUpdateAsync(m => m
            .SetProperty(x => x.Act_Ind, 0)
            .SetProperty(x => x.IsDeleted, true)
            .SetProperty(x => x.UpdatedAt, BaseService.GetLocalNow())
            .SetProperty(x => x.UpdatedBy, BaseService.UserInfo().UserName)
        );

        await _dbContext.IdentityRole
            .Where(p => p.Id == roleId)
            .ExecuteUpdateAsync(p => p
                .SetProperty(x => x.Act_Ind, 0)
                .SetProperty(x => x.IsDeleted, true)
                .SetProperty(x => x.UpdatedAt, BaseService.GetLocalNow())
                .SetProperty(x => x.UpdatedBy, BaseService.UserInfo().UserName)
            );
    }

    public async Task DeactivateUserAsync(Guid userId)
    {

        await _dbContext.IdentityUserRoleMapping
        .Where(m => m.User_Id == userId)
        .ExecuteUpdateAsync(m => m
            .SetProperty(x => x.Act_Ind, 0)
            .SetProperty(x => x.IsDeleted, true)
            .SetProperty(x => x.UpdatedAt, BaseService.GetLocalNow())
            .SetProperty(x => x.UpdatedBy, BaseService.UserInfo().UserName)
        );

        await _dbContext.IdentityUser
            .Where(p => p.Id == userId)
            .ExecuteUpdateAsync(p => p
                .SetProperty(x => x.Act_Ind, 0)
                .SetProperty(x => x.IsDeleted, true)
                .SetProperty(x => x.UpdatedAt, BaseService.GetLocalNow())
                .SetProperty(x => x.UpdatedBy, BaseService.UserInfo().UserName)
            );
    }
}
