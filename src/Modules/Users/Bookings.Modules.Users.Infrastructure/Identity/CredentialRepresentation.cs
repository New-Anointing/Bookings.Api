namespace Bookings.Modules.Users.Infrastucture.Identity;

internal sealed record CredentialRepresentation(string Type, string Value, bool Temporary);

