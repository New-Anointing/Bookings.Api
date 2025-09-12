namespace Bookings.Modules.Users.PublicApi;

public interface IUsersApi
{
    Task<UserResponse?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
