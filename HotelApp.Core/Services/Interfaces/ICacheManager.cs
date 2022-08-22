namespace HotelApp.Core.Services.Interfaces
{
    /// <summary>
    /// The contract for class working with cache <see cref="Implementation.DistributedCacheManager"/>
    /// </summary>
    public interface ICacheManager
    {
        Task<List<T>> GetAsync<T>(string key) where T : class;
        Task SetAsync<T>(string key, List<T> values, int absoluteExpiration, int slidingExpiration) where T : class;
        Task ClearlAsync(string key);
    }
}
