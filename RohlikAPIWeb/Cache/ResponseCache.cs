using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using RohlikAPI;
using RohlikAPIWeb.Models;

namespace RohlikAPIWeb.Cache
{
    public class ResponseCache
    {
        private static readonly MemoryCache Cache = MemoryCache.Default;
        private readonly FileSystemCache fileSystemCache = new FileSystemCache();

        private List<ApiProduct> InitializeProductCache()
        {
            var products = fileSystemCache.GetAllProducts();
            if (products == null)
            {
                var api = new RohlikApi(City.Brno);
                products = api.GetAllProducts().ToList();
                fileSystemCache.SetProductCache(products);
            }
            return products.Select(p => new ApiProduct(p.Name, p.Price, p.PricePerUnit, p.Unit, p.ProductUrl)).OrderByDescending(p => p.PPU).ToList();
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

        public List<ApiProduct> GetAllProducts()
        {
            return AddOrGetExisting("allproducts", InitializeProductCache);
        }
    }
}