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
    }
}
