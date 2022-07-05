using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
        }

        private void SendButton_Clicked(object sender, EventArgs e)
        {
            string messageText = ((Editor)FindByName("MessageEditor")).Text;
            if (messageText != null)
            {
                CreateMessage(true, messageText);
                ((Editor)FindByName("MessageEditor")).Text = null;
            }
        }

        private void CreateMessage(bool fromMe, string messageText)
        {
            StackLayout bubbleContainer = (StackLayout)FindByName("BubbleContainer");

            StackLayout newMessageContainer = new StackLayout();
            Label messageDate = new Label();
            messageDate.Text = DateTime.Now.ToString();
            messageDate.FontSize = 10;
            messageDate.HorizontalTextAlignment = (fromMe ? TextAlignment.End : TextAlignment.Start);
            newMessageContainer.Children.Add(messageDate);

            Button newMessage = new Button();
            newMessage.FontFamily = "Eastman-Light";
            newMessage.CornerRadius = 10;
            newMessage.Text = messageText;
            int margin = (messageText.Length > 20 ? 100 : (300 - messageText.Length * 10));
            newMessage.Margin = new Thickness(fromMe ? margin : 0, 0, fromMe ? 0: margin, 0);

            newMessageContainer.Children.Add(newMessage);
            bubbleContainer.Children.Add(newMessageContainer);
        }
    }
}