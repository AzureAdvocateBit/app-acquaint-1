using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Acquaint.Constants;
using Acquaint.Extensions;
using Acquaint.Models;
using Acquaint.Services;
using Acquaint.Views;
using MvvmHelpers.Commands;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Command = Xamarin.Forms.Command;

namespace Acquaint.ViewModels
{
    public class DetailViewModel : ViewModelBase
    {
        public DetailViewModel()
        {

        }
		public DetailViewModel(Acquaintance acquaintance)
		{
			Acquaintance = acquaintance;

			SubscribeToSaveAcquaintanceMessages();
		}

		public Acquaintance Acquaintance { private set; get; }

		public bool HasEmailAddress => !string.IsNullOrWhiteSpace(Acquaintance?.Email);

		public bool HasPhoneNumber => !string.IsNullOrWhiteSpace(Acquaintance?.Phone);

		public bool HasAddress => !string.IsNullOrWhiteSpace(Acquaintance?.AddressString);


        AsyncCommand editAcquaintanceCommand;

        public AsyncCommand EditAcquaintanceCommand => 
            editAcquaintanceCommand ??= new AsyncCommand(ExecuteEditAcquaintanceCommand);

        Task ExecuteEditAcquaintanceCommand() => PushAsync(new EditPage(Acquaintance));

        Command deleteAcquaintanceCommand;

		public Command DeleteAcquaintanceCommand => deleteAcquaintanceCommand ?? (deleteAcquaintanceCommand = new Command(ExecuteDeleteAcquaintanceCommand));

		void ExecuteDeleteAcquaintanceCommand()
		{
			MessagingService.Current.SendMessage<MessagingServiceQuestion>(MessageKeys.DisplayQuestion, new MessagingServiceQuestion()
			{
				Title = string.Format("Delete {0}?", Acquaintance.DisplayName),
				Question = null,
				Positive = "Delete",
				Negative = "Cancel",
				OnCompleted = new Action<bool>(async result => {
					if (!result) return;

					// send a message that we want the given acquaintance to be deleted
					MessagingService.Current.SendMessage<Acquaintance>(MessageKeys.DeleteAcquaintance, Acquaintance);

					await PopAsync();
				})
			});
		}

        Command dialNumberCommand;

        public Command DialNumberCommand => dialNumberCommand ??= 
            new Command(ExecuteDialNumberCommand);

        void ExecuteDialNumberCommand()
        {
            if (!HasPhoneNumber)
                return;

            try
            {
                PhoneDialer.Open(Acquaintance.Phone.SanitizePhoneNumber());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.DisplayAlert, new MessagingServiceAlert()
                {
                    Title = "Not Supported",
                    Message = "Phone calls are not supported on this device.",
                    Cancel = "OK"
                });
            }

        }

        AsyncCommand messageNumberCommand;

        public AsyncCommand MessageNumberCommand => messageNumberCommand ??=
            new AsyncCommand(ExecuteMessageNumberCommand);

        async Task ExecuteMessageNumberCommand()
        {
            if (!HasPhoneNumber)
                return;

            try
            {
                await Sms.ComposeAsync(new SmsMessage(string.Empty, Acquaintance.Phone.SanitizePhoneNumber()));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.DisplayAlert, new MessagingServiceAlert()
                {
                    Title = "Not Supported",
                    Message = "Sms is not supported on this device.",
                    Cancel = "OK"
                });
            }
        }

        AsyncCommand emailCommand;

        public AsyncCommand EmailCommand => 
            emailCommand ??= new AsyncCommand(ExecuteEmailCommandCommand);

        async Task ExecuteEmailCommandCommand()
        {
            if (string.IsNullOrWhiteSpace(Acquaintance.Email))
                return;

            try
            {
                await Email.ComposeAsync(string.Empty, string.Empty, Acquaintance.Email);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.DisplayAlert, new MessagingServiceAlert()
                {
                    Title = "Not Supported",
                    Message = "Email is not supported on this device.",
                    Cancel = "OK"
                });
            }
        }

        AsyncCommand getDirectionsCommand;

        public AsyncCommand GetDirectionsCommand => 
            getDirectionsCommand ??= new AsyncCommand(ExecuteGetDirectionsCommand);


        async Task ExecuteGetDirectionsCommand()
        {           
            try
            {
                await Xamarin.Essentials.Map.OpenAsync(new Placemark
                {
                    AdminArea = Acquaintance.State,
                    Locality = Acquaintance.City,
                    PostalCode = Acquaintance.PostalCode,
                    Thoroughfare = Acquaintance.AddressString
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.DisplayAlert, new MessagingServiceAlert()
                {
                    Title = "Not Supported",
                    Message = "Unable to open a map application on the device..",
                    Cancel = "OK"
                });
            }
        }

        public void SetupMap()
        {
            if (HasAddress)
            {
                MessagingService.Current.SendMessage(MessageKeys.SetupMap);
            }
        }

        public void DisplayGeocodingError()
        {
            //MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.DisplayAlert, new MessagingServiceAlert()
            //    {
            //        Title = "Geocoding Error", 
            //        Message = "Please make sure the address is valid, or that you have a network connection.",
            //        Cancel = "OK"
            //    });

            IsBusy = false;
        }

        public async Task<Position> GetPosition()
        {
            if (!HasAddress)
                return new Position(0, 0);

            IsBusy = true;

            var p = new Position();

            //TODO: Essentials
            //p = (await _Geocoder.GetPositionsForAddressAsync(Acquaintance.AddressString)).FirstOrDefault();

            // The Android geocoder (the underlying implementation in Android itself) fails with some addresses unless they're rounded to the hundreds.
            // So, this deals with that edge case.
            //if (p.Latitude == 0 && p.Longitude == 0 && AddressBeginsWithNumber(Acquaintance.AddressString) && Device.OS == TargetPlatform.Android)
            //{
            //    var roundedAddress = GetAddressWithRoundedStreetNumber(Acquaintance.AddressString);

            //    p = (await _Geocoder.GetPositionsForAddressAsync(roundedAddress)).FirstOrDefault();
            //}

            IsBusy = false;

            return p;
        }
			
        void SubscribeToSaveAcquaintanceMessages()
        {
            // This subscribes to the "SaveAcquaintance" message
            MessagingService.Current.Subscribe<Acquaintance>(MessageKeys.UpdateAcquaintance, (service, acquaintance) =>
                {
					Acquaintance = acquaintance;
					OnPropertyChanged("Acquaintance");

                	MessagingService.Current.SendMessage<Acquaintance>(MessageKeys.AcquaintanceLocationUpdated, Acquaintance);
                });
        }

        static bool AddressBeginsWithNumber(string address)
        {
            return !string.IsNullOrWhiteSpace(address) && char.IsDigit(address.ToCharArray().First());
        }

        static string GetAddressWithRoundedStreetNumber(string address)
        {
            var endingIndex = GetEndingIndexOfNumericPortionOfAddress(address);

            if (endingIndex == 0)
                return address;

            var originalNumber = 0;
            var roundedNumber = 0;

            int.TryParse(address.Substring(0, endingIndex + 1), out originalNumber);

            if (originalNumber == 0)
                return address;

            roundedNumber = originalNumber.RoundToLowestHundreds();

            return address.Replace(originalNumber.ToString(), roundedNumber.ToString());
        }

        static int GetEndingIndexOfNumericPortionOfAddress(string address)
        {
            var endingIndex = 0;

            for (var i = 0; i < address.Length; i++)
            {
                if (char.IsDigit(address[i]))
                    endingIndex = i;
                else
                    break;
            }

            return endingIndex;
        }
    }
}

