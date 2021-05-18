using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xamarin_login_app.Models;

namespace unit_testing
{
	[NUnit.Framework.TestFixture]
	class RestTesting
	{
		RestClient client = new RestClient("http://192.168.8.112:5000");

		[Test]
		public async Task<bool> RegisterUser(string uname, string passwd)
		{
			if (await UserExists(uname) != false)
			{
				Console.WriteLine("User Exists");
				return true;
			}
			else
			{
				Console.WriteLine("User does not exist");
				return false;
			}
		}
		[Test]
		public async Task<bool> LoginUser(string uname, string passwd)
		{
			if (await UserExists(uname))
				return true;
			return false;
		}
		[Test]
		private async Task<bool> UserExists(string uname)
		{
			var url = "api/users/" + uname;
			var request = new RestRequest(url, DataFormat.Json);
			var response = await ExecuteAsyncRequest<User>(client, request);

			if (response != null)
				return true;
			else
				return false;

		}
		[Test]
		private async Task<IRestResponse<T>> ExecuteAsyncRequest<T>(RestClient client, IRestRequest request) where T : class, new()
		{
			var taskCompletionSource = new TaskCompletionSource<IRestResponse<T>>();

			client.ExecuteAsync<T>(request, restResponse =>
			{
				if (restResponse.ErrorException != null)
				{
					const string message = "Error retrieving response.";
					throw new ApplicationException(message, restResponse.ErrorException);
				}
				taskCompletionSource.SetResult(restResponse);

			});

			return await taskCompletionSource.Task;

		}
	}
}
