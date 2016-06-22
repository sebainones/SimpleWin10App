using App1.Model;
using App1.Services;
using Caliburn.Micro;

namespace App1.ViewModels
{
    public class AnotherViewModel : ViewModelBase
    {
        private INavigationService _pageNavigationService;


        public AnotherViewModel(INavigationService pageNavigationService) : base(pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;
        }

        public void GoPrevious()
        {
            //_pageNavigationService.Navigate<AnotherViewModel>();
            _pageNavigationService.For<SomeViewModel>().Navigate();
        }

        public void GetExchangeRates()
        {
            //_pageNavigationService.Navigate<AnotherViewModel>();
            _pageNavigationService.For<SomeViewModel>().Navigate();

            RestClient restClient = new RestClient();

            var response = restClient.Get<RatesResponse>();

        }
    }
}
