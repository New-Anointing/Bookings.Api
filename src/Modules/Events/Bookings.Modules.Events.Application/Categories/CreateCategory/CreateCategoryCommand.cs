using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Events.Application.Categories.CreateCategory;

public sealed record CreateCategoryCommand(string Name) : ICommand<Guid>;
