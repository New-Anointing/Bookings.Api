using Bookings.Modules.Events.Application.Abstractions.Messaging;

namespace Bookings.Modules.Events.Application.Categories.ArchiveCategory;

public sealed record ArchiveCategoryCommand(Guid CategoryId) : ICommand;
