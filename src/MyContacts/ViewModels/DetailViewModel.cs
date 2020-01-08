using System;
using System.Threading.Tasks;
using MyContacts.Constants;
using MyContacts.Models;
using MyContacts.Services;
using MyContacts.Views;
using MvvmHelpers.Commands;
using Command = Xamarin.Forms.Command;

namespace MyContacts.ViewModels
{
    public class DetailViewModel : AcquainanceViewModel
    {
        public DetailViewModel()
        {

        }
        public DetailViewModel(Contact MyContactsance)
        {
            MyContactsance = MyContactsance;

            SubscribeToSaveMyContactsanceMessages();
        }

        public Contact MyContactsance { private set; get; }

        public bool HasEmailAddress => !string.IsNullOrWhiteSpace(MyContactsance?.Email);

        public bool HasPhoneNumber => !string.IsNullOrWhiteSpace(MyContactsance?.Phone);

        public bool HasAddress => !string.IsNullOrWhiteSpace(MyContactsance?.AddressString);


        AsyncCommand editMyContactsanceCommand;

        public AsyncCommand EditMyContactsanceCommand =>
            editMyContactsanceCommand ??= new AsyncCommand(ExecuteEditMyContactsanceCommand);

        Task ExecuteEditMyContactsanceCommand() => PushAsync(new EditPage(MyContactsance));

        Command deleteMyContactsanceCommand;

        public Command DeleteMyContactsanceCommand => deleteMyContactsanceCommand ?? (deleteMyContactsanceCommand = new Command(ExecuteDeleteMyContactsanceCommand));

        void ExecuteDeleteMyContactsanceCommand()
        {
            MessagingService.Current.SendMessage<MessagingServiceQuestion>(MessageKeys.DisplayQuestion, new MessagingServiceQuestion()
            {
                Title = string.Format("Delete {0}?", MyContactsance.DisplayName),
                Question = null,
                Positive = "Delete",
                Negative = "Cancel",
                OnCompleted = new Action<bool>(async result =>
                {
                    if (!result) return;

                    // send a message that we want the given MyContactsance to be deleted
                    MessagingService.Current.SendMessage<Contact>(MessageKeys.DeleteMyContactsance, MyContactsance);

                    await PopAsync();
                })
            });
        }

        public void SetupMap()
        {
            if (HasAddress)
            {
                MessagingService.Current.SendMessage(MessageKeys.SetupMap);
            }
        }
        void SubscribeToSaveMyContactsanceMessages()
        {
            // This subscribes to the "SaveMyContactsance" message
            MessagingService.Current.Subscribe<Contact>(MessageKeys.UpdateMyContactsance, (service, MyContactsance) =>
                {
                    MyContactsance = MyContactsance;
                    OnPropertyChanged("MyContactsance");

                    MessagingService.Current.SendMessage<Contact>(MessageKeys.MyContactsanceLocationUpdated, MyContactsance);
                });
        }

        public void DisplayGeocodingError()
        {
            MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.DisplayAlert, new MessagingServiceAlert()
            {
                Title = "Geocoding Error",
                Message = "Please make sure the address is valid, or that you have a network connection.",
                Cancel = "OK"
            });

            IsBusy = false;
        }
    }
}

