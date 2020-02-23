using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RohlikAPI;
using RohlikAPIWeb.Model;
using RohlikAPIWeb.Monitoring;
using Sentry;

namespace RohlikAPIWeb
{
    public class RohlikProductsUpdateService : IHostedService, IDisposable
    {
        private readonly ILogger<RohlikProductsUpdateService> logger;
        private readonly DataDogClient dataDogClient;
        private readonly RedisStorage storage;
        private readonly ISentryClient sentryClient;
        private Timer timer;
        private static readonly object Lock = new object();

        public RohlikProductsUpdateService(ILogger<RohlikProductsUpdateService> logger, IServiceProvider serviceProvider, DataDogClient dataDogClient, RedisStorage storage)
        {
            this.logger = logger;
            this.dataDogClient = dataDogClient;
            this.storage = storage;
            try
            {
                sentryClient = (ISentryClient) serviceProvider.GetService(typeof(ISentryClient));
            }
            catch
            {
                sentryClient = null;
            }
        }

        private void Update(object state)
        {
            try
            {
                lock (Lock)
                {
                    var age = storage.GetAgeHours();
                    if (age.HasValue && age.Value < 2)
                    {
                        logger.LogInformation("No need to refresh products yet");
                        dataDogClient.SendGauge("rohlikapi.productsage", age.Value).Wait();
                        return;
                    }

                    logger.LogInformation("Starting products update.");
                    var api = new RohlikApi(City.Brno);
                    var allProducts = api.GetAllProducts();
                    var apiProducts = allProducts.Select(p => new ApiProduct(p.Name, p.Price, p.PricePerUnit, p.Unit, p.ProductUrl, p.ImageUrl)).OrderBy(p => p.PPU);
                    var response = new ApiResponse(DateTime.UtcNow, apiProducts);
                    storage.Set(response);

                    dataDogClient.SendGauge("rohlikapi.productsage", 0).Wait();
                    logger.LogInformation("Products updated.");
                }
            }
            catch (Exception e)
            {
                logger.LogCritical(e, e.Message);
                sentryClient?.CaptureException(e);
            }
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Update service is starting.");

            timer = new Timer(Update, null, TimeSpan.Zero, TimeSpan.FromMinutes(60));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Update service is stopping.");

            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}