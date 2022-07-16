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
    public partial class WritePage : ContentPage
    {
        public WritePage()
        {
            InitializeComponent();
        }

        async public void SendMessage(Object sender, EventArgs eventArgs)
        {
            string username = ((Entry)FindByName("user")).Text;
            string message = ((Editor)FindByName("message")).Text;
            int id = MySQLService.getInstance().findIdByUsername(username);

            if (username == UserSession.getInstance().username)
            {
                await DisplayAlert("Ошибка при отправке", "Нельзя отправить сообщение самому себе", "Ой");
                return;
            }

            if (username == null || username == "")
            {
                await DisplayAlert("Ошибка при отправке", "Вы не ввели логин получателя", "Ой");
                return;
            }
            if (message == null || message == "")
            {
                await DisplayAlert("Ошибка при отправке", "Вы не ввели сообщение", "Ой");
                return;
            }

            if (id != -1)
            {
                MySQLService.getInstance().insertMessage(new Message
                {
                    fromId = UserSession.getInstance().id,
                    toId = id,
                    text = message,
                    sendTime = DateTime.Now.Year + "-" + DateTime.Now.Month + "-"
                + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":"
                + DateTime.Now.Second
                });
                UserSession.getInstance().initDialogs();
                await DisplayAlert("Успешная отправка", "Воспользуйтесь кнопкой в меню, чтобы обновить сообщения", "Ок");
                ClosePage(this, null);
            }
            else
            {
                await DisplayAlert("Ошибка при отправке", "Пользователя с таким логином не существует", "Ой");
            }
        }

        async public void ClosePage(Object sender, EventArgs eventArgs)
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(0), () =>
            {
                Task.Run(async () =>
                {
                    for (int counter = 70; counter >= 0; counter--)
                    {
                        await Task.Delay(5);
                        ((BoxView)FindByName("background")).Opacity = counter * 0.01;
                    }
                });
                return false;
            });
            await Task.Delay(320);
            await Navigation.PopModalAsync();
        }

        async public void AppearOpacityChange(Object sender, EventArgs eventArgs)
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(300), () =>
            {
                Task.Run(async () =>
                {
                    for (int counter = 0; counter < 70; counter++)
                    {
                        await Task.Delay(5);
                        ((BoxView)FindByName("background")).Opacity = counter * 0.01;
                    }
                });
                return false;
            });
        }
    }
}