using Caliburn.Micro;
using RateApp.Tiles;

namespace RateApp.ViewModels
{
    public class InformationViewModel : ViewModelBase
    {
        private INavigationService _pageNavigationService;
        private ITileManager _tileManager;

        private string message;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }


        public InformationViewModel(INavigationService pageNavigationService, ITileManager tileManager) : base(pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;
            _tileManager = tileManager;

            message = "Dolar Argentina porvee la cotización del Dolar y el Euro en Argentina actualziada diariamente";
        }

        public void GoHome()
        {
            _pageNavigationService.For<MainViewModel>().Navigate();
        }

        public void UpdateTile()
        {
            _tileManager.UpdateTile();
        }
    }
}
