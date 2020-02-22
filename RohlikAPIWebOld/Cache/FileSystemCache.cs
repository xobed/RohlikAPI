using System.IO;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using RohlikAPIWeb.Models;

namespace RohlikAPIWeb.Cache
{
    public class FileSystemCache
    {
        private readonly string cacheFilePath = Path.Combine(HttpRuntime.AppDomainAppPath, @"responseCache.json");
        private readonly object lockObject = new object();

        public ApiResponse GetAllProducts()
        {
            lock (lockObject)
            {
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
                    var productsCache = JsonConvert.DeserializeObject<ApiResponse>(productsString);
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

        public void SetProductCache(ApiResponse productList)
        {
            lock (lockObject)
            {
                var serializedProducts = JsonConvert.SerializeObject(productList);
                File.WriteAllText(cacheFilePath, serializedProducts, Encoding.UTF8);
            }
        }
    }
}