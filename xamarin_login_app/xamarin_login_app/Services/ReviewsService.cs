using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using System.Threading.Tasks;
using xamarin_login_app.Models;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace xamarin_login_app.Services
{
	class ReviewsService
	{

		RestClient client;

		public ReviewsService()
		{
			client = new RestClient(Constants.RestIP);
		}
		public async Task<List<Reviews>> GetReviews(int item_id)
		{
			List<Reviews> Comments = new List<Reviews>();
			var url = "api/reviews/product/" + item_id;
			var request = new RestRequest(url, Method.GET, DataFormat.Json);

			try
			{
				var result = await client.ExecuteAsync(request);
				if (result.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var myData = JsonConvert.DeserializeObject<List<Reviews>>(result.Content);
					Comments = myData;
					return Comments;
				}
				else
				{
					return Comments;
				}
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
			}
			return Comments;
		}
		public async Task<string> CreateReviews(int item_id,int rating, string comment)
		{
			var url = "api/create_review";
			var request = new RestRequest(url, Method.POST, DataFormat.Json);
			request.AddJsonBody(new { item_id = item_id, rating = rating, comment = comment });
			var token = await SecureStorage.GetAsync("token");
			request.AddHeader("x-access-token", token);
			try
			{
				var result = await client.ExecuteAsync<string>(request);

				return result.Content;
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
			}
			return "Internal app failure";
		}
	}
}
