using NotificationsExtensions;
using NotificationsExtensions.Tiles;
using Windows.UI.Notifications;

namespace RateApp.Tiles
{
    //TODO: INterface segregation??? Between Creation and Update of tiles...
    public class TileManager : ITileManager
    {
        public string CreateAdaptiveTile(string prefix, string leftTitle, string leftValue, string rightTitle, string rightValuevalue)
        {
            // Create a TileBinding for Large and Wide
            var bigTilebinding = new TileBinding();
            bigTilebinding.Content = CreateTileContent(prefix, leftTitle, leftValue, rightTitle, rightValuevalue);

            var mediumTileBinding = new TileBinding();
            mediumTileBinding.Content = CreateTileContent(string.Empty, leftTitle, leftValue, rightTitle, rightValuevalue);

            //Create visual object
            var tileVisual = new TileVisual();
            tileVisual.TileLarge = tileVisual.TileWide = bigTilebinding;

            tileVisual.TileMedium = mediumTileBinding;

            //Create tile object
            var tileObject = new TileContent();
            tileObject.Visual = tileVisual;

            return tileObject.GetContent();
        }

        private static TileBindingContentAdaptive CreateTileContent(string prefix, string leftTitle, string leftValue, string rightTitle, string rightValuevalue)
        {
            //Create a TileContent
            var tileContent = new TileBindingContentAdaptive();
            tileContent.TextStacking = TileTextStacking.Center;
            //Create a groups and subgroups
            var tileGroup = new AdaptiveGroup();

            AdaptiveSubgroup columnGroupLeft = CreateAdaptiveGroupColumn(prefix + " " + leftTitle, leftValue);
            AdaptiveSubgroup CoulumnGroupRight = CreateAdaptiveGroupColumn(prefix + " " + rightTitle, rightValuevalue);

            tileGroup.Children.Add(columnGroupLeft);
            tileGroup.Children.Add(CoulumnGroupRight);


            tileContent.Children.Add(tileGroup);
            return tileContent;
        }

        private static AdaptiveSubgroup CreateAdaptiveGroupColumn(string title, string value)
        {
            AdaptiveSubgroup adaptiveColumnGroup = new AdaptiveSubgroup();
            var contentTitle = new AdaptiveText();
            contentTitle.Text = title;

            var contenteValue = new AdaptiveText();
            contenteValue.Text = value;

            adaptiveColumnGroup.Children.Add(contentTitle);
            adaptiveColumnGroup.Children.Add(contenteValue);
            return adaptiveColumnGroup;
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
