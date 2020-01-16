using MyContacts.Constants;
using MyContacts.Services;
using MyContacts.Styles;
using MyContacts.Util;
using MyContacts.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MyContacts
{
    public partial class App : Application
    {
        public static bool UseLocalDataSource = false;
        public App()
        {
            InitializeComponent();

            if (UseLocalDataSource)
                DependencyService.Register<FileDataSource>();
            else
                DependencyService.Register<AzureDataStore>();

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
    }
}

