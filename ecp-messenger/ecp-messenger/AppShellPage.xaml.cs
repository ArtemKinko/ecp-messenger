using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using ecp_messenger.Services;
using ecp_messenger.Views;

namespace ecp_messenger
{
    public partial class AppShellPage : Shell
    {

        public AppShellPage()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            DisplayAlert("OK", "OK", "OK");
        }

        private void GoToDialogs(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//dialogs");
            Shell.Current.FlyoutIsPresented = false;
        }

        async private void UpdateMessages(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new LoadingPage(), false);
            App.Current.MainPage = new AppShellPage();
            DisplayAlert("Успешно обновлено!", "База данных сообщений обновлена", "Ок");
        }

        private void GoToSettings(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//settings");
            Shell.Current.FlyoutIsPresented = false;
        }

        private void LogOff(object sender, EventArgs e)
        {
            Preferences.Clear();
            App.Current.MainPage = new LoginPage();
        }
    }
}