using Newtonsoft.Json;
using RohlikAPI.Model.JsonDeserialization;

namespace RohlikAPI
{
    internal class Rohlikovac
    {
        private readonly PersistentSessionHttpClient httpSessionClient;

        public Rohlikovac(PersistentSessionHttpClient httpSessionClient)
        {
            this.httpSessionClient = httpSessionClient;
        }

        public RohlikovacResult Run()
        {
            var resultString = httpSessionClient.Get("https://www.rohlik.cz/services/frontend-service/credit-forge-roll");
            return JsonConvert.DeserializeObject<RohlikovacResult>(resultString);
        }
    }
}