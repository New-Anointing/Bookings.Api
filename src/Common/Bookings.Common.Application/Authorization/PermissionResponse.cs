namespace Bookings.Common.Application.Authorization;

public sealed record PermissionResponse(Guid USerId, HashSet<string> Permissions);
