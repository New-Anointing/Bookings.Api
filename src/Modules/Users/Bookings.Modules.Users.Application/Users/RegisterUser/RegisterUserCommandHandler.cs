using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Users.Application.Abstractions.Data;
using Bookings.Modules.Users.Application.Abstractions.Identity;
using Bookings.Modules.Users.Domain.Users;

namespace Bookings.Modules.Users.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler(
    IIdentityProviderService identityProviderService, IUnitOfWork unitOfWork, IUserRepository userRepository)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        Result<string> identityResult = await identityProviderService.RegisterUserAsync(
            new UserModel(command.Email, command.Password, command.FirstName, command.LastName), cancellationToken);

        if (identityResult.IsFailure)
        {
            return Result.Failure<Guid>(identityResult.Error);
        }

        var user = User.Create(command.Email, command.FirstName, command.LastName, identityResult.Value);

        userRepository.Insert(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;

    }
}
