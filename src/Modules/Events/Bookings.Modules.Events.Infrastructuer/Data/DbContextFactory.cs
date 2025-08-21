using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookings.Modules.Events.Application.Abstractions.Data;
using Npgsql;

namespace Bookings.Modules.Events.Infrastructure.Data;
internal sealed class DbContextFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}
