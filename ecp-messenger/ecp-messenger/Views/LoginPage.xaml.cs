using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using ecp_messenger.Services;
using ecp_messenger.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ecp_messenger.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : TabbedPage
    {
        public LoginPage()
        {
            InitializeComponent();
            if (Preferences.ContainsKey("username") && (Preferences.ContainsKey("password")))
            {
                ((Entry)FindByName("loginHolder")).Text = Preferences.Get("username", "");
                ((Entry)FindByName("passwordHolder")).Text = Preferences.Get("password", "");
                ((Entry)FindByName("hostHolder")).Text = Preferences.Get("host", "");
                Button_Clicked((Button)FindByName("buttonLogin"), null);
                ((CheckBox)FindByName("saveCheckBox")).IsChecked = true;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string login;
            string password;
            string host;
            await Navigation.PushModalAsync(new LoadingPage(), false);
            if ((Button)sender == (Button)FindByName("buttonLogin")) {
                login = ((Entry)FindByName("loginHolder")).Text;
                password = ((Entry)FindByName("passwordHolder")).Text;
                host = ((Entry)FindByName("hostHolder")).Text;
            }
            else
            {
                login = ((Entry)FindByName("loginHolderReg")).Text;
                password = ((Entry)FindByName("passwordHolderReg")).Text;
                host = ((Entry)FindByName("hostHolderReg")).Text;
            }
            if (login == null || login == "")
            {
                await DisplayAlert("Неправильные данные", "Не введен логин", "Ой");
                await Navigation.PopModalAsync(false);
                return;
            }
            else if (password == null || password == "")
            {
                await DisplayAlert("Неправильные данные", "Не введен пароль", "Ой");
                await Navigation.PopModalAsync(false);
                return;
            }
            if (host == null || host == "")
            {
                await DisplayAlert("Неправильные данные", "Не введен IP сервера", "Ой");
                await Navigation.PopModalAsync(false);
                return;
            }
            MySQLService.getInstance(host).updateHost(host);
            if (((CheckBox)FindByName("saveCheckBox")).IsChecked)
            {
                Preferences.Set("username", login);
                Preferences.Set("password", password);
                Preferences.Set("host", host);
            }

            if ((Button)sender == (Button)FindByName("buttonLogin"))
            {
                // check user passwords
                try
                {
                    if (MySQLService.getInstance(host).checkUserAccess(login, password))
                    {

                        // UserSession.getInstance().messages = MySQLService.getInstance().getAllMessages(login);
                        UserSession.getInstance().id = MySQLService.getInstance().findIdByUsername(login);
                        UserSession.getInstance().username = login;
                        UserSession.getInstance().initDialogs();
                        //UserSession.getInstance().debugShowDialogs();

                        await Task.Factory.StartNew(() =>
                        {
                            bool needToClose = false;
                            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                            {
                                App.Current.MainPage = new AppShellPage();
                                Navigation.PopModalAsync(false);
                                return false;
                            });
                        });
                        return;
                    }
                    await DisplayAlert("Ошибка входа", "Неверный логин и/или пароль", "Ок");
                    await Navigation.PopModalAsync(false);
                }
                catch
                {
                    await DisplayAlert("Кажется, сервер не отвечает", "Проверьте IP-адрес или попробуйте позже", "Ок");
                    await Navigation.PopModalAsync(false);
                }
            }
            else
            {
                // trying to register user
                try
                {
                    if (MySQLService.getInstance(host).findIdByUsername(login) != -1)
                    {
                        await DisplayAlert("Ошибка регистрации", "Пользователь с таким логином уже существует", "Ок");
                        await Navigation.PopModalAsync(false);
                        return;
                    }
                    MySQLService.getInstance(host).insertUser(new User
                    {
                        Username = login,
                        Password = BCrypt.Net.BCrypt.HashPassword(password)
                    });
                    await DisplayAlert("Успешная регистрация", "Попробуйте войти через страницу авторизации", "Ок");
                    await Navigation.PopModalAsync(false);
                    return;
                }
                catch
                {
                    await DisplayAlert("Кажется, сервер не отвечает", "Проверьте IP-адрес или попробуйте позже", "Ок");
                    await Navigation.PopModalAsync(false);
                }
            }

        }

        async public void ChangeBarColor(Object sender, EventArgs eventArgs)
        {
            Color color = new Color();
            if (sender == FindByName("LogPage"))
                color = new Color(0.419, 0.329, 0.549);
            else
                color = new Color(0.529, 0.439, 0.502);

            Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
            {
                Task.Run(async () =>
                {
                    BarBackgroundColor = color;
                });
                return false;
            });
        }
    }
}
