using Newtonsoft.Json;

namespace App1.Model
{
    public class RatesResponse
    {
        public int timestamp { get; set; }

        [JsonProperty(PropertyName = "base")]
        public string BaseCurrency { get; set; }

        public Rates rates { get; set; }
    }
}
