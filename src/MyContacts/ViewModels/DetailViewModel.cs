using System;
using System.Threading.Tasks;
using MyContacts.Constants;
using MyContacts.Models;
using MyContacts.Services;
using MyContacts.Views;
using MvvmHelpers.Commands;
using Command = Xamarin.Forms.Command;
using MyContacts.Shared.Models;

namespace MyContacts.ViewModels
{
    public class DetailViewModel : ContactViewModel
    {
        public DetailViewModel()
        {

        }
        public DetailViewModel(Contact contact)
        {
            Contact = contact;

            SubscribeToSaveContactsMessages();
        }

        public Contact Contact { get; private set; }

        public bool HasEmailAddress => !string.IsNullOrWhiteSpace(Contact?.Email);

        public bool HasPhoneNumber => !string.IsNullOrWhiteSpace(Contact?.Phone);

        public bool HasAddress => !string.IsNullOrWhiteSpace(Contact?.AddressString);


        AsyncCommand editCommand;

        public AsyncCommand EditCommand =>
            editCommand ??= new AsyncCommand(ExecuteEditCommand);

        Task ExecuteEditCommand() => PushAsync(new EditPage(Contact));

        Command deleteCommand;

        public Command DeleteCommand => deleteCommand ?? (deleteCommand = new Command(ExecuteDeleteCommand));

        void ExecuteDeleteCommand()
        {
            MessagingService.Current.SendMessage<MessagingServiceQuestion>(MessageKeys.DisplayQuestion, new MessagingServiceQuestion()
            {
                Title = string.Format("Delete {0}?", Contact.DisplayName),
                Question = null,
                Positive = "Delete",
                Negative = "Cancel",
                OnCompleted = new Action<bool>(async result =>
                {
                    if (!result) return;

                    // send a message that we want the given MyContacts to be deleted
                    MessagingService.Current.SendMessage<Contact>(MessageKeys.DeleteContact, Contact);

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
        void SubscribeToSaveContactsMessages()
        {
            // This subscribes to the "SaveMyContacts" message
            MessagingService.Current.Subscribe<Contact>(MessageKeys.UpdateContact, (service, contact) =>
                {
                    Contact = contact;
                    OnPropertyChanged("MyContacts");

                    MessagingService.Current.SendMessage<Contact>(MessageKeys.ContactLocationUpdated, Contact);
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

