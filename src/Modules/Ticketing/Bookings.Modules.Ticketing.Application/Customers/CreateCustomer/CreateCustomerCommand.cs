using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Ticketing.Application.Customers.CreateCustomer;

public sealed record CreateCustomerCommand(Guid Id, string Email, string FirstName, string LastName) : ICommand;
