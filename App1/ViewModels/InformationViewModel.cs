using Caliburn.Micro;
using RateApp.Tiles;

namespace RateApp.ViewModels
{
    public class InformationViewModel : ViewModelBase
    {
        private INavigationService _pageNavigationService;
        private ITileManager _tileManager;

        public InformationViewModel(INavigationService pageNavigationService, ITileManager tileManager) : base(pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;
            _tileManager = tileManager;
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
