using System;
using System.Runtime.Caching;
using RohlikAPIWeb.Models;

namespace RohlikAPIWeb.Cache
{
    public class ResponseCache
    {
        private static readonly MemoryCache Cache = MemoryCache.Default;
        private readonly FileSystemCache fileSystemCache = new FileSystemCache();

        private ApiResponse InitializeProductCache()
        {
            var apiResponse = fileSystemCache.GetAllProducts();
            if (apiResponse == null)
            {
                throw new Exception("Failed to get products from file system cache. Products need to be initialized by calling UpdateProducts.");
            }
            return apiResponse;
        }


        private T AddOrGetExisting<T>(string key, Func<T> valueFactory)
        {
            var newValue = new Lazy<T>(valueFactory);
            var oldValue = Cache.AddOrGetExisting(key, newValue, DateTime.Now.AddMinutes(30)) as Lazy<T>;
            try
            {
                return (oldValue ?? newValue).Value;
            }
            catch
            {
                Cache.Remove(key);
                throw;
            }
        }

        public ApiResponse GetAllProducts()
        {
            return AddOrGetExisting("allproducts", InitializeProductCache);
        }

        public void SetProductCache(ApiResponse response)
        {
            Cache.Set("allproducts", response, DateTime.Now.AddMinutes(30));
        }
    }
}