using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        bool needToClose = false;
        public string toUsername;
        private List<Message> messages;
        CancellationTokenSource ts = new CancellationTokenSource();

        public DialogPage(string username)
        {
            InitializeComponent();
            toUsername = username;
            initializeMessages(username);
            Title = "Диалог с  «" + username + "»";
            CancellationToken ct = ts.Token;
            messages = new List<Message>();

            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                if (needToClose)
                    return false;
                
                Task.Run(async () =>
                {
                    messages = UserSession.getInstance().updateMessages(username);
                });
                return true;
            });

            Task.Factory.StartNew(() =>
            {
                Device.StartTimer(new TimeSpan(0, 0, 0, 0, 100), () =>
                {
                    if (needToClose)
                        return false;
                    if (messages.Count == 0)
                        return true;
                    else
                    {
                        foreach (Message message in messages)
                            CreateMessage(message.fromId == UserSession.getInstance().id ? true : false,
                                message.text, message.sendTime);
                        System.Diagnostics.Debug.WriteLine(messages.Count);
                        messages.Clear();
                        var lastChild = ((StackLayout)FindByName("BubbleContainer")).Children.LastOrDefault();
                        ((ScrollView)FindByName("ScrollView")).ScrollToAsync(lastChild, ScrollToPosition.MakeVisible, true);
                    }
                    return true;
                });
            });
        }

        async private void initializeMessages(string username)
        {
            StackLayout bubbleContainer = (StackLayout)FindByName("BubbleContainer");
            if (bubbleContainer.Children.Count > 0)
                bubbleContainer.Children.Clear();
            List<Message> messages = UserSession.getInstance().getMessagesByUsername(username);
            foreach (Message message in messages)
            {
                if (message.fromId == UserSession.getInstance().id)
                    CreateMessage(true, message.text, message.sendTime);
                else
                    CreateMessage(false, message.text, message.sendTime);
            }

            await Task.Delay(100);
            var lastChild = ((StackLayout)FindByName("BubbleContainer")).Children.LastOrDefault();
            await ((ScrollView)FindByName("ScrollView")).ScrollToAsync(lastChild, ScrollToPosition.MakeVisible, false);
        }

        async private void SendButton_Clicked(object sender, EventArgs e)
        {
            string messageText = ((Editor)FindByName("MessageEditor")).Text;
            if (messageText != null)
            {
                CreateMessage(true, messageText);
                ((Editor)FindByName("MessageEditor")).Text = null;
            }
            else
                return;

            Device.StartTimer(new TimeSpan(0, 0, 0), () =>
            {
                Task.Run(async () =>
                {
                    MySQLService.getInstance().insertMessage(new Message
                    {
                        fromId = UserSession.getInstance().id,
                        toId = MySQLService.getInstance().findIdByUsername(toUsername),
                        text = messageText,
                        sendTime = DateTime.Now.Year + "-" + DateTime.Now.Month + "-"
                        + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":"
                        + DateTime.Now.Second
                    });
                    UserSession.getInstance().updateMessages(toUsername);
                    await Task.Delay(100);
                });
                return false;
            });

            var lastChild = ((StackLayout)FindByName("BubbleContainer")).Children.LastOrDefault();
            await ((ScrollView)FindByName("ScrollView")).ScrollToAsync(lastChild, ScrollToPosition.MakeVisible, true);
        }

        private void CreateMessage(bool fromMe, string messageText, string sendTime = "")
        {
            StackLayout bubbleContainer = (StackLayout)FindByName("BubbleContainer");

            StackLayout newMessageContainer = new StackLayout();
            Label messageDate = new Label();

            messageDate.Text = sendTime == "" ? (DateTime.Now.Day < 10 ? "0" : "") + DateTime.Now.Day + "." +
                (DateTime.Now.Month < 10 ? "0" : "") + DateTime.Now.Month + " " +
                (DateTime.Now.Hour < 10 ? "0" : "") + DateTime.Now.Hour + ":" +
                (DateTime.Now.Minute < 10 ? "0" : "") + DateTime.Now.Minute : sendTime;
            messageDate.FontSize = 10;
            messageDate.HorizontalTextAlignment = (fromMe ? TextAlignment.End : TextAlignment.Start);
            messageDate.Margin = new Thickness(fromMe ? 0 : 10, 0, fromMe ? 10 : 0, 0);
            newMessageContainer.Children.Add(messageDate);

            Button newMessage = new Button();
            newMessage.Text = messageText;
            newMessage.FontFamily = "Eastman-Light";
            newMessage.CornerRadius = 10;
            int margin = (messageText.Length > 20 ? 100 : (300 - messageText.Length * 10));
            newMessage.Margin = new Thickness(fromMe ? margin : 10,
                                            0,
                                            fromMe ? 10: margin,
                                            0);

            newMessageContainer.Children.Add(newMessage);
            bubbleContainer.Children.Add(newMessageContainer);
        }

        public void closePage(Object sender, EventArgs eventArgs)
        {
            ts.Cancel();
            needToClose = true;
            System.Diagnostics.Debug.WriteLine("Closed");
        }
    }

}