using Acquaint.Constants;
using Acquaint.Services;
using Acquaint.Styles;
using Acquaint.Util;
using Acquaint.ViewModels;
using Acquaint.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Acquaint
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SubscribeToDisplayAlertMessages();

            ThemeHelper.ChangeTheme(Settings.ThemeOption, true);

            var navPage = new NavigationPage( new ListPage());

            navPage.SetDynamicResource(VisualElement.BackgroundColorProperty, "PrimaryColor");

            // set the MainPage of the app to the navPage
            MainPage = navPage;

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

