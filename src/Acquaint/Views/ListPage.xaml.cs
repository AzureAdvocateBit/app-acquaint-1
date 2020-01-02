using System;
using Xamarin.Forms;
using Acquaint.Util;
using Acquaint.Models;
using Acquaint.ViewModels;

namespace Acquaint.Views
{
	public partial class ListPage : ContentPage
	{
		protected ListViewModel ViewModel => BindingContext as ListViewModel;

		public ListPage()
		{
			InitializeComponent();

		    if (Device.OS != TargetPlatform.Windows)
		    {
		        ToolbarItems.Remove(refreshToolbarItem);
		    }
		}

		/// <summary>
		/// The action to take when a list item is tapped.
		/// </summary>
		/// <param name="sender"> The sender.</param>
		/// <param name="e">The ItemTappedEventArgs</param>
		void ItemTapped(object sender, ItemTappedEventArgs e)
		{
			Navigation.PushAsync(new DetailPage() { BindingContext = new DetailViewModel((Acquaintance)e.Item) });

            // prevents the list from displaying the navigated item as selected when navigating back to the list
			((ListView)sender).SelectedItem = null;
		}

		/// <summary>
		/// The action to take when the + ToolbarItem is clicked on Android.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The EventArgs</param>
		void AndroidAddButtonClicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new EditPage() { BindingContext = new EditViewModel() });
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await ViewModel.ExecuteLoadCommand();		
		}
	}
}

