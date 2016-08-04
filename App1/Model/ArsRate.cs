using Newtonsoft.Json;

namespace RateApp.Model
{
    internal class ArsRate
    {
        [JsonProperty("oficial")]
        public Currency Dolar { get; set; }
        [JsonProperty("blue")]
        public Currency DolarBlue { get; set; }
        [JsonProperty("oficial_euro")]
        public Currency Euro { get; set; }
        [JsonProperty("blue_euro")]
        public Currency EuroBLue { get; set; }
        [JsonProperty("last_update")]
        public string LastUpdate { get; set; }
    }
}
