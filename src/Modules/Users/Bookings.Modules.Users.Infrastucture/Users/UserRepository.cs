using Bookings.Modules.Users.Domain.Users;
using Bookings.Modules.Users.Infrastucture.Database;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Modules.Users.Infrastucture.Users;

internal sealed class UserRepository(UsersDbContext context) : IUserRepository
{
    public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Users.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public void Insert(User user)
    {
        context.Users.Add(user);
    }
}
