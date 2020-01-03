using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Acquaint.Constants;
using Acquaint.Extensions;
using Acquaint.Models;
using Acquaint.Services;
using MvvmHelpers.Commands;
using Xamarin.Essentials;

namespace Acquaint.ViewModels
{
    public class AcquainanceViewModel : ViewModelBase
    {

        Command<string> dialNumberCommand;

        public Command<string> DialNumberCommand => 
            dialNumberCommand ??= new Command<string>(ExecuteDialNumberCommand);

        void ExecuteDialNumberCommand(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return;

            try
            {
                PhoneDialer.Open(number.SanitizePhoneNumber());
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

        AsyncCommand<string> messageNumberCommand;

        public AsyncCommand<string> MessageNumberCommand => 
            messageNumberCommand ??= new AsyncCommand<string>(ExecuteMessageNumberCommand);

        async Task ExecuteMessageNumberCommand(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return;

            try
            {
                await Sms.ComposeAsync(new SmsMessage(string.Empty, number.SanitizePhoneNumber()));
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

        AsyncCommand<string> emailCommand;

        public AsyncCommand<string> EmailCommand =>
            emailCommand ??= new AsyncCommand<string>(ExecuteEmailCommandCommand);

        async Task ExecuteEmailCommandCommand(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return;

            try
            {
                await Email.ComposeAsync(string.Empty, string.Empty, email);
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

        AsyncCommand<Acquaintance> getDirectionsCommand;

        public AsyncCommand<Acquaintance> GetDirectionsCommand =>
            getDirectionsCommand ??= new AsyncCommand<Acquaintance>(ExecuteGetDirectionsCommand);


        async Task ExecuteGetDirectionsCommand(Acquaintance acquaintance)
        {
            try
            {
                await Xamarin.Essentials.Map.OpenAsync(new Placemark
                {
                    AdminArea = acquaintance.State,
                    Locality = acquaintance.City,
                    PostalCode = acquaintance.PostalCode,
                    Thoroughfare = acquaintance.AddressString
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
    }
}
