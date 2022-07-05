using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
                DisplayAlert("Пока обновления нет...", "Но оно скоро будет!", "Хорошо");
            });
            LogOff = new Command(() =>
            {
                DisplayAlert("Пока выхода нет...", "Но он скоро будет!", "Хорошо");
            });
            BindingContext = this;
        }
    }
}