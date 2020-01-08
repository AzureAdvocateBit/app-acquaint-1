using MyContacts.Constants;
using MyContacts.Services;
using MyContacts.Styles;
using MyContacts.Util;
using MyContacts.ViewModels;
using MyContacts.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MyContacts
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SubscribeToDisplayAlertMessages();

            var navPage = new NavigationPage(new ListPage())
            {
                BarTextColor = Color.White
            };

            navPage.SetDynamicResource(NavigationPage.BarBackgroundColorProperty, "PrimaryColor");

            // set the MainPage of the app to the navPage
            MainPage = navPage;

        }

        protected override void OnStart()
        {
            base.OnStart();
            ThemeHelper.ChangeTheme(Settings.ThemeOption, true);
        }

        protected override void OnResume()
        {
            base.OnResume();
            ThemeHelper.ChangeTheme(Settings.ThemeOption, true);
        }

        /// <summary>
        /// Subscribes to messages for displaying alerts.
        /// </summary>
        static void SubscribeToDisplayAlertMessages()
        {
            MessagingService.Current.Subscribe<MessagingServiceAlert>(MessageKeys.DisplayAlert, async (service, info) =>
            {
                var task = Current?.MainPage?.DisplayAlert(info.Title, info.Message, info.Cancel);
                if (task != null)
                {
                    await task;
                    info?.OnCompleted?.Invoke();
                }
            });

            MessagingService.Current.Subscribe<MessagingServiceQuestion>(MessageKeys.DisplayQuestion, async (service, info) =>
            {
                var task = Current?.MainPage?.DisplayAlert(info.Title, info.Question, info.Positive, info.Negative);
                if (task != null)
                {
                    var result = await task;
                    info?.OnCompleted?.Invoke(result);
                }
            });
        }
    }
}

