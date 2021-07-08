using System;
using Windows.Storage;

namespace RateApp.LocalSettings
{
    public class LocalSettings : ILocalSettings
    {
        private readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public ApplicationDataContainer CreateContainer(string containerName)
        {
            return localSettings.CreateContainer(containerName, ApplicationDataCreateDisposition.Always);
        }

        public void CreateSetting(string containerName, string settingName, string settingValue)
        {
            if (localSettings.Containers.ContainsKey(containerName))
            {
                var container = localSettings.Containers[containerName];
                CreateSetting(container, settingName, settingValue);
            }
            else
            {
                var container = CreateContainer(containerName);
                CreateSetting(container, settingName, settingValue);
            }
        }

        public void UpdateSetting(string containerName, string settingName, string settingValue)
        {
            localSettings.Containers[containerName].Values[settingName] = settingValue;
        }

        private void CreateSetting(ApplicationDataContainer container, string settingName, string settingValue)
        {
            container.Values[settingName] = settingValue;
        }

        public string TryGetValue(string containerName, string settingName)
        {
            string value = string.Empty;

            if (SettingHasValue(containerName, settingName))
                value = GetValueFromSetting(containerName, settingName);

            return value;
        }

        //TODO: Refactor this. To be homogeneous!!!
        private string GetValueFromSetting(string containerName, string settingName)
        {
            if (!localSettings.Containers.ContainsKey(containerName))
                return string.Empty;

            var container = localSettings.Containers[containerName];

            if (!container.Values.ContainsKey(settingName))
                return string.Empty;

            return container.Values[settingName].ToString();
        }

        private bool SettingHasValue(string containerName, string settingValue)
        {
            if (!localSettings.Containers.ContainsKey(containerName))
                return false;

            if (localSettings.Containers[containerName].Values[settingValue] == null)
                return false;

            return true;
        }

        public void Delete(string containerName)
        {
            if (localSettings.Containers.ContainsKey(containerName))
                localSettings.DeleteContainer(containerName);
        }
    }
}
