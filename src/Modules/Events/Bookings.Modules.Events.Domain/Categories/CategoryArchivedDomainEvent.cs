using Bookings.Modules.Events.Domain.Abstractions;

namespace Bookings.Modules.Events.Domain.Categories;
public sealed partial class Category
{
    public sealed class CategoryArchivedDomainEvent(Guid categoryId) : DomainEvent
    {
        public Guid CategoryId { get; init; } = categoryId;
    }

}
