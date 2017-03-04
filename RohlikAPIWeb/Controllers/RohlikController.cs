using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RohlikAPI.Model;

namespace RohlikAPIWeb.Controllers
{
    public class RohlikController : ApiController
    {
        private static readonly ResponseCache Cache = new ResponseCache();

        [HttpGet]
        [Route("api/GetAllProducts")]
        public List<Product> Get()
        {
            return Cache.GetAllProducts();
        }
    }
}
