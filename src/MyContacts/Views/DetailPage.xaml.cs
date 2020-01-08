using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Threading.Tasks;
using System;
using MyContacts.ViewModels;
using Xamarin.Essentials;
using System.Linq;
using MyContacts.Models;

namespace MyContacts.Views
{
    public partial class DetailPage : ContentPage
    {
        protected DetailViewModel ViewModel => BindingContext as DetailViewModel;

        public DetailPage()
        {
            InitializeComponent();
        }

        public DetailPage(Contact MyContactsance)
        {
            InitializeComponent();
            BindingContext = new DetailViewModel (MyContactsance);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await SetupMap();
        }

        /// <summary>
        /// Sets up the map.
        /// </summary>
        /// <returns>A Task.</returns>
        async Task SetupMap()
        {
            if (ViewModel.HasAddress)
            {
                MyContactsanceMap.IsVisible = false;

                // set to a default position
                Location position;

                try
                {
                    position = (await Geocoding.GetLocationsAsync(ViewModel.MyContactsance.AddressString)).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    ViewModel.DisplayGeocodingError();
                    return;
                }

                // if lat and lon are both 0, then it's assumed that position acquisition failed
                if (position == null || (position.Latitude == 0 && position.Longitude == 0))
                {
                    ViewModel.DisplayGeocodingError();
                    return;
                }

                var xfpos = new Position(position.Latitude, position.Longitude);

                if (DeviceInfo.Platform != DevicePlatform.UWP)
                {
                    var pin = new Pin()
                    {
                        Type = PinType.Place,
                        Position = xfpos,
                        Label = ViewModel.MyContactsance.DisplayName,
                        Address = ViewModel.MyContactsance.AddressString
                    };

                    MyContactsanceMap.Pins.Clear();

                    MyContactsanceMap.Pins.Add(pin);
                }

                MyContactsanceMap.MoveToRegion(MapSpan.FromCenterAndRadius(xfpos, Distance.FromMiles(10)));

                MyContactsanceMap.IsVisible = true;
            }
        }
    }
}

