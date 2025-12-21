namespace Visitor.Module.IAM.Application.DecoratorServices.ValidatorServices;

public class CreatePageActionCommandValidator : AbstractValidator<CreatePageActionCommand>
{
    private readonly IIdentityPageActionBusinessService _businessService;
    private readonly IIdentityPageBusinessService _pageBusinessService;

    public CreatePageActionCommandValidator(IIdentityPageActionBusinessService businessService, IIdentityPageBusinessService pageBusinessService)
    {
        _businessService = businessService;
        _pageBusinessService = pageBusinessService;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Action name is required.");

        RuleFor(x => x.Page_Nm)
            .NotEmpty().WithMessage("Page name is required.");

        RuleFor(x => x.Action)
            .NotEmpty().WithMessage("Action is required.");

        RuleFor(x => x.AccessLevel)
            .NotEmpty().WithMessage("Access level is required.");

        RuleFor(x => x.PageUrl)
            .NotEmpty().WithMessage("Page URL is required.");

        RuleFor(x => x.Page_Nm)
            .MustAsync(async (pageName, cancellation) =>
            {
                var page = await _pageBusinessService.GetByNameAsync(pageName);
                return page is not null;
            }).WithMessage(x => $"Page '{x.Page_Nm}' does not exist.");

        RuleFor(x => x)
               .MustAsync(async (command, cancellation) =>
               {
                   Guid Page_Id = Guid.Empty;
                   if (!string.IsNullOrEmpty(command.Page_Nm))
                   {
                       var page = await _pageBusinessService.GetByNameAsync(command.Page_Nm);
                       if (page is null)
                           return true;
                       Page_Id = page.Id;
                   }
                   var existing = await _businessService.GetListAsync(new() { Page_Id = Page_Id, Action = command.Action, Name = command.Name });

                   return existing is null || !existing.Any();
               })
               .WithMessage(x => $"An action with name '{x.Name}' and action '{x.Action}' already exists for page '{x.Page_Nm}'.");
    }
}

public class UpdatePageActionCommandValidator : AbstractValidator<UpdatePageActionCommand>
{
    private readonly IIdentityPageActionBusinessService _businessService;
    private readonly IIdentityPageBusinessService _pageBusinessService;

    public UpdatePageActionCommandValidator(IIdentityPageActionBusinessService businessService, IIdentityPageBusinessService pageBusinessService)
    {
        _businessService = businessService;
        _pageBusinessService = pageBusinessService;

        RuleFor(x => x.Page_Nm)
            .NotEmpty().WithMessage("Page name is required.");

        RuleFor(x => x.Action)
            .NotEmpty().WithMessage("Action is required.");

        RuleFor(x => x.AccessLevel)
            .NotEmpty().WithMessage("Access level is required.");

        RuleFor(x => x.PageUrl)
            .NotEmpty().WithMessage("Page URL is required.");

        RuleFor(x => x.Page_Nm)
            .MustAsync(async (pageName, cancellation) =>
            {
                var page = await _pageBusinessService.GetByNameAsync(pageName);
                return page is not null;
            }).WithMessage(x => $"Page '{x.Page_Nm}' does not exist.");
    }
}