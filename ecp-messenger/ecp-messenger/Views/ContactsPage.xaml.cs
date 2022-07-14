using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecp_messenger.Models;

using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit.Effects;

namespace ecp_messenger.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactsPage : ContentPage
    {
        public ContactsPage()
        {
            InitializeComponent();
            createDialogs();
        }

        public void updateDialogs()
        {
            InitializeComponent();
            createDialogs();
        }

        public void createDialogs()
        {
            int count = 0;
            Frame frame = null;
            Grid grid = null;
            foreach (Dialog dialog in UserSession.getInstance().dialogs)
            {
                if (count == 0)
                {
                    AbsoluteLayout layout = (AbsoluteLayout)FindByName("FirstDialog");
                    layout.IsVisible = true;
                    ((Label)FindByName("FirstUsername")).Text = dialog.User.Username;
                    ((AbsoluteLayout)FindByName("FirstDialog")).ClassId = dialog.User.Username;
                }
                else
                {
                    if (count % 2 == 1)
                    {
                        frame = new Frame();
                        frame.BackgroundColor = Color.Transparent;
                        grid = new Grid();
                        grid.Padding = -10;
                        ColumnDefinitionCollection columns = new ColumnDefinitionCollection();
                        columns.Add(new ColumnDefinition());
                        columns.Add(new ColumnDefinition());
                        grid.ColumnDefinitions = columns;

                        AbsoluteLayout layout = new AbsoluteLayout();
                        layout.HeightRequest = 200;
                        layout.HorizontalOptions = LayoutOptions.Fill;
                        layout.VerticalOptions = LayoutOptions.Fill;
                        TouchEffect.SetAnimationDuration(layout, 100);
                        TouchEffect.SetAnimationEasing(layout, Easing.CubicIn);
                        TouchEffect.SetPressedScale(layout, 0.8);

                        TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
                        tapGestureRecognizer.Tapped += (s, e) =>
                        {
                            OpenDialog(s, e);
                        };
                        layout.GestureRecognizers.Add(tapGestureRecognizer);

                        BoxView shadowBox = new BoxView();
                        shadowBox.TranslationX = 2;
                        shadowBox.TranslationY = 2;
                        shadowBox.Scale = 1.02;
                        shadowBox.BackgroundColor = Color.Black;
                        shadowBox.Margin = 10;
                        shadowBox.CornerRadius = 10;
                        AbsoluteLayout.SetLayoutBounds(shadowBox, new Rectangle(0, 0, 1, 1));
                        AbsoluteLayout.SetLayoutFlags(shadowBox, AbsoluteLayoutFlags.All);
                        layout.ClassId = dialog.User.Username;
                        layout.Children.Add(shadowBox);

                        BoxView boxView = new BoxView();
                        boxView.BackgroundColor = Color.LightGray;
                        boxView.Margin = 10;
                        boxView.CornerRadius = 10;
                        AbsoluteLayout.SetLayoutBounds(boxView, new Rectangle(0, 0, 1, 1));
                        AbsoluteLayout.SetLayoutFlags(boxView, AbsoluteLayoutFlags.All);
                        layout.Children.Add(boxView);

                        Grid secGrid = new Grid();

                        Image image = new Image();
                        image.Source = "user_icon";
                        image.Margin = new Thickness(30, 10, 30, 10);
                        image.BackgroundColor = Color.Transparent;
                        secGrid.Children.Add(image);

                        Label label = new Label();
                        Grid.SetRow(label, 1);
                        label.Text = dialog.User.Username;
                        label.HorizontalOptions = LayoutOptions.Center;
                        label.FontSize = 20;
                        label.FontFamily = "Eastman-Light";
                        secGrid.Children.Add(label);

                        layout.Children.Add(secGrid);
                        grid.Children.Add(layout);
                        if (count == UserSession.getInstance().dialogs.Count - 1)
                            frame.Content = grid;

                        ((StackLayout)FindByName("FrameContainer")).Children.Add(frame);
                    }
                    else
                    {
                        AbsoluteLayout layout = new AbsoluteLayout();
                        layout.HeightRequest = 200;
                        layout.HorizontalOptions = LayoutOptions.Fill;
                        layout.VerticalOptions = LayoutOptions.Fill;
                        TouchEffect.SetAnimationDuration(layout, 100);
                        TouchEffect.SetAnimationEasing(layout, Easing.CubicIn);
                        TouchEffect.SetPressedScale(layout, 0.8);
                        Grid.SetColumn(layout, 1);

                        TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
                        tapGestureRecognizer.Tapped += (s, e) =>
                        {
                            OpenDialog(s, e);
                        };
                        layout.GestureRecognizers.Add(tapGestureRecognizer);

                        BoxView shadowBox = new BoxView();
                        shadowBox.TranslationX = 2;
                        shadowBox.TranslationY = 2;
                        shadowBox.Scale = 1.02;
                        shadowBox.BackgroundColor = Color.Black;
                        shadowBox.Margin = 10;
                        shadowBox.CornerRadius = 10;
                        AbsoluteLayout.SetLayoutBounds(shadowBox, new Rectangle(0, 0, 1, 1));
                        AbsoluteLayout.SetLayoutFlags(shadowBox, AbsoluteLayoutFlags.All);
                        layout.ClassId = dialog.User.Username;
                        layout.Children.Add(shadowBox);

                        BoxView boxView = new BoxView();
                        boxView.BackgroundColor = Color.LightGray;
                        boxView.Margin = 10;
                        boxView.CornerRadius = 10;
                        AbsoluteLayout.SetLayoutBounds(boxView, new Rectangle(0, 0, 1, 1));
                        AbsoluteLayout.SetLayoutFlags(boxView, AbsoluteLayoutFlags.All);
                        layout.Children.Add(boxView);

                        Grid secGrid = new Grid();

                        Image image = new Image();
                        image.Source = "user_icon";
                        image.Margin = new Thickness(30, 10, 30, 10);
                        image.BackgroundColor = Color.Transparent;
                        secGrid.Children.Add(image);

                        Label label = new Label();
                        Grid.SetRow(label, 1);
                        label.Text = dialog.User.Username;
                        label.HorizontalOptions = LayoutOptions.Center;
                        label.FontSize = 20;
                        label.FontFamily = "Eastman-Light";
                        secGrid.Children.Add(label);

                        layout.Children.Add(secGrid);
                        grid.Children.Add(layout);
                        frame.Content = grid;
                    }
                }
                count++;
            }
            Title = "Выбор диалога (всего: " + count + ")";
        }

        public void WriteMessage(Object sender, EventArgs eventArgs)
        {
            Navigation.PushModalAsync(new WritePage());
        }

        public void OpenDialog(Object sender, EventArgs eventArgs)
        {
            Navigation.PushAsync(new DialogPage(((AbsoluteLayout)sender).ClassId));
        }
    }
}