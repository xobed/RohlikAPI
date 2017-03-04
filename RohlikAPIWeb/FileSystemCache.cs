using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using RohlikAPI.Model;

namespace RohlikAPIWeb
{
    public class FileSystemCache
    {
        private readonly string cacheFilePath = Path.Combine(HttpRuntime.AppDomainAppPath, @"responseCache.json");
        private readonly string timeStampFilePath = Path.Combine(HttpRuntime.AppDomainAppPath, @"timestamp.json");

        private readonly object lockObject = new object();

        private bool IsCacheValid()
        {
            try
            {
                var timeStamp = File.ReadAllText(timeStampFilePath);
                var expiration = JsonConvert.DeserializeObject<DateTime>(timeStamp);
                return DateTime.UtcNow < expiration;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Product> GetAllProducts()
        {
            lock (lockObject)
            {
                if (!IsCacheValid())
                {
                    return null;
                }

                string productsString;
                try
                {
                    productsString = File.ReadAllText(cacheFilePath);
                }
                catch (FileNotFoundException)
                {
                    return null;
                }

                try
                {
                    var productsCache = JsonConvert.DeserializeObject<List<Product>>(productsString);
                    return productsCache;
                }
                // Return null on deserialization error
                // Data class may change but server will persist cached file with new deployment
                catch (JsonReaderException)
                {
                    return null;
                }
                catch (JsonSerializationException)
                {
                    return null;
                }
            }
        }

        public void SetProductCache(List<Product> productList)
        {
            lock (lockObject)
            {
                var expiration = DateTime.UtcNow.AddMinutes(90);
                var serializedExpiration = JsonConvert.SerializeObject(expiration);
                File.WriteAllText(timeStampFilePath, serializedExpiration);

                var serializedProducts = JsonConvert.SerializeObject(productList);
                File.WriteAllText(cacheFilePath, serializedProducts, Encoding.UTF8);
            }
        }
    }
}