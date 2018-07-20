using System.Collections.Generic;

namespace RohlikAPI.Model.JsonDeserialization
{
    public class ProductResponseJson
    {
        public long Status { get; set; }
        public List<string> Messages { get; set; }
        public Data Data { get; set; }
    }
}