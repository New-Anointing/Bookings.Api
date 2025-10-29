using Bookings.Common.Application.Authorization;
using Bookings.Common.Domain;
using Bookings.Modules.Users.Application.Users.GetUserPermissions;
using MediatR;

namespace Bookings.Modules.Users.Infrastucture.Authorization;

internal sealed class PermissionService(ISender sender) : IPermissionService
{
    public async Task<Result<PermissionResponse>> GetUserPermissionsAsync(string identityId)
    {
        return await sender.Send(new GetUserPermissionQuery(identityId));
    }
}
