using System;
using MyContacts.Models;
using Xamarin.Essentials;

namespace MyContacts.Util
{

    public static class Settings
    {
        public static DateTime LastUpdate
        {
            get => Preferences.Get(nameof(LastUpdate), DateTime.UtcNow);
            set => Preferences.Set(nameof(LastUpdate), value);
        }
        public static Theme ThemeOption
        {
            get => (Theme)Preferences.Get(nameof(ThemeOption), HasDefaultThemeOption ? (int)Theme.Default : (int)Theme.Light);
            set => Preferences.Set(nameof(ThemeOption), (int)value);
        }

        public static bool HasDefaultThemeOption
        {
            get
            {
                var minDefaultVersion = new Version(13, 0);
                if (DeviceInfo.Platform == DevicePlatform.UWP)
                    minDefaultVersion = new Version(10, 0, 17763, 1);
                else if (DeviceInfo.Platform == DevicePlatform.Android)
                    minDefaultVersion = new Version(10, 0);

                return DeviceInfo.Version >= minDefaultVersion;
            }
        }

        public const string BingMapsKey = "UW0peICp3gljJyhqQKFZ~R3XF1I5BvWmWmkD4ujytTA~AoUOqpk2nJB-Wh7wH-9S-zaG-w6sygLitXugNOqm71wx_nc6WHIt6Lb29gyTU04X";

    }
}

