using Acquaint.Constants;
using Acquaint.Services;
using Acquaint.ViewModels;
using Xamarin.Forms;

namespace Acquaint.Views
{
    public partial class SettingsPage : ContentPage
	{
		protected SettingsViewModel ViewModel => BindingContext as SettingsViewModel;

		public SettingsPage()
		{
			InitializeComponent();

            BindingContext = new SettingsViewModel();

            MessagingService.Current.Subscribe(MessageKeys.DataPartitionPhraseValidation, (service) =>
            {
                DataPartitionPhraseEntry.PlaceholderColor = Color.Red;
                DataPartitionPhraseEntry.Focus();
            });

            MessagingService.Current.Subscribe(MessageKeys.BackendUrlValidation, (service) =>
            {
                BackendServiceUrlEntry.PlaceholderColor = Color.Red;
            });
        }
	}
}

