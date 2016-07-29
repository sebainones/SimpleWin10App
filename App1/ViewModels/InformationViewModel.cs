using Caliburn.Micro;
using Windows.UI.Notifications;

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

        public void UpdateTile()
        {
            var template = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Text01);
            var childNode = template?.GetElementsByTagName("text");
            if (childNode != null)
            {
                childNode[0].InnerText = "Actualizado";

                TileNotification tileNotification = new TileNotification(template);
                TileUpdater updateMgr = TileUpdateManager.CreateTileUpdaterForApplication();
                updateMgr.Update(tileNotification);
            }
        }
    }
}
