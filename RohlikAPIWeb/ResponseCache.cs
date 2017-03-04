using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using RohlikAPI;
using RohlikAPI.Model;

namespace RohlikAPIWeb
{
    public class ResponseCache
    {
        private static readonly MemoryCache Cache = MemoryCache.Default;
        private readonly FileSystemCache fileSystemCache = new FileSystemCache();

        private List<Product> InitializeProductCache()
        {
            var products = fileSystemCache.GetAllProducts();
            if (products == null)
            {
                var api = new RohlikApi(City.Brno);
                products = api.GetAllProducts().ToList();
                fileSystemCache.SetProductCache(products);
            }
            return products;
        }


        private T AddOrGetExisting<T>(string key, Func<T> valueFactory)
        {
            var newValue = new Lazy<T>(valueFactory);
            var oldValue = Cache.AddOrGetExisting(key, newValue, DateTime.Now.AddMinutes(90)) as Lazy<T>;
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

        public List<Product> GetAllProducts()
        {
            return AddOrGetExisting("allproducts", InitializeProductCache);
        }
    }
}