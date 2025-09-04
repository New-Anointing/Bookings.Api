using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Events.Application.Categories.ArchiveCategory;

public sealed record ArchiveCategoryCommand(Guid CategoryId) : ICommand;
