using FluentValidation;

namespace Bookings.Modules.Events.Application.Categories.ArchiveCategory;

internal sealed class ArchiveCategoryCommandValidator : AbstractValidator<ArchiveCategoryCommand> 
{
    public ArchiveCategoryCommandValidator()
    {
        RuleFor(c => c.CategoryId).NotEmpty();
    }
}
