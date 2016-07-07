using App1.Model;
using App1.Services;
using Caliburn.Micro;

namespace App1.ViewModels
{
    public class AnotherViewModel : ViewModelBase
    {
        private INavigationService _pageNavigationService;
        private IRestClient _restClient;

        private double compra;
        public double Compra
        {
            get { return compra; }
            set
            {
                compra = value;
                NotifyOfPropertyChange(() => Compra);
            }
        }
        private double venta;

        public double Venta
        {
            get { return venta; }
            set { venta = value;
                NotifyOfPropertyChange(() => Venta);
            }
        }


        public AnotherViewModel(INavigationService pageNavigationService, IRestClient restClient) : base(pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;
            _restClient = restClient;
        }

        public void GoPrevious()
        {
            //_pageNavigationService.Navigate<AnotherViewModel>();
            _pageNavigationService.For<SomeViewModel>().Navigate();
        }

        public async void GetExchangeRates()
        {
            var response = await _restClient.Get<ArsRate>();

            PopulateRates(response);

        }

        private void PopulateRates(ArsRate arsRate)
        {
            Compra = arsRate.oficial.value_buy;
            Venta = arsRate.oficial.value_sell;
        }
    }
}
