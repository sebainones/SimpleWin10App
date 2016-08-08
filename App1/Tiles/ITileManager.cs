namespace RateApp.Tiles
{
    public interface ITileManager
    {
        string CreateAdaptiveTile(string prefix, string leftTitle, string leftValue, string rightTitle, string rightValuevalue);

        void Update(string xmldocumentinfo);

        void UpdateTile();
    }
}