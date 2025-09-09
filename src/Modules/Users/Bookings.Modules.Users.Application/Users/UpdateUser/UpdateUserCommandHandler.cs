using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Users.Application.Abstractions.Data;
using Bookings.Modules.Users.Domain.Users;

namespace Bookings.Modules.Users.Application.Users.UpdateUser;


internal sealed class UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetAsync(command.UserId, cancellationToken);

        if(user is null)
        {
            return Result.Failure(UserErrors.NotFound(command.UserId));
        }

        user.Update(command.FirstName, command.LastName);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
