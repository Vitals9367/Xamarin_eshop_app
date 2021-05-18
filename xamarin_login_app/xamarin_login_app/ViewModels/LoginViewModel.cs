using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using xamarin_login_app.Services;
using xamarin_login_app.Views;

namespace xamarin_login_app.ViewModels
{
	class LoginViewModel : BaseViewModel
	{

		private bool _RememberMe;
		public bool RememberMe
		{
			set
			{
				this._RememberMe = value;
				OnPropertyChanged();
			}
			get
			{
				return this._RememberMe;
			}
		}

		private string _Username;
		public string Username
		{
			set
			{
				this._Username = value;
				OnPropertyChanged();
			}
			get
			{
				return this._Username;
			}
		}

		private string _Password;
		public string Password
		{
			set
			{
				this._Password = value;
				OnPropertyChanged();
			}
			get
			{
				return this._Password;
			}
		}

		private string _Email;
		public string Email
		{
			set
			{
				this._Email = value;
				OnPropertyChanged();
			}
			get
			{
				return this._Email;
			}
		}

		private string _ConfirmPassword;
		public string ConfirmPassword
		{
			set
			{
				this._ConfirmPassword = value;
				OnPropertyChanged();
			}
			get
			{
				return this._ConfirmPassword;
			}
		}

		private bool _IsBusy;
		public bool IsBusy
		{
			set
			{
				this._IsBusy = value;
				OnPropertyChanged();
			}
			get
			{
				return this._IsBusy;
			}
		}

		private bool _Result;
		public bool Result
		{
			set
			{
				this._Result = value;
				OnPropertyChanged();
			}
			get
			{
				return this._Result;
			}
		}

		public Command LoginCommand { get; set; }
		public Command RegisterCommand { get; set; }
		public Command RegisterView { get; set; }
		public Command LoginView { get; set; }

		public LoginViewModel()
		{
			LoginCommand = new Command(async () => await LoginCommandAsync());
			RegisterCommand = new Command(async () => await RegisterCommandAsync());
			RegisterView = new Command(async() => await RegisterViewAsync());
			LoginView = new Command(async() => await LoginViewAsync());
		}

		private async Task RegisterViewAsync()
		{
			await Application.Current.MainPage.Navigation.PushAsync(new RegisterView());
		}
		private async Task LoginViewAsync()
		{
			await Application.Current.MainPage.Navigation.PopAsync();
		}
		private async Task RegisterCommandAsync()
		{
			if (IsBusy)
				return;
			try
			{
				IsBusy = true;

				if (ValidateString(Username,"Username")&&ValidateString(Password,"Password")&& ValidateEmail(Email)) {

					if (ConfirmPassword.Equals(Password))
					{
						await Application.Current.MainPage.Navigation.PushAsync(new LoadingView());
						var userService = new UserService();
						Result = await userService.RegisterUser(Username, Password, Email);
						if (Result)
						{
							SecureStorage.Remove("token");
							SecureStorage.Remove("username");
							SecureStorage.Remove("password");
							SecureStorage.Remove("RememberMe");

							await Task.WhenAll(
								 Application.Current.MainPage.Navigation.PopAsync(),
								 Application.Current.MainPage.Navigation.PopAsync()
								);
							await Application.Current.MainPage.DisplayAlert("Success", "User Registered!", "OK");
						}
						else
						{
							await Application.Current.MainPage.Navigation.PopAsync();
							await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong!", "OK");
						}
					}
					else
					{
						await Application.Current.MainPage.Navigation.PopAsync();
						await Application.Current.MainPage.DisplayAlert("Error", "Passwords does not match!", "OK");
					}
				}
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
			finally
			{
				IsBusy = false;
			}

		}

		private async Task LoginCommandAsync()
		{
			if (IsBusy)
				return;
			try
			{
				IsBusy = true;

				await SecureStorage.SetAsync("RememberMe", RememberMe.ToString());
				await Application.Current.MainPage.Navigation.PushAsync(new LoadingView());
				var userService = new UserService();
				var login = userService.LoginUser(Username, Password);
				var Result = await login;
				if (Result == true)
				{
					Device.BeginInvokeOnMainThread(() => Application.Current.MainPage = new HomeTabsView());
					await Application.Current.MainPage.DisplayAlert("Success", "User logged in!", "OK");
				}
				else
				{
					await Application.Current.MainPage.Navigation.PopAsync();
					await Application.Current.MainPage.DisplayAlert("Error", "Invalid info!", "OK");
				}
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.Navigation.PopAsync();
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
			finally
			{

				IsBusy = false;
			}
		}
		public async Task AutoLogin()
		{

			var check = await SecureStorage.GetAsync("RememberMe");

			if (check == "True")
			{
				var userService = new UserService();
				var attempt = await userService.AutoLogin();
				if (attempt == true)
				{
					Device.BeginInvokeOnMainThread(() => Application.Current.MainPage = new HomeTabsView());
					await Application.Current.MainPage.DisplayAlert("Success", "User logged in!", "OK");
				}
				else
				{
					//await Application.Current.MainPage.Navigation.PopAsync();
					await Application.Current.MainPage.DisplayAlert("Reminder", "Please log in!", "OK");
				}
			}
			else
			{
				//await Application.Current.MainPage.Navigation.PopAsync();
				await Application.Current.MainPage.DisplayAlert("Reminder", "Please log in!", "OK");
			}
		}
		public bool ValidateString(string string1,string name)
		{
			List<string> invalidChars = new List<string>() { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-" };
			
			if (string1.Length > 50)
			{
				Application.Current.MainPage.DisplayAlert("Error", name + " too long", "OK");
				return false;
			}else if (string1.Length < 4)
			{
				Application.Current.MainPage.DisplayAlert("Error", name + " too short", "OK");
				return false;
			}
			else
			{
				
				foreach (string s in invalidChars)
				{
					if (string1.Contains(s))
					{
						Application.Current.MainPage.DisplayAlert("Error", name + " contains not available characters", "OK");
						return false;
					}
				}
				return true;
			}
		}
		public bool ValidateEmail(string string1)
		{
			List<string> invalidChars = new List<string>() { "!", "#", "$", "%", "^", "&", "*", "(", ")" };

			if (string1.Length < 5)
			{
				Application.Current.MainPage.DisplayAlert("Error", "Email too short", "OK");
				return false;
			}else if(!string1.Contains(".")&&!string1.Contains("@")){
				Application.Current.MainPage.DisplayAlert("Error", "Email invalid", "OK");
				return false;
			}
			else
			{
				foreach (string s in invalidChars)
				{
					if (string1.Contains(s))
					{
						Application.Current.MainPage.DisplayAlert("Error", "Email contains not available characters", "OK");
						return false;
					}
				}
				return true;
			}
		}
	}
}
