using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using xamarin_login_app.Models;

namespace xamarin_login_app.Services
{
	class UserInfoService
	{

		RestClient client;
		public UserInfoService()
		{
			client = new RestClient(Constants.RestIP);
		}
		public async Task<User_Info> GetInfoAsync()
		{
			User_Info info = new User_Info();
			var url = "api/user_info";
			var request = new RestRequest(url, Method.GET, DataFormat.Json);
			var token = await SecureStorage.GetAsync("token");
			request.AddHeader("x-access-token", token);

			try
			{
				var result = await client.ExecuteAsync<User_Info>(request);

				if (result.StatusCode == System.Net.HttpStatusCode.OK)
				{
					info = result.Data;
				}
				return info;
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
			}
			return info;
		}
		public async Task UpdateInfoAsync(User_Info info)
		{
			var url = "api/user_info/update";
			var request = new RestRequest(url, Method.PUT, DataFormat.Json);
			var token = await SecureStorage.GetAsync("token");
			request.AddHeader("x-access-token", token);
			request.AddJsonBody(new
			{
				first_name = info.first_name,
				last_name = info.last_name,
				phone_number = info.phone_number,
				address = info.address,
			});
			try
			{
				var result = await client.ExecuteAsync(request);

				if (result.StatusCode == System.Net.HttpStatusCode.OK)
				{
					await Application.Current.MainPage.DisplayAlert("Sucess", "Info Updated", "OK");
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("Error", "Could not update info", "OK");
				}
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
			}
		}
	}
}
