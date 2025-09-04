using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookings.Common.Application.Data;
using Npgsql;

namespace Bookings.Common.Infrastructure.Data;
internal sealed class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}
