using Caliburn.Micro;

namespace App1.ViewModels
{
    public class ViewModelBase : Screen
    {
        protected readonly INavigationService PageNavigationService;

        protected ViewModelBase(INavigationService pageNavigationService)
        {
            PageNavigationService = pageNavigationService;
        }

        public bool CanGoBack
        {
            get { return PageNavigationService.CanGoBack; }
        }

        public void GoBack()
        {
            PageNavigationService.GoBack();
        }

        protected void NavigateTo<T>()
        {
            PageNavigationService.Navigate<T>();
        }       
    }
}
