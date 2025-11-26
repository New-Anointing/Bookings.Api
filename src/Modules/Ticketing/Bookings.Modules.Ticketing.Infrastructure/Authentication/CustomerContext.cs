using Bookings.Common.Application.Exceptions;
using Bookings.Common.Infrastructure.Authentication;
using Bookings.Modules.Ticketing.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace Bookings.Modules.Ticketing.Infrastructure.Authentication;

internal sealed class CustomerContext(IHttpContextAccessor httpContextAccessor) : ICustomerContext
{
    public Guid CustomerId => httpContextAccessor.HttpContext?.User.GetUserId() ??
                              throw new BookingsException("User identifier is unavailable");
}
