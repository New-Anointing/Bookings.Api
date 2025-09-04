using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Modules.Events.Domain.Categories;

namespace Bookings.Modules.Events.Application.Categories.CreateCategory;

internal sealed class CreateCategoryCommandHandler
    (ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateCategoryCommand, Guid>

{
    public async Task<Result<Guid>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = Category.Create(command.Name);

        categoryRepository.Insert(category);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}
