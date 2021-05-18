using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using xamarin_login_app.Models;

namespace xamarin_login_app.Services
{
	class CartItemService
	{
		RestClient client;

		public CartItemService()
		{
			client = new RestClient(Constants.RestIP);
		}

		public async Task<List<Cart_Items>> GetAllProductsAsync()
		{
			List<Cart_Items> productList = new List<Cart_Items>();
			ImageService imageService = new ImageService();
			var url = "/api/user/cart_items";
			var request = new RestRequest(url, Method.GET, DataFormat.Json);
			var token = await SecureStorage.GetAsync("token");
			request.AddHeader("x-access-token", token);
			try
			{
				var result = await client.ExecuteAsync(request);
				var myData = JsonConvert.DeserializeObject<List<Cart_Items>>(result.Content);

				if (result.StatusCode == System.Net.HttpStatusCode.OK)
				{
					productList = myData;
					foreach(Cart_Items item in productList)
						imageService.getImage(item.defined_item.item);
				}
				return productList;
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Service Error", e.Message, "OK");
			}
			return productList;
		}

		public async Task<string> DeleteItemAsync(int id)
		{
			var url = "/api/user/delete_cart_item";
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
				await Application.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
			}

			return "Internal App Error";
		}
	}
}
