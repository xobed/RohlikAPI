using System.Collections.Generic;
using System.Web.Http;
using RohlikAPIWeb.Cache;
using RohlikAPIWeb.Models;

namespace RohlikAPIWeb.Controllers
{
    public class RohlikController : ApiController
    {
        private static readonly ResponseCache Cache = new ResponseCache();

        [HttpGet]
        [Route("api/GetAllProducts")]
        public List<ApiProduct> Get()
        {
            return Cache.GetAllProducts();
        }
    }
}