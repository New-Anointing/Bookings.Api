using Bookings.Common.Domain;
using Bookings.Modules.Users.Application.Users.GetUser;
using Bookings.Modules.Users.PublicApi;
using MediatR;
using UserResponse = Bookings.Modules.Users.PublicApi.UserResponse;

namespace Bookings.Modules.Users.Infrastucture.PublicApi;

internal sealed class UsersApi(ISender sender) : IUsersApi
{

    public async Task<UserResponse?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        Result<Application.Users.GetUser.UserResponse> result = await sender.Send(new GetUserQuery(userId), cancellationToken);

        if (result.IsFailure)
        {
            return null;
        }

        return new UserResponse(
            result.Value.Id,
            result.Value.Email,
            result.Value.FirstName,
            result.Value.LastName);
    }
}
