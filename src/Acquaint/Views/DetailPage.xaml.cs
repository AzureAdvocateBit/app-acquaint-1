using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Threading.Tasks;
using System;
using Acquaint.ViewModels;
using Xamarin.Essentials;
using System.Linq;
using Acquaint.Models;

namespace Acquaint.Views
{
    public partial class DetailPage : ContentPage
    {
        protected DetailViewModel ViewModel => BindingContext as DetailViewModel;

        public DetailPage()
        {
            InitializeComponent();
        }

        public DetailPage(Acquaintance acquaintance)
        {
            InitializeComponent();
            BindingContext = new DetailViewModel (acquaintance);
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
                AcquaintanceMap.IsVisible = false;

                // set to a default position
                Location position;

                try
                {
                    position = (await Geocoding.GetLocationsAsync(ViewModel.Acquaintance.AddressString)).FirstOrDefault();
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
                        Label = ViewModel.Acquaintance.DisplayName,
                        Address = ViewModel.Acquaintance.AddressString
                    };

                    AcquaintanceMap.Pins.Clear();

                    AcquaintanceMap.Pins.Add(pin);
                }

                AcquaintanceMap.MoveToRegion(MapSpan.FromCenterAndRadius(xfpos, Distance.FromMiles(10)));

                AcquaintanceMap.IsVisible = true;
            }
        }
    }
}

