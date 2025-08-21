using Bookings.Modules.Events.Application.Abstractions.Messaging;

namespace Bookings.Modules.Events.Application.Categories.GetCategory;

public sealed record GetCategoryQuery(Guid CategoryId) : IQuery<CategoryResponse>;


