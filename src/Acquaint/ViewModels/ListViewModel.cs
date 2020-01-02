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

            LoadCommand = new AsyncCommand(ExecuteLoadCommand);

            RefreshCommand = new AsyncCommand(ExecuteRefreshCommand);

            NewCommand = new AsyncCommand(ExecuteNewCommand);
        }


        public ObservableRangeCollection<Acquaintance> Acquaintances { get; } = new ObservableRangeCollection<Acquaintance>();

        /// <summary>
        /// Command to load acquaintances
        /// </summary>
        public AsyncCommand LoadCommand { get; }

        public async Task ExecuteLoadCommand()
        {
            LoadCommand.RaiseCanExecuteChanged();


            if (Settings.LocalDataResetIsRequested)
                Acquaintances.Clear();

            if (Acquaintances.Count < 1 || !Settings.DataIsSeeded || Settings.ClearImageCacheIsRequested)
                await FetchAcquaintances();

            LoadCommand.RaiseCanExecuteChanged();
        }

        public AsyncCommand RefreshCommand { get; }

        async Task ExecuteRefreshCommand()
        {
            RefreshCommand.RaiseCanExecuteChanged();

            await FetchAcquaintances();

            RefreshCommand.RaiseCanExecuteChanged();
        }

        async Task FetchAcquaintances()
        {
            IsBusy = true;

            var items = await DataSource.GetItems();

            Acquaintances.ReplaceRange(items);

            // ensuring that this flag is reset
            Settings.ClearImageCacheIsRequested = false;

            IsBusy = false;
        }

        /// <summary>
        /// Command to create new acquaintance
        /// </summary>
        public AsyncCommand NewCommand { get; }
        Task ExecuteNewCommand() => PushAsync(new EditPage());

        /// <summary>
        /// Command to show settings
        /// </summary>
        public AsyncCommand ShowSettingsCommand { get; }

        Task ExecuteShowSettingsCommand()
        {
            var navPage = new NavigationPage(
                new SettingsPage())
            {
                BarBackgroundColor = Color.FromHex("547799")
            };
            
            navPage.BarTextColor = Color.White;

            return PushModalAsync(navPage);
        }

        Command _DialNumberCommand;

        /// <summary>
        /// Command to dial acquaintance phone number
        /// </summary>
        public Command DialNumberCommand
        {
            get
            {
                return _DialNumberCommand ??
                (_DialNumberCommand = new Command((parameter) =>
                        ExecuteDialNumberCommand((string)parameter)));
            }
        }

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
        public Command MessageNumberCommand
        {
            get
            {
                return messageNumberCommand ??
                (messageNumberCommand = new Command((parameter) =>
                        ExecuteMessageNumberCommand((string)parameter)));
            }
        }

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

        /// <summary>
        /// Command to email acquaintance
        /// </summary>
        public Command EmailCommand
        {
            get
            {
                return emailCommand ??
                (emailCommand = new Command((parameter) =>
                        ExecuteEmailCommand((string)parameter)));
            }
        }

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

