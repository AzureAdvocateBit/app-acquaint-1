using System;
using Xamarin.Essentials;

namespace Acquaint.Util
{
    /// <summary>
    /// This class uses the Xamarin settings plugin (Plugins.Settings) to implement cross-platform storing of settings.
    /// </summary>
    public static class Settings
	{
		public static void ResetUserConfigurableSettingsToDefaults()
		{
			DataIsSeeded = DataIsSeededDefault;
			LocalDataResetIsRequested = true;
			AzureAppServiceUrl = AzureAppServiceUrlDefault;
			ImageCacheDurationHours = ImageCacheDurationHoursDefault;
		}

		public static bool IsUsingLocalDataSource => DataPartitionPhrase == "UseLocalDataSource";

		public static event EventHandler OnDataPartitionPhraseChanged;

		/// <summary>
		/// Raises the data parition phrase changed event.
		/// </summary>
		/// <param name="e">E.</param>
		static void RaiseDataParitionPhraseChangedEvent(EventArgs e)
		{
			var handler = OnDataPartitionPhraseChanged;

			if (handler != null)
				handler(null, e);
		}

		#region user-configurable

		private const string AzureAppServiceUrlKey = "AzureAppServiceUrl_key";
		private static readonly string AzureAppServiceUrlDefault = "https://app-acquaint.azurewebsites.net";

		private const string DataPartitionPhraseKey = "DataPartitionPhrase_key";
		private static readonly string DataSeedPhraseDefault = "";

		private const string ImageCacheDurationHoursKey = "ImageCacheDurationHours_key";
		public static readonly int ImageCacheDurationHoursDefault = 1;

		public static string AzureAppServiceUrl
        {
            get => Preferences.Get(AzureAppServiceUrlKey, AzureAppServiceUrlDefault);
            set => Preferences.Set(AzureAppServiceUrlKey, value);
        }

        public static string DataPartitionPhrase
        {
            get => Preferences.Get(DataPartitionPhraseKey, DataSeedPhraseDefault);
            set
            {
                Preferences.Set(DataPartitionPhraseKey, value);
                RaiseDataParitionPhraseChangedEvent(null);
            }
        }

        public static int ImageCacheDurationHours
        {
            get => Preferences.Get(ImageCacheDurationHoursKey, ImageCacheDurationHoursDefault);
            set => Preferences.Set(ImageCacheDurationHoursKey, value);
        }

        #endregion


        #region non-user-configurable

        private const string DataIsSeededKey = "DataIsSeeded_key";
		private static readonly bool DataIsSeededDefault = false;

		private const string LocalDataResetIsRequestedKey = "LocalDataResetIsRequested_key";
		private static readonly bool LocalDataResetIsRequestedDefault = false;

		private const string ClearImageCacheIsRequestedKey = "ClearImageCacheIsRequested_key";
		private static readonly bool ClearImageCacheIsRequestedDefault = false;

        private const string BingMapsKeyKey = "BingMapsKey_key";
        private static readonly string BingMapsKeyDefault = "UW0peICp3gljJyhqQKFZ~R3XF1I5BvWmWmkD4ujytTA~AoUOqpk2nJB-Wh7wH-9S-zaG-w6sygLitXugNOqm71wx_nc6WHIt6Lb29gyTU04X";

		public static bool LocalDataResetIsRequested
        {
            get => Preferences.Get(LocalDataResetIsRequestedKey, LocalDataResetIsRequestedDefault);
            set => Preferences.Set(LocalDataResetIsRequestedKey, value);
        }

        public static bool ClearImageCacheIsRequested
        {
            get => Preferences.Get(ClearImageCacheIsRequestedKey, ClearImageCacheIsRequestedDefault);
            set => Preferences.Set(ClearImageCacheIsRequestedKey, value);
        }

        public static bool DataIsSeeded
        {
            get => Preferences.Get(DataIsSeededKey, DataIsSeededDefault);
            set => Preferences.Set(DataIsSeededKey, value);
        }

        public static string BingMapsKey
        {
            get => Preferences.Get(BingMapsKeyKey, BingMapsKeyDefault);
            set => Preferences.Set(BingMapsKeyKey, value);
        }

        #endregion
    }
}

