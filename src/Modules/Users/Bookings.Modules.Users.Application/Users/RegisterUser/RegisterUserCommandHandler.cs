using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Users.Application.Abstractions.Data;
using Bookings.Modules.Users.Domain.Users;

namespace Bookings.Modules.Users.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {

        Result<User> result = User.Create(command.Email, command.FirstName, command.LastName);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        userRepository.Insert(result.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result.Value.Id;

    }
}
