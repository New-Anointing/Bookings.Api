﻿
namespace Bookings.Modules.Events.Domain.Events;
public interface IEventRepository
{
    void Insert(Event @event);
}
