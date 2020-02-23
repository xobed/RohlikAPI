using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RohlikAPIWeb.Model;

namespace RohlikAPIWeb
{
    public class RedisStorage
    {
        private readonly IDistributedCache cache;
        private const string CacheKey = "showtimes";

        public RedisStorage(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public void Set(ApiResponse apiResponse)
        {
            var serialized = JsonConvert.SerializeObject(apiResponse);
            cache.SetString(CacheKey, serialized);
        }

        public int? GetAgeHours()
        {
            var cacheEntry = cache.GetString(CacheKey);
            if (cacheEntry == null) return null;
            var response = JsonConvert.DeserializeObject<ApiResponse>(cacheEntry);
            var timeSinceLastUpdate = DateTime.Now - response.SyncTime;
            return (int) timeSinceLastUpdate.TotalHours;
        }

        public ApiResponse Get()
        {
            try
            {
                var cacheEntry = cache.GetString(CacheKey);
                var response = JsonConvert.DeserializeObject<ApiResponse>(cacheEntry);
                return response;
            }
            catch (Exception)
            {
                return new ApiResponse(DateTime.MinValue, new List<ApiProduct>());
            }
        }
    }
}