using Newtonsoft.Json;
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
	class OrdersService
	{
		RestClient client;

		public OrdersService()
		{
			client = new RestClient(Constants.RestIP);
		}

		public async Task<List<Orders>> GetAllProductsAsync()
		{
			List<Orders> ordersList = new List<Orders>();
			var url = "/api/user/orders";
			var request = new RestRequest(url, Method.GET, DataFormat.Json);
			var token = await SecureStorage.GetAsync("token");
			request.AddHeader("x-access-token", token);
			try
			{
				var result = await client.ExecuteAsync(request);
				var myData = JsonConvert.DeserializeObject<List<Orders>>(result.Content);

				if (result.StatusCode == System.Net.HttpStatusCode.OK)
				{
					ordersList = myData;
				}
				return ordersList;
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Order Service Error", e.Message , "OK");
			}
			return ordersList;
		}
		public async Task<Orders> GetProductAsync(int id)
		{
			var url = "/api/user/order/"+id;
			var request = new RestRequest(url, Method.GET, DataFormat.Json);
			var token = await SecureStorage.GetAsync("token");
			request.AddHeader("x-access-token", token);
			try
			{
				var result = await client.ExecuteAsync(request);
				var myData = JsonConvert.DeserializeObject<Orders>(result.Content);

				if (result.StatusCode == System.Net.HttpStatusCode.OK)
				{
					return myData;
				}
				else
				{
					return null;
				}
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Order Service Error", e.Message, "OK");
				return null;
			}
		}
		public async Task<string> DeleteItemAsync(int id)
		{
			var url = "api/user/delete_order";
			var request = new RestRequest(url, Method.DELETE, DataFormat.Json);
			var token = await SecureStorage.GetAsync("token");
			request.AddHeader("x-access-token", token);
			request.AddJsonBody(new { item_id = id });

			try
			{
				var result = await client.ExecuteAsync(request);
				return result.Content;
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("error", e.Message, "ok");
			}

			return "internal app error";
		}
		public async Task<Orders> CreateOrder()
		{

			Orders order = new Orders();

			var url = "/api/user/create_order";
			var request = new RestRequest(url, Method.POST, DataFormat.Json);
			var token = await SecureStorage.GetAsync("token");
			request.AddHeader("x-access-token", token);
			try
			{
				var result = await client.ExecuteAsync<Orders>(request);

				if (result.StatusCode == System.Net.HttpStatusCode.OK)
				{
					order = result.Data;
				}
				return order;
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Order Service Error", e.Message, "OK");
			}
			return order;
		}
		public async Task<bool> CompletePayment(int id)
		{

			var url = "/api/user/complete_order/"+id;
			var request = new RestRequest(url, Method.PUT, DataFormat.Json);
			var token = await SecureStorage.GetAsync("token");
			request.AddHeader("x-access-token", token);
			try
			{
				var result = await client.ExecuteAsync(request);

				if (result.StatusCode == System.Net.HttpStatusCode.OK)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Order Service Error", e.Message, "OK");
				return false;
			}
		}
	}
}
