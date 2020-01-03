using System.Threading.Tasks;
using Xamarin.Forms;
using Acquaint.Models;
using Acquaint.Extensions;
using Acquaint.Services;
using Acquaint.Constants;

namespace Acquaint.ViewModels
{
    public class EditViewModel : ViewModelBase
	{
		bool isNewAcquaintance;

        public EditViewModel()
        {
            Acquaintance = new Acquaintance();
            isNewAcquaintance = true;
            Title = "New Acquaintance";
        }
		public EditViewModel(Acquaintance acquaintance)
		{
			if (acquaintance == null)
			{
				Acquaintance = new Acquaintance();
				isNewAcquaintance = true;
                Title = "New Acquaintance";
            }
			else
			{
                Acquaintance = acquaintance.Clone();
                Title = "Edit Acquaintance";
            }


		}

		public Acquaintance Acquaintance { private set; get; }

		Command saveAcquaintanceCommand;

		public Command SaveAcquaintanceCommand => saveAcquaintanceCommand ?? (saveAcquaintanceCommand = new Command(async () => await ExecuteSaveAcquaintanceCommand()));

		async Task ExecuteSaveAcquaintanceCommand()
		{
			if (string.IsNullOrWhiteSpace(Acquaintance.LastName) || string.IsNullOrWhiteSpace(Acquaintance.FirstName))
			{
				MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.DisplayAlert, new MessagingServiceAlert()
					{
						Title = "Invalid name!", 
						Message = "An acquaintance must have both a first and last name.",
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

			if (isNewAcquaintance)
			{
				MessagingService.Current.SendMessage<Acquaintance>(MessageKeys.AddAcquaintance, Acquaintance);
			}
			else 
			{
				MessagingService.Current.SendMessage<Acquaintance>(MessageKeys.UpdateAcquaintance, Acquaintance);
			}
			await PopAsync();
		}

		bool RequiredAddressFieldCombinationIsFilled
		{
			get
			{
				if (!Acquaintance.Street.IsNullOrWhiteSpace() && !Acquaintance.City.IsNullOrWhiteSpace() && !Acquaintance.State.IsNullOrWhiteSpace())
				{
					return true;
				}

				if (!Acquaintance.PostalCode.IsNullOrWhiteSpace() && (Acquaintance.Street.IsNullOrWhiteSpace() || Acquaintance.City.IsNullOrWhiteSpace() || Acquaintance.State.IsNullOrWhiteSpace()))
				{
					return true;
				}

				if (Acquaintance.AddressString.IsNullOrWhiteSpace())
				{
					return true;
				}

				return false;
			}
		}
	}
}

