using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ecp_messenger.Views;

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
            Device.SetFlags(new[] { "Brush_Experimental" });
            MainPage = new LoginPage();
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
