using System;
using System.Collections.Generic;

namespace RohlikAPIWeb.Models
{
    public class ApiResponse
    {
        public ApiResponse(DateTime utcNow, IEnumerable<ApiProduct> products)
        {
            SyncTime = utcNow;
            Products = products;
        }

        public IEnumerable<ApiProduct> Products { get; set; }

        public DateTime SyncTime { get; set; }
    }
}