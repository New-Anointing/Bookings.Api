namespace Bookings.Modules.Ticketing.Application.Customers.GetCuastomer;

public sealed record CustomerResponse(Guid Id, string Email, string FirstName, string LastName);
