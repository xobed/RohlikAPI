using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RohlikAPIWeb.Models;

namespace RohlikAPIWeb.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("/api/GetAllProducts")]
        public ApiResponse Get()
        {
            return new ApiResponse(DateTime.UtcNow, new List<ApiProduct>());
        }
    }
}