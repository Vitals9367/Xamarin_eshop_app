using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamarin_login_app.Views;
using ListView = xamarin_login_app.Views.ListView;

namespace xamarin_login_app
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage( new LoginView());
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
