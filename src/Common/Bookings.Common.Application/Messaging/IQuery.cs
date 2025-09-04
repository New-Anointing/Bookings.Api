using Bookings.Common.Domain;
using MediatR;

namespace Bookings.Common.Application.Messaging;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
