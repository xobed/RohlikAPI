using Microsoft.AspNetCore.Mvc;
using RohlikAPIWeb.Model;

namespace RohlikAPIWeb.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly RedisStorage storage;

        public ProductController(RedisStorage storage)
        {
            this.storage = storage;
        }

        [HttpGet]
        [Route("/api/GetAllProducts")]
        public ApiResponse Get()
        {
            return storage.Get();
        }
    }
}