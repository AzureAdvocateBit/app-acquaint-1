using System;
using System.Linq;
using System.Threading.Tasks;
using Acquaint.Interfaces;
using Acquaint.Models;
using Acquaint.Util;
using Acquaint.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace Acquaint.ViewModels
{
    public class ListViewModel : AcquainanceViewModel
    {
        public DateTime LastUpdate { get; set; }
        public ListViewModel()
        {
        }


        public ObservableRangeCollection<Acquaintance> Acquaintances { get; } = new ObservableRangeCollection<Acquaintance>();

        AsyncCommand loadCommand;
        public AsyncCommand LoadCommand => loadCommand ??=
            new AsyncCommand(ExecuteLoadCommand);

        public async Task ExecuteLoadCommand()
        { 
            if (Acquaintances.Count < 1 || LastUpdate < Settings.LastUpdate)
                await FetchAcquaintances();
        }

        AsyncCommand refreshCommand;
        public AsyncCommand RefreshCommand => refreshCommand ??=
            new AsyncCommand(ExecuteRefreshCommand);

        async Task ExecuteRefreshCommand()
        {
            await FetchAcquaintances();
        }

        async Task FetchAcquaintances()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            await Task.Delay(1000);
            var items = await DataSource.GetItems();

            Acquaintances.ReplaceRange(items);

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

