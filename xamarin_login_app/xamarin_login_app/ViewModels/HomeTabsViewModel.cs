using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using xamarin_login_app.Views;

namespace xamarin_login_app.ViewModels
{
	class HomeTabsViewModel : BaseViewModel
	{

		public ICommand Logout { get; set; }

		public HomeTabsViewModel()
		{
			Logout = new Command(async ()=>await Operation());
		}
		public async Task Operation()
		{
			var answer = await Reminder();
			if (answer == true)
				Remove();
			else
				return;
		}

		private async Task<bool> Reminder()
		{
			bool answer = await Application.Current.MainPage.DisplayAlert("Reminder", "Do you really want to exit?", "Yes", "No");
			return answer;
		}

		private void Remove()
		{

			SecureStorage.Remove("token");
			SecureStorage.Remove("username");
			SecureStorage.Remove("password");
			SecureStorage.Remove("RememberMe");

			Device.BeginInvokeOnMainThread(() => Application.Current.MainPage = new LoginView());
		}
	}
}
