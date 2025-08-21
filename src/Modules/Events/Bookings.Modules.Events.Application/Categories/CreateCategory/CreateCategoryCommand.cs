using Bookings.Modules.Events.Application.Abstractions.Messaging;

namespace Bookings.Modules.Events.Application.Categories.CreateCategory;

public sealed record CreateCategoryCommand(string Name): ICommand<Guid>;
