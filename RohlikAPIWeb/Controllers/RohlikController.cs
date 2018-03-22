using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using RohlikAPIWeb.Cache;
using RohlikAPIWeb.Models;

namespace RohlikAPIWeb.Controllers
{
    public class RohlikController : ApiController
    {
        private static readonly ResponseCache Cache = new ResponseCache();
        private static readonly FileSystemCache FileSystemCache = new FileSystemCache();
        private static readonly RohlikSync RohlikSync = new RohlikSync();

        [HttpGet]
        [Route("api/GetAllProducts")]
        public ApiResponse Get()
        {
            return Cache.GetAllProducts();
        }

        [HttpGet]
        [Route("api/GetAllProductsAge")]
        public int GetProductsAgeHours()
        {
            var lastSyncTime = Cache.GetAllProducts().SyncTime;
            TimeSpan diff = DateTime.UtcNow - lastSyncTime;
            var hours = (int) diff.TotalHours;
            return hours;
        }

        [HttpGet]
        [Route("api/UpdateProducts")]
        public IHttpActionResult UpdateProducts(string key)
        {
            var acceptedKey = ConfigurationManager.AppSettings["updateKey"];
            if (key != acceptedKey)
            {
                return new StatusCodeResult(HttpStatusCode.Unauthorized, Request);
            }

            Task.Run(() =>
            {
                var response = RohlikSync.CreateApiResponse();
                FileSystemCache.SetProductCache(response);
                Cache.SetProductCache(response);
            });
            
            return Ok();
        }
    }
}