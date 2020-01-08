using System;
using MyContacts.Constants;
using MyContacts.Services;
using MyContacts.ViewModels;
using Xamarin.Forms;

namespace MyContacts.Views
{
    public partial class SettingsPage : ContentPage
	{
		protected SettingsViewModel ViewModel => BindingContext as SettingsViewModel;

		public SettingsPage()
		{
			InitializeComponent();

            BindingContext = new SettingsViewModel();
        }
        async void CloseButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}

