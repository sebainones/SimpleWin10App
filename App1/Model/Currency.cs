namespace App1.Model
{
    internal class Currency
    {
        public double value_avg { get; set; }
        public double value_sell { get; set; }
        public double value_buy { get; set; }
        public bool HasValue
        {
            get { return value_buy != 0 & value_sell != 0; }
        }
    }
}