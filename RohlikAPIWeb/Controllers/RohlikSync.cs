using System;
using System.Linq;
using RohlikAPI;
using RohlikAPIWeb.Models;

namespace RohlikAPIWeb.Controllers
{
    public class RohlikSync
    {
        public ApiResponse CreateApiResponse()
        {
            var api = new RohlikApi(City.Brno);
            var products = api.GetAllProducts().ToList();
            var apiProducts = products.Select(p => new ApiProduct(p.Name, p.Price, p.PricePerUnit, p.Unit, p.ProductUrl, p.ImageUrl)).OrderBy(p => p.PPU);
            var response = new ApiResponse(DateTime.UtcNow, apiProducts);
            return response;
        }
    }
}