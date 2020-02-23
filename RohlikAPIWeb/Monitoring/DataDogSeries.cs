using System.Collections.Generic;
using Newtonsoft.Json;

namespace RohlikAPIWeb.Monitoring
{
    public class DataDogSeries
    {
        [JsonProperty("series")] public List<DataDogSerie> Series { get; set; }
    }

    public class DataDogSerie
    {
        [JsonProperty("metric")] public string Metric { get; set; }
        [JsonProperty("type")] public string Type { get; set; }
        [JsonProperty("points")] public double[][] Points { get; set; }
        [JsonProperty("host")] public string Host { get; set; }
    }
}