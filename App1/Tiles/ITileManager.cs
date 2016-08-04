namespace RateApp.Tiles
{
    public interface ITileManager
    {
        string CreateAdaptiveTile(string title, string caption, string value);

        void Update(string xmldocumentinfo);

        void UpdateTile();
    }
}