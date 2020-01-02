using System.ComponentModel;
using Acquaint.Models;
using Acquaint.ViewModels;
using Xamarin.Forms;

namespace Acquaint.Views
{
    public partial class EditPage : ContentPage
    {
		protected EditViewModel ViewModel => BindingContext as EditViewModel;

        public EditPage()
        {
            InitializeComponent();

            if (Device.OS == TargetPlatform.iOS)
                Title = null; // because iOS already displays the previous page's title with the back button, we don't want to display it twice.

            BindingContext = new EditViewModel();
        }

        public EditPage(Acquaintance acquaintance) 
        {
            InitializeComponent();

            if (Device.OS == TargetPlatform.iOS)
                Title = null; // because iOS already displays the previous page's title with the back button, we don't want to display it twice.

            BindingContext = new EditViewModel(acquaintance);
        }

        /// <summary>
        /// Ensures the state field has 2 characters at most.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The PropertyChangedEventArgs</param>
        void StateEntry_PropertyChanged (object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                var entryCell = sender as EntryCell;

                var val = entryCell.Text;

				if (val != null)
				{

					if (val.Length > 2)
					{
						val = val.Remove(val.Length - 1);
					}

					entryCell.Text = val.ToUpperInvariant();
				}
            }
        }

        /// <summary>
        /// Ensures the zip code field has 5 characters at most.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The PropertyChangedEventArgs</param>
        void PostalCode_PropertyChanged (object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                var entryCell = sender as EntryCell;

                var val = entryCell.Text;

				if (val != null && val.Length > 5)
                {
                    val = val.Remove(val.Length - 1);
                    entryCell.Text = val;
                }
            }
            
        }
    }
}

