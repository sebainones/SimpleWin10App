using Windows.Storage;

namespace RateApp.LocalStorage
{
    public interface ILocalStorage
    {
        ApplicationDataContainer CreateContainer(string containerName);

        void CreateSetting(ApplicationDataContainer container, string settingName, string settingValue);

        string GetValueFromSetting(string containerName, string settingName);

        bool SettingHasValue(string containerName, string settingValue);

    }
}