﻿namespace Punica.Bp.CQRS
{
    public interface IPublisher
    {
        Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default);
    }
}
