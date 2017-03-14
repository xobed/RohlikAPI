using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using RohlikAPI;
using RohlikAPIWeb.Cache;
using RohlikAPIWeb.Models;

namespace RohlikAPIWeb.Controllers
{
    public class RohlikController : ApiController
    {
        private static readonly ResponseCache Cache = new ResponseCache();
        private static readonly FileSystemCache FileSystemCache = new FileSystemCache();

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

            var api = new RohlikApi(City.Brno);
            var products = api.GetAllProducts().ToList();
            var apiProducts = products.Select(p => new ApiProduct(p.Name, p.Price, p.PricePerUnit, p.Unit, p.ProductUrl)).OrderBy(p => p.PPU);
            var response = new ApiResponse(DateTime.UtcNow, apiProducts);

            FileSystemCache.SetProductCache(response);
            return Ok();
        }
    }
}