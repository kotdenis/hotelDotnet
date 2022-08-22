using HotelApp.Core.Services.Interfaces;
using HotelApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace HotelApp.Core.Services.Implementation
{
    /// <summary>
    /// Manages distributed cache
    /// </summary>
    public class DistributedCacheManager : ICacheManager
    {
        private readonly IDistributedCache _distributedCache;
        private readonly HotelDbContext _dbContext;
        public DistributedCacheManager(IDistributedCache distributedCache, HotelDbContext dbContext)
        {
            _dbContext = dbContext;
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// Gets cache if exists
        /// </summary>
        /// <typeparam name="T">Genetic type where T is class</typeparam>
        /// <param name="key">Cache key</param>
        /// <returns>List of T</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<List<T>> GetAsync<T>(string key) where T : class
        {
            if (key == null)
                throw new ArgumentNullException("Cache key needed");
            var result = await _distributedCache.GetAsync(key);
            if (result == null)
                return new List<T>();
            var temp = Encoding.UTF8.GetString(result);
            var list = JsonConvert.DeserializeObject<List<T>>(temp);
            return list!;
        }

        /// <summary>
        /// Sets cache
        /// </summary>
        /// <typeparam name="T">Genetic type where T is class</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="values">List of values to set in cache</param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task SetAsync<T>(string key, List<T> values, int absoluteExpiration, int slidingExpiration) where T : class
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("Cache key needed");
            var temp = JsonConvert.SerializeObject(values);
            var result = Encoding.UTF8.GetBytes(temp);
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(absoluteExpiration))
                .SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpiration));
            await _distributedCache.SetAsync(key, result);
        }

        /// <summary>
        /// Clears cache by key
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns></returns>
        public Task ClearlAsync(string key)
        {
            _distributedCache.Remove(key);
            return Task.CompletedTask;
        }
    }
}
