using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit.Effects;
using ecp_messenger.Services;

using ecp_messenger.Models;

namespace ecp_messenger.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogPage : ContentPage
    {
        public int lines;
        public string text;

        public DialogPage()
        {
            InitializeComponent();
            initializeMessages();

            Task.Factory.StartNew(() =>
            {

                Device.StartTimer(new TimeSpan(0, 0, 5), () =>
                {
                    UserSession.getInstance().messages = MySQLService.getInstance().getAllMessages(UserSession.getInstance().username);
                    initializeMessages();
                    return true;
                });
            });
        }

        private void initializeMessages()
        {
            StackLayout bubbleContainer = (StackLayout)FindByName("BubbleContainer");
            if (bubbleContainer.Children.Count > 0)
                bubbleContainer.Children.Clear();
            foreach (Message message in UserSession.getInstance().messages)
            {
                if (message.fromId == UserSession.getInstance().id)
                    CreateMessage(true, message.text, message.sendTime);
                else
                    CreateMessage(false, message.text, message.sendTime);
            }
        }

        private void SendButton_Clicked(object sender, EventArgs e)
        {
            string messageText = ((Editor)FindByName("MessageEditor")).Text;
            if (messageText != null)
            {
                CreateMessage(true, messageText);
                ((Editor)FindByName("MessageEditor")).Text = null;
            }

            MySQLService.getInstance().insertMessage(new Message
            {
                fromId = UserSession.getInstance().id,
                toId = UserSession.getInstance().id == 1 ? 2 : 1,
                text = messageText,
                sendTime = DateTime.Now.Year + "-" + DateTime.Now.Month + "-"
                + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":"
                + DateTime.Now.Second
            }) ;
        }

        private void CreateMessage(bool fromMe, string messageText, string sendTime = "")
        {
            StackLayout bubbleContainer = (StackLayout)FindByName("BubbleContainer");

            StackLayout newMessageContainer = new StackLayout();
            Label messageDate = new Label();

            messageDate.Text = sendTime == "" ? DateTime.Now.ToString() : sendTime;
            messageDate.FontSize = 10;
            messageDate.HorizontalTextAlignment = (fromMe ? TextAlignment.End : TextAlignment.Start);
            newMessageContainer.Children.Add(messageDate);

            Button newMessage = new Button();
            newMessage.Text = messageText;
            newMessage.FontFamily = "Eastman-Light";
            newMessage.CornerRadius = 10;
            int margin = (messageText.Length > 20 ? 100 : (300 - messageText.Length * 10));
            newMessage.Margin = new Thickness(fromMe ? margin : 0, 0, fromMe ? 0: margin, 0);

            newMessageContainer.Children.Add(newMessage);
            bubbleContainer.Children.Add(newMessageContainer);
        }
    }
}