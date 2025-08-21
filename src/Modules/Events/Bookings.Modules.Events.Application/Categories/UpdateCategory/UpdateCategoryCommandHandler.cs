using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Modules.Events.Application.Abstractions.Messaging;
using Bookings.Modules.Events.Domain.Abstractions;
using Bookings.Modules.Events.Domain.Categories;

namespace Bookings.Modules.Events.Application.Categories.UpdateCategory;
internal sealed class UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateCategoryCommand>
{
    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        Category? category = await categoryRepository.GetAsync(command.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.Failure(CategoryErrors.NotFound(command.CategoryId));
        }

        category.ChangeName(command.Name);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
