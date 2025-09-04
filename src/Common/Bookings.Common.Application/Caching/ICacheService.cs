namespace Bookings.Common.Application.Caching;
public interface ICacheService
{
    Task<T?> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default);

    Task SetAsync<T>(
        string cacheKey,
        T value,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default);
}
