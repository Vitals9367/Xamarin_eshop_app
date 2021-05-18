using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using xamarin_login_app.Models;

namespace xamarin_login_app.Services
{
	class ProductService
	{
		RestClient client;

		public ProductService()
		{
			client = new RestClient(Constants.RestIP);
		}
		public async Task<List<Item>> GetAllProductsAsync()
		{
			List<Item> productList = new List<Item>();
			var url = "api/products";
			var request = new RestRequest(url, Method.GET, DataFormat.Json);

			ImageService imageService = new ImageService();

			try
			{
				var result = await client.ExecuteAsync(request);
				var myData = JsonConvert.DeserializeObject<List<Item>>(result.Content);

				if (result.StatusCode == System.Net.HttpStatusCode.OK)
				{
					productList = myData;
					foreach (Item item in productList)
						imageService.getImage(item);
				}
				return productList;
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
			}
			return productList;
		}

		public async Task<Item> GetProductAsync(int id)
		{
			Item product = new Item();
			var url = "api/product/" + id;
			var request = new RestRequest(url, Method.GET, DataFormat.Json);
			try
			{
				var result = await client.ExecuteAsync<Item>(request);

				if (result.StatusCode == System.Net.HttpStatusCode.OK)
				{
					product = result.Data;
					new ImageService().getImage(product);
				}
				return product;
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
			}
			return product;
		}

		public async Task addToCart(int item_id, string selectedSize, string selectedAmount)
		{
			var url = "api/product/addtocart";
			var request = new RestRequest(url, Method.POST, DataFormat.Json);
			var token = await SecureStorage.GetAsync("token");
			request.AddHeader("x-access-token", token);
			//request.AddParameter("item_id", item_id);
			//request.AddParameter("selectedSize", selectedSize);
			//request.AddParameter("selectedAmount", selectedAmount);
			request.AddJsonBody(new { item_id = item_id, selectedSize = selectedSize, selectedAmount = selectedAmount });

			try
			{
				var result = await client.ExecuteAsync(request);

				if(result.StatusCode == System.Net.HttpStatusCode.OK)
					await Application.Current.MainPage.DisplayAlert("Success", result.Content, "OK");
				else
					await Application.Current.MainPage.DisplayAlert("Error", result.Content, "OK");
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Error","Something went wrong!", "OK");
			}
		}
	}
}
