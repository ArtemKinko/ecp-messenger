using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecp_messenger.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ecp_messenger.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuHeader : ContentView
{
    public MenuHeader()
    {
        InitializeComponent();
            ((Label)FindByName("UsernameLabel")).Text = UserSession.getInstance().username;
    }
}
}