using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Ticketing.Application.Customers.UpdateCustomer;

public sealed record UpdateCustomerCommand(Guid CustomerId, string FirstName, string LastName) : ICommand;
