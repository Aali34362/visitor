using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Visitor.Module.IAM.Application.AppServices;

public interface IIdentityPageAppService
{
    Task<Result<PaginatedList<IdentityPageList>>> GetAllAsync(GetPageListQuery query);
    
    Task<Result<IdentityPageDetail>> GetByIdAsync(GetPageQuery query);
    
    Task<Result<bool>> CreateAsync(CreatePageCommand command);
    
    Task<Result<bool>> UpdateAsync(UpdatePageCommand command);

    Task<Result<bool>> DeleteAsync(DeletePageCommand command);
}

public class IdentityPageAppService : IIdentityPageAppService
{
    private readonly IIdentityPageBusinessService _businessService;
    private readonly IIdentityModuleBusinessService _moduleBusinessService;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;

    public IdentityPageAppService(IIdentityPageBusinessService businessService, IIdentityModuleBusinessService moduleBusinessService, IValidationService validationService, IMapper mapper)
    {
        _businessService = businessService;
        _moduleBusinessService = moduleBusinessService;
        _validationService = validationService;
        _mapper = mapper;
    }
    
    public async Task<Result<bool>> CreateAsync(CreatePageCommand command)
    {
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Error);

        // Re-fetch Module by name to get the ID
        var module = await _moduleBusinessService.GetByNameAsync(command.Module_Nm);

        var Page = _mapper.Map<CreatePageCommand, IdentityPage>(command);
        Page.module_Id = module.id;
        if(Page.page_Level == 1)
            Page.parent_Id = Guid.Empty; // Set Parent_Id to null for top-level pages

        return await _businessService.CreateAsync(Page);
    }
    
    public async Task<Result<bool>> UpdateAsync(UpdatePageCommand command)
    {
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Error);

        var data = await _businessService.GetByIdAsync(command.Id);
        if (data is null)
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {command.Id!}", propertyName: nameof(IdentityPage)));

        // Re-fetch Module by name to get the ID
        var module = await _moduleBusinessService.GetByNameAsync(data.module_Nm);

        var Page = _mapper.Map<UpdatePageCommand, IdentityPage>(command);
        Page.module_Id = module.id;
        Page.page_Nm = data.page_Nm; // Preserve the original name if not changed
        Page.created_At = data.updated_At; // Preserve the original creation date
        Page.created_By = data.updated_By; // Preserve the original creator

        return await _businessService.UpdateAsync(Page);
    }
    
    public async Task<Result<bool>> DeleteAsync(DeletePageCommand command)
    {
        var data = await _businessService.GetByIdAsync(command.Id);
        if (data is null)
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {command.Id!}", propertyName: nameof(IdentityPage)));

        return await _businessService.DeleteAsync(command.Id);
    }

    public async Task<Result<PaginatedList<IdentityPageList>>> GetAllAsync(GetPageListQuery query)
    {
        var Page = _mapper.Map<GetPageListQuery, IdentityPage>(query);

        if (!string.IsNullOrEmpty(query.Module_Nm))
        { 
            var module = await _moduleBusinessService.GetByNameAsync(query.Module_Nm);
            if (module is null)
                return Result<PaginatedList<IdentityPageList>>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {query.Module_Nm} ", propertyName: nameof(IdentityModule)));
            Page.module_Id = module.id;
        }

        var data = await _businessService.GetAllAsync(Page, query.index, query.size);
        if (data is null)
            return Result<PaginatedList<IdentityPageList>>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} ", propertyName: nameof(IdentityPage)));
        
        return Result<PaginatedList<IdentityPageList>>.Success(data);
    }
    
    public async Task<Result<IdentityPageDetail>> GetByIdAsync(GetPageQuery query)
    {
        var data = await _businessService.GetByIdAsync(query.Id);
        if (data is null)
            return Result<IdentityPageDetail>.Failure(ErrorDetail.NotFound(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {query.Id} ", propertyName: nameof(IdentityPage)));

        return Result<IdentityPageDetail>.Success(data);
    }
}
