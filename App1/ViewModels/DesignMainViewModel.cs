using Caliburn.Micro;

namespace RateApp.ViewModels
{
    internal class DesignMainViewModel : MainViewModel
    {
        public DesignMainViewModel() : base(null, null, null, null, null)
        {

            if (Execute.InDesignMode)//(DesignMode.DesignModeEnabled)
            {
                Caption = "Cotización en Argentina";

                DolarCompra = 99.99;
                DolarVenta = 88.88;
                EuroCompra = 77.77;
                EuroVenta = 66.66;
                LastUpdated = "25/10/2017";//("d MMM yyyy HH:mm");                    
            }
        }
    }
}
