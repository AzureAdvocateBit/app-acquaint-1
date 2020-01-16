using System.Threading.Tasks;
using Xamarin.Forms;
using MyContacts.Extensions;
using MyContacts.Shared.Models;
using MyContacts.Utils;

namespace MyContacts.ViewModels
{
    public class EditViewModel : ViewModelBase
    {
        readonly bool isNew;

        public EditViewModel()
        {
            Contact = new Contact();
            isNew = true;
            Title = "New MyContacts";
        }
        public EditViewModel(Contact contact)
        {
            if (contact == null)
            {
                Contact = new Contact();
                isNew = true;
                Title = "New MyContacts";
            }
            else
            {
                Contact = contact.Clone();
                Title = "Edit MyContacts";
            }


        }

        public Contact Contact { private set; get; }

        Command saveCommand;

        public Command SaveCommand => saveCommand ?? (saveCommand = new Command(async () => await ExecuteSaveCommand()));

        async Task ExecuteSaveCommand()
        {
            if (string.IsNullOrWhiteSpace(Contact.LastName) || string.IsNullOrWhiteSpace(Contact.FirstName))
            {
                await Dialogs.Alert(new AlertInfo
                {
                    Title = "Invalid name!",
                    Message = "An MyContacts must have both a first and last name.",
                    Cancel = "OK"
                });
                return;
            }

            if (!RequiredAddressFieldCombinationIsFilled)
            {
                await Dialogs.Alert(new AlertInfo
                {
                    Title = "Invalid address!",
                    Message = "You must enter either a street, city, and state combination, or a postal code.",
                    Cancel = "OK"
                });
                return;
            }

            if (isNew)
            {
                await DataSource.AddItem(Contact);
            }
            else
            {
                await DataSource.UpdateItem(Contact);
            }
            await PopAsync();
        }

        bool RequiredAddressFieldCombinationIsFilled
        {
            get
            {
                if (!Contact.Street.IsNullOrWhiteSpace() && !Contact.City.IsNullOrWhiteSpace() && !Contact.State.IsNullOrWhiteSpace())
                {
                    return true;
                }

                if (!Contact.PostalCode.IsNullOrWhiteSpace() && (Contact.Street.IsNullOrWhiteSpace() || Contact.City.IsNullOrWhiteSpace() || Contact.State.IsNullOrWhiteSpace()))
                {
                    return true;
                }

                if (Contact.AddressString.IsNullOrWhiteSpace())
                {
                    return true;
                }

                return false;
            }
        }
    }
}

