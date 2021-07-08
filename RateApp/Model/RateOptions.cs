using System.ComponentModel.DataAnnotations;

namespace RateApp.Model
{
    public enum RateOptions
    {
        [Display(Description = "compra")]
        Buy,
        [Display(Description = "venta")]
        Sell
    }
}
