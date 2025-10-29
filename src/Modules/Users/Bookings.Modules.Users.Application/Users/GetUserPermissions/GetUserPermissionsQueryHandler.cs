using System.Data.Common;
using Bookings.Common.Application.Authorization;
using Bookings.Common.Application.Data;
using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Users.Domain.Users;
using Dapper;

namespace Bookings.Modules.Users.Application.Users.GetUserPermissions;

public sealed record GetUserPermissionQuery(string IdentityId) : IQuery<PermissionResponse>;

internal sealed class GetUserPermissionsQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetUserPermissionQuery, PermissionResponse>
{
    public async Task<Result<PermissionResponse>> Handle(GetUserPermissionQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT DISTINCT
                u.id AS {nameof(UserPermission.UserId)},
                rp.permission_code AS {nameof(UserPermission.PermissionName)}
            FROM users.users u
            JOIN users.user_roles ur ON ur.user_id = u.id
            JOIN users.role_permissions rp ON rp.role_name = ur.role_name
            WHERE u.identity_id = @IdentityId;
            """;

        List<UserPermission> permission = (await connection.QueryAsync<UserPermission>(sql, request)).AsList();

        if (!permission.Any())
        {
            return Result.Failure<PermissionResponse>(UserErrors.NotFound(request.IdentityId));
        }

        return new PermissionResponse(permission[0].UserId, permission.Select(p => p.PermissionName).ToHashSet());
    }
}

internal sealed class UserPermission
{
    internal Guid UserId { get; init; }
    internal string PermissionName { get; init; }
}
