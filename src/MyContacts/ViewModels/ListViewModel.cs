using System;
using System.Linq;
using System.Threading.Tasks;
using MyContacts.Interfaces;
using MyContacts.Models;
using MyContacts.Util;
using MyContacts.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace MyContacts.ViewModels
{
    public class ListViewModel : AcquainanceViewModel
    {
        public DateTime LastUpdate { get; set; }
        public ListViewModel()
        {
        }


        public ObservableRangeCollection<Contact> MyContacts { get; } = new ObservableRangeCollection<Contact>();

        AsyncCommand loadCommand;
        public AsyncCommand LoadCommand => loadCommand ??=
            new AsyncCommand(ExecuteLoadCommand);

        public async Task ExecuteLoadCommand()
        { 
            if (MyContacts.Count < 1 || LastUpdate < Settings.LastUpdate)
                await FetchMyContacts();
        }

        AsyncCommand refreshCommand;
        public AsyncCommand RefreshCommand => refreshCommand ??=
            new AsyncCommand(ExecuteRefreshCommand);

        async Task ExecuteRefreshCommand()
        {
            await FetchMyContacts();
        }

        async Task FetchMyContacts()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            await Task.Delay(1000);
            var items = await DataSource.GetItems();

            MyContacts.ReplaceRange(items);

            LastUpdate = DateTime.UtcNow;

            IsBusy = false;
        }

        AsyncCommand newCommand;
        public AsyncCommand NewCommand => newCommand ??=
            new AsyncCommand(ExecuteNewCommand);
        Task ExecuteNewCommand() => PushAsync(new EditPage());

        AsyncCommand showSettingsCommand;
        public AsyncCommand ShowSettingsCommand => showSettingsCommand ??=
            new AsyncCommand(ExecuteShowSettingsCommand);

        Task ExecuteShowSettingsCommand() => PushModalAsync(new SettingsPage());
    }
}

