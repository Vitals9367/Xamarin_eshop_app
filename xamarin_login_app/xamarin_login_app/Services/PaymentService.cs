using Stripe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace xamarin_login_app.Services
{
	class PaymentService
	{

		public string TestApiKey = "sk_test_QrZUDZ7oiRfB7ar1c9GMewMp00wTwWj2uD";

		public PaymentService()
		{
			StripeConfiguration.SetApiKey(TestApiKey);

		}

		public async Task<bool> MakePayment(string cardNumber, int expYear, int expMonth, string cvc,string name, string email,int price)
		{
			try
			{
				TokenCardOptions stripecard = new TokenCardOptions
				{
					Number = cardNumber,
					ExpYear = expYear,
					ExpMonth = expMonth,
					Cvc = cvc,

				};

				TokenCreateOptions token = new TokenCreateOptions();
				token.Card = stripecard;
				TokenService tokenService = new TokenService();
				Token newToken = tokenService.Create(token);

				var options = new SourceCreateOptions
				{
					Type = SourceType.Card,
					Currency = "eur",
					Token = newToken.Id,
				};

				var service = new SourceService();
				Source source = service.Create(options);

				CustomerCreateOptions customer = new CustomerCreateOptions()
				{
					Name = name,
					Email = email,
					Description = "Customer",
				};
				var customerService = new CustomerService();
				Customer stripeCustomer = customerService.Create(customer);

				var chargeOptions = new ChargeCreateOptions
				{
					Amount = price * 100,
					Currency = "EUR",
					ReceiptEmail = "eshop@gmail.com",
					Customer = stripeCustomer.Id,
					Source = source.Id,
				};

				var service1 = new ChargeService();
				Charge charge = service1.Create(chargeOptions);
				return true;
			}
			catch (Exception ex)
			{
				await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
				return false;
			}
		}
	}
}
