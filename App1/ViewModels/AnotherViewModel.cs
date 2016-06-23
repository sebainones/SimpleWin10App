using System;
using App1.Model;
using App1.Services;
using Caliburn.Micro;

namespace App1.ViewModels
{
    public class AnotherViewModel : ViewModelBase
    {
        private INavigationService _pageNavigationService;
        private IRestClient _restClient;

        private double pesos;
        public double Pesos
        {
            get { return pesos; }
            set
            {
                pesos = value;
                NotifyOfPropertyChange(() => Pesos);
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
            //_pageNavigationService.Navigate<AnotherViewModel>();
            _pageNavigationService.For<SomeViewModel>().Navigate();

            var response = await _restClient.Get<RatesResponse>();

            PopulateRates(response.rates);

        }

        private void PopulateRates(Rates rates)
        {
            Pesos = rates.ARS;
        }
    }
}
