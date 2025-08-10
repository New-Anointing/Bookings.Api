﻿using MediatR;

namespace Bookings.Shared.Abstractions.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse> 
    where TResponse : notnull
{
}
