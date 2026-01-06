using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Visitor.Module.IAM.Application.DecoratorServices.ValidatorServices;

public class CreatePageCommandValidator : AbstractValidator<CreatePageCommand>
{
    private readonly IIdentityPageBusinessService _businessService;
    private readonly IIdentityModuleBusinessService _moduleBusinessService;

    public CreatePageCommandValidator(IIdentityPageBusinessService businessService, IIdentityModuleBusinessService moduleBusinessService)
    {
        _businessService = businessService;
        _moduleBusinessService = moduleBusinessService;

        RuleFor(x => x.Page_Nm)
            .NotEmpty().WithMessage("Page name is required");
            //.MaximumLength(100).WithMessage("Page name cannot exceed 100 characters");

        RuleFor(x => x.Page_Title)
            .NotEmpty().WithMessage("Page title is required");

        RuleFor(x => x.Page_Url)
            .NotEmpty().WithMessage("Page URL is required");

        RuleFor(x => x.Icon)
            .NotEmpty().WithMessage("Icon is required")
            .When(x => x.Page_Level == 1);

        RuleFor(x => x.Module_Nm)
            .NotEmpty().WithMessage("Module name is required");

        RuleFor(x => x.Page_Level)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page level must start from 1 or higher.");

        RuleFor(x => x.Page_Nm)
            .MustAsync(async (pageName, cancellation) =>
            {
                var existing = await _businessService.GetByNameAsync(pageName);
                return existing is null;
            }).WithMessage(x => $"Page '{x.Page_Nm}' already exists.");

        RuleFor(x => x.Parent_Id)
            .MustAsync(async (parentId, cancellation) =>
            {
                return await _businessService.ParentIdExistAsync(parentId);
            })
            .WithMessage(x => $"Parent page with ID '{x.Parent_Id}' does not exist.")
            .When(x => x.Page_Level > 1);

        RuleFor(x => x)
            .MustAsync(async (command, cancellation) =>
            {
                if (command.Page_Level <= 1 || command.Parent_Id == Guid.Empty)
                    return true;

                var parentPage = await _businessService.GetByIdAsync(command.Parent_Id);
                if (parentPage == null)
                    return true; // skip; handled in previous rule

                return command.Page_Level > parentPage.page_Level;
            })
            .WithMessage(x =>
                $"Page level ({x.Page_Level}) must be greater than parent’s level for Parent ID {x.Parent_Id}.")
            .When(x => x.Page_Level > 1);

        RuleFor(x => x.Module_Nm)
            .MustAsync(async (moduleName, cancellation) =>
            {
                var module = await _moduleBusinessService.GetByNameAsync(moduleName);
                return module is not null;
            }).WithMessage(x => $"Module '{x.Module_Nm}' does not exist.");
    }
}

public class UpdatePageCommandValidator : AbstractValidator<UpdatePageCommand>
{
    private readonly IIdentityPageBusinessService _businessService;
    private readonly IIdentityModuleBusinessService _moduleBusinessService;

    public UpdatePageCommandValidator(IIdentityPageBusinessService businessService, IIdentityModuleBusinessService moduleBusinessService)
    {
        _businessService = businessService;
        _moduleBusinessService = moduleBusinessService;

        RuleFor(x => x.Page_Title)
            .NotEmpty().WithMessage("Page title is required");

        RuleFor(x => x.Page_Url)
            .NotEmpty().WithMessage("Page URL is required");

        RuleFor(x => x.Icon)
            .NotEmpty().WithMessage("Icon is required")
            .When(x => x.Page_Level == 1);

        RuleFor(x => x.Page_Level)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page level must start from 1 or higher.");

        RuleFor(x => x.Parent_Id)
            .MustAsync(async (parentId, cancellation) =>
            {
                return await _businessService.ParentIdExistAsync(parentId);
            })
            .WithMessage(x => $"Parent page with ID '{x.Parent_Id}' does not exist.")
            .When(x => x.Page_Level > 1);

        RuleFor(x => x)
            .MustAsync(async (command, cancellation) =>
            {
                if (command.Page_Level <= 1 || command.Parent_Id == Guid.Empty)
                    return true;

                var parentPage = await _businessService.GetByIdAsync(command.Parent_Id);
                if (parentPage == null)
                    return true; // skip; handled in previous rule

                return command.Page_Level > parentPage.page_Level;
            })
            .WithMessage(x =>
                $"Page level ({x.Page_Level}) must be greater than parent’s level for Parent ID {x.Parent_Id}.")
            .When(x => x.Page_Level > 1);
    }
}