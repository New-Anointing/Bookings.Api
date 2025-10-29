using Bookings.Common.Domain;

namespace Bookings.Common.Application.Authorization;

public interface IPermissionService
{
    Task<Result<PermissionResponse>> GetUserPermissionsAsync(string identityId);
}
