namespace App1.Model
{
    public class OficialEuro
    {
        public double value_avg { get; set; }
        public double value_sell { get; set; }
        public double value_buy { get; set; }
        public bool HasValue { get { return value_buy != 0 & value_sell != 0; } }
    }
}
