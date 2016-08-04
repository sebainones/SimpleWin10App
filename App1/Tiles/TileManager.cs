using NotificationsExtensions;
using NotificationsExtensions.Tiles;
using Windows.UI.Notifications;

namespace RateApp.Tiles
{
    public class TileManager : ITileManager
    {
        //TODO: INterface segregation??? Between Creation and Update of tiles...

        public string CreateAdaptiveTile(string title, string caption, string value)
        {
            // Create a TileBinding
            var tilebinding = new TileBinding();
            //Create a TileContent
            var tileContent = new TileBindingContentAdaptive();
            tileContent.TextStacking = TileTextStacking.Center;
            //Create a groups and subgroups
            var tileGroup = new AdaptiveGroup();

            var subgroup1 = new AdaptiveSubgroup();

            //Create a subtitle
            var subTitle = new AdaptiveText();
            subTitle.HintStyle = AdaptiveTextStyle.Body;
            subTitle.Text = title;
            subgroup1.Children.Add(subTitle);

            var subgroup2 = new AdaptiveSubgroup();
            //Create a bodysubtile
            var bodyTitle = new AdaptiveText();
            bodyTitle.HintAlign = AdaptiveTextAlign.Left;
            bodyTitle.Text = caption;

            var bodyTitleValue = new AdaptiveText();
            bodyTitleValue.HintAlign = AdaptiveTextAlign.Right;
            bodyTitleValue.Text = value;
            subgroup2.Children.Add(bodyTitle);
            subgroup2.Children.Add(bodyTitleValue);


            //tileGroup.Children.Add(subgroup1);
            tileGroup.Children.Add(subgroup2);

            //tileContent.Children.Add(subTitle);
            //tileContent.Children.Add(bodyTitle);
            //tileContent.Children.Add(bodyTitleValue);

            tileContent.Children.Add(tileGroup);
            tilebinding.Content = tileContent;

            //Create visual object
            var tileVisual = new TileVisual();
            tileVisual.TileWide = tilebinding;

            //Create tile object
            var tileObject = new TileContent();
            tileObject.Visual = tileVisual;

            return tileObject.GetContent();
        }
        
        //TODO: Remove this if not being used. Use it as example for the CreateAdaptiveTile method.
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

        
        public void Update(string xmldocumentinfo)
        {
            var xmldocument = new Windows.Data.Xml.Dom.XmlDocument();
            xmldocument.LoadXml(xmldocumentinfo);
            TileNotification tileNotification = new TileNotification(xmldocument);
            TileUpdater updateMgr = TileUpdateManager.CreateTileUpdaterForApplication();
            updateMgr.Update(tileNotification);
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
