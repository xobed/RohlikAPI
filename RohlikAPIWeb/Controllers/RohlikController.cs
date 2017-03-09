using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RohlikAPI.Model;
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
