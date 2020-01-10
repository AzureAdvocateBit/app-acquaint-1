using System.Threading.Tasks;
using Xamarin.Forms;
using MyContacts.Models;
using MyContacts.Extensions;
using MyContacts.Services;
using MyContacts.Constants;
using MyContacts.Shared.Models;

namespace MyContacts.ViewModels
{
    public class EditViewModel : ViewModelBase
	{
		bool isNewMyContacts;

        public EditViewModel()
        {
            Contact = new Contact();
            isNewMyContacts = true;
            Title = "New MyContacts";
        }
		public EditViewModel(Contact contact)
		{
			if (contact == null)
			{
                Contact = new Contact();
				isNewMyContacts = true;
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
				MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.DisplayAlert, new MessagingServiceAlert()
					{
						Title = "Invalid name!", 
						Message = "An MyContacts must have both a first and last name.",
						Cancel = "OK"
					});
				return;
			}

			if (!RequiredAddressFieldCombinationIsFilled)
			{
				MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.DisplayAlert, new MessagingServiceAlert()
					{
						Title = "Invalid address!", 
						Message = "You must enter either a street, city, and state combination, or a postal code.",
						Cancel = "OK"
					});
				return;
			}

			if (isNewMyContacts)
			{
				MessagingService.Current.SendMessage<Contact>(MessageKeys.AddContact, Contact);
			}
			else 
			{
				MessagingService.Current.SendMessage<Contact>(MessageKeys.UpdateContact, Contact);
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

