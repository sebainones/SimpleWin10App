using Caliburn.Micro;

namespace App1.ViewModels
{
    public class SomeViewModel : ViewModelBase
    {
        private INavigationService _pageNavigationService;


        public SomeViewModel(INavigationService pageNavigationService) : base(pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;
        }

        private string _myMessage;
        public string MyMessage
        {
            get { return _myMessage; }
            set
            {
                _myMessage = value;
                NotifyOfPropertyChange(() => MyMessage);
            }
        }

        public void SayHello()
        {
            MyMessage = "Hello World!";
        }
    }
}
