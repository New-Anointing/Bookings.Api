using MediatR;

namespace Bookings.Shared.Abstractions.CQRS;

public interface ICommand : ICommand<Unit> { }

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
