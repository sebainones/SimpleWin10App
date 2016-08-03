using App1.Background;
using Caliburn.Micro;
using NotificationsExtensions;
using NotificationsExtensions.Tiles;
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


        //TODO: Do something with this. Maybe move it to a different class.
        private static void CreateTileContent()
        {
            TileContent content = new TileContent()
            {
                Visual = new TileVisual()
                {

                    TileWide = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = "Jennifer Parker",
                                        HintStyle = AdaptiveTextStyle.Subtitle
                                    },

                                    new AdaptiveText()
                                    {
                                        Text = "Photos from our trip",
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                    },

                                    new AdaptiveText()
                                    {
                                        Text = "Check out these awesome photos I took while in New Zealand!",
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                    }
                                }
                        }
                    },

                }
            };
        }


        public void RegisterBackGroundTask()
        {
            BackgroundTaskManager backgroundTaskManager = new BackgroundTaskManager();
            var backGroundTask = backgroundTaskManager.RegisterBackGroundTask().Result;

            backGroundTask.Completed += BackGroundTask_Completed;

        }

        private void BackGroundTask_Completed(Windows.ApplicationModel.Background.BackgroundTaskRegistration sender, Windows.ApplicationModel.Background.BackgroundTaskCompletedEventArgs args)
        {
            UpdateTile();
        }
    }
}
