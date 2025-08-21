using Bookings.Modules.Events.Application.Abstractions.Messaging;
using Bookings.Modules.Events.Application.Categories.GetCategory;

namespace Bookings.Modules.Events.Application.Categories.GetCategories;

public sealed record GetCategoriesQuery : IQuery<IReadOnlyCollection<CategoryResponse>>;
