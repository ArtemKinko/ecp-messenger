using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecp_messenger.Services;
using ecp_messenger.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ecp_messenger.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoadingPage(), false);
            string login = ((Entry)FindByName("loginHolder")).Text;
            string password = ((Entry)FindByName("passwordHolder")).Text;
            string host = ((Entry)FindByName("hostHolder")).Text;
            if (login == null || login == "")
            {
                await DisplayAlert("Неправильные данные", "Не введен логин", "Ввести");
                await Navigation.PopModalAsync(false);
                return;
            }
            else if (password == null || password == "")
            {
                await DisplayAlert("Неправильные данные", "Не введен пароль", "Ввести");
                await Navigation.PopModalAsync(false);
                return;
            }
            if (host == null || host == "")
            {
                host = "51.250.27.157";
            }

            // check user passwords

             if (MySQLService.getInstance(host).checkUserAccess(login, password))
            {
                UserSession.getInstance().messages = MySQLService.getInstance().getAllMessages(login);
                UserSession.getInstance().id = MySQLService.getInstance().findIdByUsername(login);
                UserSession.getInstance().username = login;
                await Navigation.PopModalAsync(false);
                App.Current.MainPage = new AppShellPage();
                return;
            }
            await DisplayAlert("Ошибка входа", "Неверный логин и/или пароль", "Ок");
            await Navigation.PopModalAsync(false);

        }
    }
}
