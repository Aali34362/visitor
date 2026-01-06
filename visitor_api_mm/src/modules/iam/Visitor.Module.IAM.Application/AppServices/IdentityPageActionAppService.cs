namespace Visitor.Module.IAM.Application.AppServices;

public interface IIdentityPageActionAppService
{
    Task<Result<PaginatedList<IdentityPageActionList>>> GetAllAsync(GetPageActionListQuery query);
    
    Task<Result<IdentityPageActionDetail>> GetByIdAsync(GetPageActionQuery query);
    
    Task<Result<bool>> CreateAsync(CreatePageActionCommand command);
    
    Task<Result<bool>> UpdateAsync(UpdatePageActionCommand command);

    Task<Result<bool>> DeleteAsync(DeletePageActionCommand command);
}

public class IdentityPageActionAppService : IIdentityPageActionAppService
{
    private readonly IIdentityPageActionBusinessService _businessService;
    private readonly IIdentityPageBusinessService _pageBusinessService;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;

    public IdentityPageActionAppService(IIdentityPageActionBusinessService businessService, IIdentityPageBusinessService pageBusinessService, IValidationService validationService, IMapper mapper)
    {
        _businessService = businessService;
        _pageBusinessService = pageBusinessService;
        _validationService = validationService;
        _mapper = mapper;
    }
    
    public async Task<Result<bool>> CreateAsync(CreatePageActionCommand command)
    {
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Error);

        var page = await _pageBusinessService.GetByNameAsync(command.Page_Nm);

        var PageAction = _mapper.Map<CreatePageActionCommand, IdentityPageAction>(command);
        PageAction.page_Id = page.id;

        return await _businessService.CreateAsync(PageAction);
    }
    
    public async Task<Result<bool>> UpdateAsync(UpdatePageActionCommand command)
    {
        var validationResult = await _validationService.ValidateAsync(command);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Error);

        var data = await _businessService.GetByIdAsync(command.Id);
        if (data is null)
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {command.Id!}", propertyName: nameof(IdentityPage)));

        // Re-fetch Module by name to get the ID
        var page = await _pageBusinessService.GetByNameAsync(command.Page_Nm);

        var PageAction = _mapper.Map<UpdatePageActionCommand, IdentityPageAction>(command);
        PageAction.page_Id = page.id;
        PageAction.name = data.name; // Preserve the original name if not changed
        PageAction.created_At = data.updated_At; // Preserve the original creation date
        PageAction.created_By = data.updated_By; // Preserve the original creator

        return await _businessService.UpdateAsync(PageAction);
    }
    
    public async Task<Result<bool>> DeleteAsync(DeletePageActionCommand command)
    {
        var data = await _businessService.GetByIdAsync(command.Id);
        if (data is null)
            return Result<bool>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {command.Id!}", propertyName: nameof(IdentityPage)));

        return await _businessService.DeleteAsync(command.Id);
    }

    public async Task<Result<PaginatedList<IdentityPageActionList>>> GetAllAsync(GetPageActionListQuery query)
    {
        var Page = _mapper.Map<GetPageActionListQuery, IdentityPageAction>(query);

        if (!string.IsNullOrEmpty(query.Page_Nm))
        { 
            var page = await _pageBusinessService.GetByNameAsync(query.Page_Nm);
            if (page is null)
                return Result<PaginatedList<IdentityPageActionList>>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {query.Page_Nm} ", propertyName: nameof(IdentityPageAction)));
            Page.page_Id = page.id;
        }

        var data = await _businessService.GetAllAsync(Page, query.index, query.size);
        if (data is null)
            return Result<PaginatedList<IdentityPageActionList>>.Failure(ErrorDetail.Business(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} ", propertyName: nameof(IdentityPageAction)));
        
        return Result<PaginatedList<IdentityPageActionList>>.Success(data);
    }
    
    public async Task<Result<IdentityPageActionDetail>> GetByIdAsync(GetPageActionQuery query)
    {
        var data = await _businessService.GetByIdAsync(query.Id);
        if (data is null)
            return Result<IdentityPageActionDetail>.Failure(ErrorDetail.NotFound(errorMessage: $"{CustomMessages.RECORD_NOT_FOUND} : {query.Id} ", propertyName: nameof(IdentityPage)));

        return Result<IdentityPageActionDetail>.Success(data);
    }
}
