using System;
using Acquaint;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AcquaintAndroid
{
    [Activity (Icon = "@drawable/icon", Label = "@string/app_name",
        Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity
	{

		protected override void OnCreate (Bundle savedInstanceState)
		{
            ToolbarResource = Resource.Layout.Toolbar;
            TabLayoutResource = Resource.Layout.Tabbar;

			base.OnCreate (savedInstanceState);

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

			Forms.Init (this, savedInstanceState);
			FormsMaps.Init (this, savedInstanceState);
            FormsMaterial.Init(this, savedInstanceState);
            //Android.Glide.Forms.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            LoadApplication (new App ());
		}

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
