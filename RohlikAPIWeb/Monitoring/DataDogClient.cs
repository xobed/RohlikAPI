using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RohlikAPIWeb.Model;

namespace RohlikAPIWeb.Monitoring
{
    public class DataDogClient
    {
        private readonly ILogger<DataDogClient> _logger;
        private readonly HttpClient _client = new HttpClient();
        private readonly string _hostname = "cloud";
        private readonly string _url;

        public DataDogClient(IOptions<AppSettings> settings, ILogger<DataDogClient> logger)
        {
            _logger = logger;
            var apiKey = settings.Value.DATADOG_APIKEY;
            _url = $"https://app.datadoghq.com/api/v1/series?api_key={apiKey}";
        }


        private async Task SendDataAsync(DataDogSeries series)
        {
            try
            {
                var payloadString = JsonConvert.SerializeObject(series);
                _logger.LogDebug($"Payload to be sent to datadog:{Environment.NewLine}{payloadString}");
                var content = new StringContent(payloadString);
                var result = await _client.PostAsync(_url, content);
                var response = await result.Content.ReadAsStringAsync();
                var code = result.StatusCode;
                _logger.LogDebug($"Datadog post returned with HTTP {code} - '{response}'");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to post metrics to datadog. {ex}");
            }
        }

        public async Task SendGauges(Dictionary<string, double> metricNameAndValueDictionary)
        {
            var series = new DataDogSeries {Series = new List<DataDogSerie>()};
            series.Series.AddRange(metricNameAndValueDictionary.Select(m => CreateDataDogGaugeSerie(m.Key, m.Value)));
            await SendDataAsync(series);
        }

        public async Task SendGauge(string name, double value)
        {
            await SendGauges(new Dictionary<string, double>
            {
                {name, value}
            });
        }

        public async Task SendGauge(string name, int value)
        {
            await SendGauges(new Dictionary<string, double>
            {
                {name, value}
            });
        }

        private DataDogSerie CreateDataDogGaugeSerie(string metricName, double value)
        {
            return new DataDogSerie
            {
                Host = _hostname,
                Type = "gauge",
                Points = new[] {new[] {DateTimeOffset.Now.ToUnixTimeSeconds(), value}},
                Metric = metricName
            };
        }
    }
}