using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using RohlikAPIWeb.Cache;
using RohlikAPIWeb.Models;

namespace RohlikAPIWeb.Controllers
{
    public class RohlikController : ApiController
    {
        private static readonly ResponseCache Cache = new ResponseCache();
        private static readonly RohlikSync RohlikSync = new RohlikSync();
        private readonly TelemetryClient _telemetryClient = new TelemetryClient(TelemetryConfiguration.Active);

        [HttpGet]
        [Route("api/GetAllProducts")]
        public ApiResponse Get()
        {
            return Cache.GetAllProducts();
        }

        [HttpGet]
        [Route("api/GetAllProductsAge")]
        public int GetProductsAgeHours()
        {
            var lastSyncTime = Cache.GetAllProducts().SyncTime;
            TimeSpan diff = DateTime.UtcNow - lastSyncTime;
            var hours = (int) diff.TotalHours;
            return hours;
        }

        [HttpGet]
        [Route("api/UpdateProducts")]
        public IHttpActionResult UpdateProducts(string key)
        {
            var acceptedKey = ConfigurationManager.AppSettings["updateKey"];
            if (key != acceptedKey)
            {
                return new StatusCodeResult(HttpStatusCode.Unauthorized, Request);
            }

            Task.Run(() => UpdateProductsTask());
            return Ok();
        }

        private void UpdateProductsTask()
        {
            var operation = _telemetryClient.StartOperation<RequestTelemetry>("UpdateProducts");

            try
            {
                var response = RohlikSync.CreateApiResponse();
                Cache.SetProductCache(response);
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _telemetryClient.StopOperation(operation);
            }
        }
    }
}