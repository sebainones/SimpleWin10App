using Caliburn.Micro;

namespace App1.ViewModels
{
    public class InformationViewModel : ViewModelBase
    {
        private INavigationService _pageNavigationService;
        public InformationViewModel(INavigationService pageNavigationService) : base(pageNavigationService)
        {

            _pageNavigationService = pageNavigationService;
        }

        public void GoHome()
        {
            _pageNavigationService.For<MainViewModel>().Navigate();
        }
    }
}
