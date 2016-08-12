using Windows.Storage;

namespace RateApp.LocalStorage
{
    public class LocalStorage : ILocalStorage

    {
        private readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;


        public ApplicationDataContainer CreateContainer(string containerName)
        {
            return localSettings.CreateContainer(containerName, ApplicationDataCreateDisposition.Always);
        }

        public void CreateSetting(ApplicationDataContainer container, string settingName, string settingValue)
        {
            container.Values[settingName] = settingValue;
        }


        //TODO: Refactor this. To be homogeneous!!!
        public string GetValueFromSetting(string containerName, string settingName)
        {
            if (!localSettings.Containers.ContainsKey(containerName))
                return string.Empty;

            var container = localSettings.Containers[containerName];

            if (!container.Values.ContainsKey(settingName))
                return string.Empty;

            return container.Values[settingName].ToString();
        }

        public bool SettingHasValue(string containerName, string settingValue)
        {
            if (!localSettings.Containers.ContainsKey(containerName))
                return false;

            if (localSettings.Containers[containerName].Values[settingValue] == null)
                return false;

            return true;
        }
    }
}
