using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Model
{
    class ArsRate
    {
        public Oficial oficial { get; set; }
        public Blue blue { get; set; }
        public OficialEuro oficial_euro { get; set; }
        public BlueEuro blue_euro { get; set; }
        public string last_update { get; set; }
    }
}
