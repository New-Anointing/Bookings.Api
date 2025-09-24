using Bookings.Common.Domain;

namespace Bookings.Modules.Users.Application.Abstractions.Identity;

public sealed class IdentityProviderErrors
{
    public static readonly Error EmailIsNotUnique = Error.Conflict(
        "Identity.EmailIsNotUnique",
        "The provided email is already in use.");
}
