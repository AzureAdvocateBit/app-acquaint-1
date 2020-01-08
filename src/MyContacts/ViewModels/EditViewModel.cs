using System.Threading.Tasks;
using Xamarin.Forms;
using MyContacts.Models;
using MyContacts.Extensions;
using MyContacts.Services;
using MyContacts.Constants;

namespace MyContacts.ViewModels
{
    public class EditViewModel : ViewModelBase
	{
		bool isNewMyContactsance;

        public EditViewModel()
        {
            MyContactsance = new Contact();
            isNewMyContactsance = true;
            Title = "New MyContactsance";
        }
		public EditViewModel(Contact MyContactsance)
		{
			if (MyContactsance == null)
			{
				MyContactsance = new Contact();
				isNewMyContactsance = true;
                Title = "New MyContactsance";
            }
			else
			{
                MyContactsance = MyContactsance.Clone();
                Title = "Edit MyContactsance";
            }


		}

		public Contact MyContactsance { private set; get; }

		Command saveMyContactsanceCommand;

		public Command SaveMyContactsanceCommand => saveMyContactsanceCommand ?? (saveMyContactsanceCommand = new Command(async () => await ExecuteSaveMyContactsanceCommand()));

		async Task ExecuteSaveMyContactsanceCommand()
		{
			if (string.IsNullOrWhiteSpace(MyContactsance.LastName) || string.IsNullOrWhiteSpace(MyContactsance.FirstName))
			{
				MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.DisplayAlert, new MessagingServiceAlert()
					{
						Title = "Invalid name!", 
						Message = "An MyContactsance must have both a first and last name.",
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

			if (isNewMyContactsance)
			{
				MessagingService.Current.SendMessage<Contact>(MessageKeys.AddMyContactsance, MyContactsance);
			}
			else 
			{
				MessagingService.Current.SendMessage<Contact>(MessageKeys.UpdateMyContactsance, MyContactsance);
			}
			await PopAsync();
		}

		bool RequiredAddressFieldCombinationIsFilled
		{
			get
			{
				if (!MyContactsance.Street.IsNullOrWhiteSpace() && !MyContactsance.City.IsNullOrWhiteSpace() && !MyContactsance.State.IsNullOrWhiteSpace())
				{
					return true;
				}

				if (!MyContactsance.PostalCode.IsNullOrWhiteSpace() && (MyContactsance.Street.IsNullOrWhiteSpace() || MyContactsance.City.IsNullOrWhiteSpace() || MyContactsance.State.IsNullOrWhiteSpace()))
				{
					return true;
				}

				if (MyContactsance.AddressString.IsNullOrWhiteSpace())
				{
					return true;
				}

				return false;
			}
		}
	}
}

