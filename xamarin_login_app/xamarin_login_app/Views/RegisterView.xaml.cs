using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamarin_login_app.Services;

namespace xamarin_login_app.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterView : ContentPage
	{
		public RegisterView()
		{
			InitializeComponent();
		}

		private async void Login_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new LoginView());
		}

	}
}