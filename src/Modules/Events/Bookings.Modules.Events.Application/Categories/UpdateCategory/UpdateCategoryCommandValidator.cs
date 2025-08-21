using FluentValidation;

namespace Bookings.Modules.Events.Application.Categories.UpdateCategory;

internal sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand> 
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(c => c.CategoryId).NotEmpty();
        RuleFor(c => c.Name).NotEmpty();
    }
}
