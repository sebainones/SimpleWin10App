using Caliburn.Micro;
using Windows.ApplicationModel;

namespace App1.ViewModels
{
    public class DesignMainViewModel : MainViewModel
    {
        public DesignMainViewModel() : base(null, null)
        {
            if (Execute.InDesignMode)//(DesignMode.DesignModeEnabled)
            {
                Caption = "Cotización en Argentina";

                Compra = 99.99;
                Venta = 88.88;
                EuroCompra = 77.77;
                EuroVenta = 66.66;
                LastUpdated = "25/10/2017";//("d MMM yyyy HH:mm");                    
            }
        }
    }
}
