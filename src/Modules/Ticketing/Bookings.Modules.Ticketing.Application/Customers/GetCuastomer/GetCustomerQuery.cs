using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Ticketing.Application.Customers.GetCuastomer;

public sealed record GetCustomerQuery(Guid CustomerId) : IQuery<CustomerResponse>;
