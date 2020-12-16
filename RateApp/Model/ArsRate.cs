using Newtonsoft.Json;

namespace RateApp.Model
{
    internal class ArsRate
    {
        public ArsRate()
        {
            Dolar = new Rate("dolar");
            DolarBlue = new Rate("dolar blue");
            Euro = new Rate("euro");
            EuroBLue = new Rate("euro blue");
        }

        
        [JsonProperty("oficial")]
        public Rate Dolar { get; set; }

        [JsonProperty("blue")]
        public Rate DolarBlue { get; set; }

        [JsonProperty("oficial_euro")]
        public Rate Euro { get; set; }

        [JsonProperty("blue_euro")]
        public Rate EuroBLue { get; set; }

        [JsonProperty("last_update")]
        public string LastUpdate { get; set; }
    }
}
