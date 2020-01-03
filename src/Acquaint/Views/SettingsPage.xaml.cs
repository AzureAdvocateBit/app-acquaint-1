using System;
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
        }
        async void CloseButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}

