using Caliburn.Micro;

namespace App1.ViewModels
{
    public abstract class ViewModelBase : Screen
    {
        protected readonly INavigationService PageNavigationService;
        private readonly ILog _log;


        protected ViewModelBase(INavigationService pageNavigationService)
        {
            PageNavigationService = pageNavigationService;

            //TODO: The log type should be the one inherinting this.
            _log = LogManager.GetLog(typeof(ViewModelBase));
        }

        public ILog Log
        {
            get { return _log; }
        }

        public bool CanGoBack
        {
            get { return PageNavigationService.CanGoBack; }
        }

        public bool CanForward
        {
            get { return PageNavigationService.CanGoForward; }
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
