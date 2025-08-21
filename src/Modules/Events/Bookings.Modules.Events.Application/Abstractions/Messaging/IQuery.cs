
using Bookings.Modules.Events.Domain.Abstractions;
using MediatR;

namespace Bookings.Modules.Events.Application.Abstractions.Messaging;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
