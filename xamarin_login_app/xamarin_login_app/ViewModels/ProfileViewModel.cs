using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using xamarin_login_app.Models;
using xamarin_login_app.Services;

namespace xamarin_login_app.ViewModels
{
	class ProfileViewModel : BaseViewModel
	{
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

		private string _FirstName;
		public string FirstName
		{
			set
			{
				this._FirstName = value;
				OnPropertyChanged();
			}
			get
			{
				return this._FirstName;
			}
		}

		private string _LastName;
		public string LastName
		{
			set
			{
				this._LastName = value;
				OnPropertyChanged();
			}
			get
			{
				return this._LastName;
			}
		}

		private string _PhoneNumber;
		public string PhoneNumber 
		{
			set
			{
				this._PhoneNumber = value;
				OnPropertyChanged();
			}
			get
			{
				return this._PhoneNumber;
			}
		}

		private string _Address;
		public string Address
		{
			set
			{
				this._Address = value;
				OnPropertyChanged();
			}
			get
			{
				return this._Address;
			}
		}

		public Command UpdateCommand { get; set; }

		public ProfileViewModel()
		{
			GetInfo();
			UpdateCommand = new Command(async () => await UpdateInfo());
		}
		public async void GetInfo()
		{
			if (IsBusy)
				return;
			try
			{
				IsBusy = true;
				User_Info info = await new UserInfoService().GetInfoAsync();
				UpdateFields(info);

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

		public async Task UpdateInfo()
		{
			if (IsBusy)
				return;
			try
			{
				IsBusy = true;

				User_Info newInfo = new User_Info();
				newInfo.first_name = FirstName;
				newInfo.last_name = LastName;
				newInfo.phone_number = PhoneNumber;
				newInfo.address = Address;


				await new UserInfoService().UpdateInfoAsync(newInfo);
				GetInfo();
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

		private void UpdateFields(User_Info info)
		{
			FirstName = info.first_name;
			LastName = info.last_name;
			PhoneNumber = info.phone_number;
			Address = info.address;
		}
	}
}
