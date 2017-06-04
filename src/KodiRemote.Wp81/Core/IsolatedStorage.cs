using System;
using System.IO.IsolatedStorage;

namespace KodiRemote.Wp81.Core
{
    public static class IsolatedStorage
    {
        public static T ReadFromIsolatedStorage<T>(string key, T defaultValue)
        {
            var settings = IsolatedStorageSettings.ApplicationSettings;

            try
            {
                if (settings.Contains(key))
                    return (T)settings[key];
            }
            catch (Exception) { }

            return defaultValue;
        }

        public static void SaveToIsolatedStorage(string key, object value)
        {
            var settings = IsolatedStorageSettings.ApplicationSettings;

            if (settings.Contains(key))
                settings[key] = value;
            else
                settings.Add(key, value);

            settings.Save();
        }
    }
}