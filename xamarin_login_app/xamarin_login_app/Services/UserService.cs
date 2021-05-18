using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using Xamarin.Essentials;
using Xamarin.Forms;
using xamarin_login_app.Models;

namespace xamarin_login_app.Services
{
	class UserService
	{
		RestClient client;
		public UserService()
		{
			client = new RestClient(Constants.RestIP);
		}

		public async Task<bool> LoginUser(string uname, string passwd)
		{
			try
			{
				if (await CheckUser(uname) == true)
				{
					client.Authenticator = new HttpBasicAuthenticator(uname, passwd);
					var request = new RestRequest("/login", Method.GET, DataFormat.Json);
					var result = client.Execute(request);
					if (result.StatusCode == System.Net.HttpStatusCode.OK)
					{
						List<Task> tasks = new List<Task>();
						tasks.Add(SecureStorage.SetAsync("token", JsonConvert.DeserializeObject<Token>(result.Content).token));
						tasks.Add(SecureStorage.SetAsync("username", uname));
						tasks.Add(SecureStorage.SetAsync("password", passwd));

						await Task.WhenAll(tasks);

						return true;
					}
				}
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
			}
			return false;

		}
		public async Task<bool> RegisterUser(string uname, string passwd,string email)
		{
			var url = "/api/users";
			var request = new RestRequest(url,Method.POST, DataFormat.Json);

			try
			{
				if (await CheckUser(uname) == false) {
					request.AddJsonBody(new User(uname, passwd,email));
					var result = await client.ExecuteAsync<User>(request);
					if (result.StatusCode == System.Net.HttpStatusCode.Created)
					{
						return true;
					}
					else
					{
						await Application.Current.MainPage.DisplayAlert("Error", result.StatusCode.ToString(), "OK");
					}
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("Error", "User already exists!", "OK");
					return false;
				}
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
			}
			return false;
		}
		public async Task<bool> CheckUser(string uname)
		{
			var url = "api/users/check/" + uname;
			var request = new RestRequest(url, Method.GET, DataFormat.Json);
			var result = await client.ExecuteAsync<User>(request);
			if (result.StatusCode == System.Net.HttpStatusCode.OK)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public async Task<bool> AutoLogin()
		{
			var username = await SecureStorage.GetAsync("username");
			var password = await SecureStorage.GetAsync("password");


			var login = await LoginUser(username,password);
			if (login == true)
				return true;
			else
				return false;
		}
		public async Task<User> getUserDetails()
		{
			var url = "api/user";
			var request = new RestRequest(url, Method.GET, DataFormat.Json);
			var token = await SecureStorage.GetAsync("token");
			request.AddHeader("x-access-token", token);
			var result = await client.ExecuteAsync<User>(request);
			if (result.StatusCode == System.Net.HttpStatusCode.OK)
			{
				return result.Data;
			}
			else
			{
				return null;
			}
		}
	}
}
