using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Events.Application.Categories.UpdateCategory;

public sealed record UpdateCategoryCommand(Guid CategoryId, string Name) : ICommand;
