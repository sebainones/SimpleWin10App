using System;

namespace RateApp.Model
{
    internal class FakeArsRate : ArsRate
    {
        public FakeArsRate()
        {   
            Dolar.Compra = 0.01;
            Dolar.Venta = 0.01;
            Euro.Compra = 0.01;
            Euro.Venta = 0.01;
            LastUpdate = DateTime.Now.ToString("d MMM yyyy");
        }
    }
}
