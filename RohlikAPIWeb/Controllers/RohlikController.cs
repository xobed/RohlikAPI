using System.Configuration;
using System.Net;
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
        [Route("api/UpdateProducts")]
        public IHttpActionResult UpdateProducts(string key)
        {
            var acceptedKey = ConfigurationManager.AppSettings["updateKey"];
            if (key != acceptedKey)
            {
                return new StatusCodeResult(HttpStatusCode.Unauthorized, Request);
            }

            var response = RohlikSync.CreateApiResponse();

            FileSystemCache.SetProductCache(response);
            Cache.SetProductCache(response);
            return Ok();
        }
    }
}