using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Model
{
    public class RatesResponse
    {
        [JsonProperty(PropertyName = "default")]        
        public string BaseCurrency { get; set; }

    }
}
