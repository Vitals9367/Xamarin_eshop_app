using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamarin_login_app.ViewModels;

namespace xamarin_login_app.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginView : ContentPage
	{
		LoginViewModel VM => (LoginViewModel)this.BindingContext;
		public LoginView()
		{
			InitializeComponent();
			VM.AutoLogin().GetAwaiter();
		}
	}
}