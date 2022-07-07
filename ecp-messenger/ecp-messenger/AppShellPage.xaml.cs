using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ecp_messenger.Services;
using ecp_messenger.Views;

namespace ecp_messenger
{
    public partial class AppShellPage : Shell
    {

        public Command UpdateMessages { private set; get; }
        public Command LogOff { private set; get; }

        public AppShellPage()
        {
            InitializeComponent();

            UpdateMessages = new Command(() =>
            {
                MySQLService.getInstance().getAllUsers();
                DisplayAlert("Пока обновления нет...", "Но оно скоро будет!", "Хорошо");
            });
            LogOff = new Command(() =>
            {
                App.Current.MainPage = new NavigationPage(new LoginPage());
            });
            BindingContext = this;
        }
    }
}