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

        public const string BingMapsKey = "AoQgJ0XALYBhKAFT_KBoYGYLqc5a4yg6UvM0s43zPkjadSQXxWxBIBLmmNdcql2f";

    }
}

