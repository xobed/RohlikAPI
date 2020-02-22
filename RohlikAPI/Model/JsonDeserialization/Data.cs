using System.Collections.Generic;

namespace RohlikAPI.Model.JsonDeserialization
{
    public class Data
    {
        public List<RohlikProduct> ProductList { get; set; }
        public long TotalHits { get; set; }
    }
}