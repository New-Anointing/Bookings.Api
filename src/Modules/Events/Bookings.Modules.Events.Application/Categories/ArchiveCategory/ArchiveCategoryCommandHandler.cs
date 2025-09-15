using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Modules.Events.Domain.Categories;

namespace Bookings.Modules.Events.Application.Categories.ArchiveCategory;

internal sealed class ArchiveCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : ICommandHandler<ArchiveCategoryCommand>
{
    public async Task<Result> Handle(ArchiveCategoryCommand command, CancellationToken cancellationToken)
    {
        Category? category = await categoryRepository.GetAsync(command.CategoryId, cancellationToken);
        if (category is null)
        {
            return Result.Failure(CategoryErrors.NotFound(command.CategoryId));
        }
        if (category.IsArchived)
        {
            return Result.Failure(CategoryErrors.AlreadyArchived);
        }

        category.Archive();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();

    }
}
