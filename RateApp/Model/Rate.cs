using Newtonsoft.Json;

namespace RateApp.Model
{
    internal class Rate
    {
        public string Name { get; set; }

        [JsonProperty("value_avg")]
        public double Promedio { get; set; }

        [JsonProperty("value_sell")]
        public double Venta { get; set; }

        [JsonProperty("value_buy")]
        public double Compra { get; set; }

        public bool HasValue
        {
            get { return Compra != 0 & Venta != 0; }
        }

        public Rate(string rateName)
        {
            Name = rateName;
        }
    }
}