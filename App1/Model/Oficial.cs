namespace App1.Model
{
    //TODO: Apply inheritance as Euro and dollar are the same but different currencies.

    public class Oficial
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
