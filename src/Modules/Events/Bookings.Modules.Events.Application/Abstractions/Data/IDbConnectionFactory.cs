using System.Data.Common;

namespace Bookings.Modules.Events.Application.Abstractions.Data;
public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAssync();
}
