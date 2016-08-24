using NotificationsExtensions;
using NotificationsExtensions.Tiles;
using System;
using Windows.UI.Notifications;

namespace RateApp.Tiles
{
    //TODO: INterface segregation??? Between Creation and Update of tiles...
    public class TileManager : ITileManager
    {
        public string CreateAdaptiveTile(string prefix, string leftTitle, string leftValue, string rightTitle, string rightValuevalue, string dateTitle, string dateContent)
        {
            // Create a TileBinding for Large and Wide
            var bigTilebinding = new TileBinding();
            bigTilebinding.Content = CreateTileContent(prefix, leftTitle, leftValue, rightTitle, rightValuevalue, dateTitle, dateContent);

            var mediumTileBinding = new TileBinding();
            mediumTileBinding.Content = CreateTileContent(string.Empty, leftTitle, leftValue, rightTitle, rightValuevalue, string.Empty, dateContent);

            //Create visual object
            var tileVisual = new TileVisual();
            tileVisual.TileLarge = tileVisual.TileWide = bigTilebinding;

            tileVisual.TileMedium = mediumTileBinding;

            //Create tile object
            var tileObject = new TileContent();
            tileObject.Visual = tileVisual;

            return tileObject.GetContent();
        }

        private static TileBindingContentAdaptive CreateTileContent(string prefix, string leftTitle, string leftValue, string rightTitle, string rightValuevalue, string dateTitle, string dateContent)
        {
            //Create a TileContent
            var tileContent = new TileBindingContentAdaptive();
            tileContent.TextStacking = TileTextStacking.Center;
            //Create a groups and subgroups
            var tileGroup = new AdaptiveGroup();

            AdaptiveSubgroup columnGroupLeft = CreateAdaptiveGroupColumn(prefix + " " + leftTitle, leftValue, dateTitle);
            AdaptiveSubgroup CoulumnGroupRight = CreateAdaptiveGroupColumn(prefix + " " + rightTitle, rightValuevalue, dateContent);

            tileGroup.Children.Add(columnGroupLeft);
            tileGroup.Children.Add(CoulumnGroupRight);


            tileContent.Children.Add(tileGroup);
            return tileContent;
        }

        private static AdaptiveSubgroup CreateAdaptiveGroupColumn(string title, string value, string dateValue)
        {
            AdaptiveSubgroup adaptiveColumnGroup = new AdaptiveSubgroup();
            var contentTitle = new AdaptiveText();
            contentTitle.Text = title;

            var contenteValue = new AdaptiveText();
            contenteValue.Text = value;

            var contentDate = new AdaptiveText();
            contentDate.Text = dateValue;

            adaptiveColumnGroup.Children.Add(contentTitle);
            adaptiveColumnGroup.Children.Add(contenteValue);
            adaptiveColumnGroup.Children.Add(contentDate);
            return adaptiveColumnGroup;
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
