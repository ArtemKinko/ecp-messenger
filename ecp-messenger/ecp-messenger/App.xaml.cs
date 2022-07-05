using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("Choplin-Medium.ttf", Alias = "Choplin")]
[assembly: ExportFont("Eastman-Light.ttf", Alias = "Eastman-Light")]
[assembly: ExportFont("Eastman-Medium.ttf", Alias = "Eastman-Medium")]


namespace ecp_messenger
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShellPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
