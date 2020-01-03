using System;
using System.Threading.Tasks;
using Acquaint.Constants;
using Acquaint.Models;
using Acquaint.Services;
using Acquaint.Views;
using MvvmHelpers.Commands;
using Command = Xamarin.Forms.Command;

namespace Acquaint.ViewModels
{
    public class DetailViewModel : AcquainanceViewModel
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
                OnCompleted = new Action<bool>(async result =>
                {
                    if (!result) return;

                    // send a message that we want the given acquaintance to be deleted
                    MessagingService.Current.SendMessage<Acquaintance>(MessageKeys.DeleteAcquaintance, Acquaintance);

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

