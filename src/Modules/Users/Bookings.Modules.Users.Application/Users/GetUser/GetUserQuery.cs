using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Users.Application.Users.GetUser;

public sealed record GetUserQuery(Guid UserId) : IQuery<UserResponse>;
