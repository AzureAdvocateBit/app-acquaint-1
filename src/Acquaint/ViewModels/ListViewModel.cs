using System.Linq;
using System.Threading.Tasks;
using Acquaint.Interfaces;
using Acquaint.Models;
using Acquaint.Util;
using Acquaint.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace Acquaint.ViewModels
{
    public class ListViewModel : ViewModelBase
    {
        public ListViewModel()
        {
            SubscribeToAddAcquaintanceMessages();

            SubscribeToUpdateAcquaintanceMessages();

            SubscribeToDeleteAcquaintanceMessages();
        }


        public ObservableRangeCollection<Acquaintance> Acquaintances { get; } = new ObservableRangeCollection<Acquaintance>();

        AsyncCommand loadCommand;
        public AsyncCommand LoadCommand => loadCommand ??=
            new AsyncCommand(ExecuteLoadCommand);

        public async Task ExecuteLoadCommand()
        { 
            if (Acquaintances.Count < 1)
                await FetchAcquaintances();
        }

        AsyncCommand refreshCommand;
        public AsyncCommand RefreshCommand => refreshCommand ??=
            new AsyncCommand(ExecuteRefreshCommand);

        async Task ExecuteRefreshCommand()
        {
            await FetchAcquaintances();
        }

        async Task FetchAcquaintances()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            await Task.Delay(1000);
            var items = await DataSource.GetItems();

            Acquaintances.ReplaceRange(items);


            IsBusy = false;
        }

        AsyncCommand newCommand;
        public AsyncCommand NewCommand => newCommand ??=
            new AsyncCommand(ExecuteNewCommand);
        Task ExecuteNewCommand() => PushAsync(new EditPage());

        AsyncCommand showSettingsCommand;
        public AsyncCommand ShowSettingsCommand => showSettingsCommand ??=
            new AsyncCommand(ExecuteShowSettingsCommand);

        Task ExecuteShowSettingsCommand() => PushModalAsync(new SettingsPage());

        Command dialNumberCommand;

        /// <summary>
        /// Command to dial acquaintance phone number
        /// </summary>
        public Command DialNumberCommand => dialNumberCommand ??=
            new Command((parameter) =>ExecuteDialNumberCommand((string)parameter));

        void ExecuteDialNumberCommand(string acquaintanceId)
        {
            if (string.IsNullOrWhiteSpace(acquaintanceId))
                return;

            var acquaintance = Acquaintances.SingleOrDefault(c => c.Id == acquaintanceId);

            if (acquaintance == null)
                return;

            // TODO: Update this 
            /*if (_CapabilityService.CanMakeCalls)
            {
                var phoneCallTask = MessagingPlugin.PhoneDialer;
                if (phoneCallTask.CanMakePhoneCall)
                    phoneCallTask.MakePhoneCall(acquaintance.Phone.SanitizePhoneNumber());
            }
            else
            {
                MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.DisplayAlert, new MessagingServiceAlert()
                {
                    Title = "Simulator Not Supported",
                    Message = "Phone calls are not supported in the iOS simulator.",
                    Cancel = "OK"
                });
            }*/
        }

        Command messageNumberCommand;

        /// <summary>
        /// Command to message acquaintance phone number
        /// </summary>
        public Command MessageNumberCommand => messageNumberCommand ??=
                    new Command((parameter) => ExecuteMessageNumberCommand((string)parameter));

        void ExecuteMessageNumberCommand(string acquaintanceId)
        {
            if (string.IsNullOrWhiteSpace(acquaintanceId))
                return;

            var acquaintance = Acquaintances.SingleOrDefault(c => c.Id == acquaintanceId);

            if (acquaintance == null)
                return;

            /*if (_CapabilityService.CanSendMessages)
            {
                var messageTask = MessagingPlugin.SmsMessenger;
                if (messageTask.CanSendSms)
                    messageTask.SendSms(acquaintance.Phone.SanitizePhoneNumber());
            }
            else
            {
                MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.DisplayAlert, new MessagingServiceAlert()
                {
                    Title = "Simulator Not Supported",
                    Message = "Messaging is not supported in the iOS simulator.",
                    Cancel = "OK"
                });
            }*/
        }

        Command emailCommand;
        public Command EmailCommand => emailCommand ??=
               new Command((parameter) => ExecuteEmailCommand((string)parameter));

        void ExecuteEmailCommand(string acquaintanceId)
        {
            if (string.IsNullOrWhiteSpace(acquaintanceId))
                return;

            var acquaintance = Acquaintances.SingleOrDefault(c => c.Id == acquaintanceId);

            if (acquaintance == null)
                return;

            /*if (_CapabilityService.CanSendEmail)
            {
                var emailTask = MessagingPlugin.EmailMessenger;
                if (emailTask.CanSendEmail)
                    emailTask.SendEmail(acquaintance.Email);
            }
            else
            {
                MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.DisplayAlert, new MessagingServiceAlert()
                {
                    Title = "Simulator Not Supported",
                    Message = "Email composition is not supported in the iOS simulator.",
                    Cancel = "OK"
                });
            }*/
        }

        /// <summary>
        /// Subscribes to "AddAcquaintance" messages
        /// </summary>
        void SubscribeToAddAcquaintanceMessages()
        {

            /*MessagingService.Current.Subscribe<Acquaintance>(MessageKeys.AddAcquaintance, async (service, acquaintance) =>
            {
                IsBusy = true;

                await _DataSource.AddItem(acquaintance);

                await FetchAcquaintances();

                IsBusy = false;
            });*/
        }

        /// <summary>
        /// Subscribes to "UpdateAcquaintance" messages
        /// </summary>
        void SubscribeToUpdateAcquaintanceMessages()
        {
            /*MessagingService.Current.Subscribe<Acquaintance>(MessageKeys.UpdateAcquaintance, async (service, acquaintance) =>
            {
                IsBusy = true;

                await _DataSource.UpdateItem(acquaintance);

                await FetchAcquaintances();

                IsBusy = false;
            });*/
        }

        /// <summary>
        /// Subscribes to "DeleteAcquaintance" messages
        /// </summary>
        void SubscribeToDeleteAcquaintanceMessages()
        {
            /*MessagingService.Current.Subscribe<Acquaintance>(MessageKeys.DeleteAcquaintance, async (service, acquaintance) =>
            {
                IsBusy = true;

                await _DataSource.RemoveItem(acquaintance);

                await FetchAcquaintances();

                IsBusy = false;
            });*/
        }
    }
}

