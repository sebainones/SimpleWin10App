using Windows.Storage;

namespace RateApp.LocalSettings
{
    public interface ILocalSettings
    {
        ApplicationDataContainer CreateContainer(string containerName);

        void CreateSetting(string containerName, string settingName, string settingValue);

        string TryGetValue(string containerName, string settingName);
        
        void UpdateSetting(string containerName, string settingName, string settingValue);

        void Delete(string name);
    }
}