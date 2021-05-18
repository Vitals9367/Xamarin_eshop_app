using Stripe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using xamarin_login_app.Models;
using xamarin_login_app.Services;
using xamarin_login_app.Views;

namespace xamarin_login_app.ViewModels
{
	class PaymentViewModel:BaseViewModel
	{

		public delegate void updateUi(bool loading);
		public event updateUi LoadingView;

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

		private Orders _CurrentOrder;
		public Orders CurrentOrder
		{
			set
			{
				this._CurrentOrder = value;
				OnPropertyChanged();
			}
			get
			{
				return this._CurrentOrder;
			}
		}

		private string _Name;
		public string Name
		{
			set
			{
				this._Name = value;
				OnPropertyChanged();
			}
			get
			{
				return this._Name;
			}
		}

		private string _CardNumber;
		public string CardNumber
		{
			set
			{
				this._CardNumber = value;
				OnPropertyChanged();
			}
			get
			{
				return this._CardNumber;
			}
		}

		private string _Cvc;
		public string Cvc
		{
			set
			{
				this._Cvc = value;
				OnPropertyChanged();
			}
			get
			{
				return this._Cvc;
			}
		}

		private string _ExpMonth;
		public string ExpMonth
		{
			set
			{
				this._ExpMonth = value;
				OnPropertyChanged();
			}
			get
			{
				return this._ExpMonth;
			}
		}

		private string _ExpYear;
		public string ExpYear
		{
			set
			{
				this._ExpYear = value;
				OnPropertyChanged();
			}
			get
			{
				return this._ExpYear;
			}
		}

		public Command MakePayment { get; set; }

		public PaymentViewModel()
		{
			MakePayment = new Command(async () => await MakePaymentAsync());
		}

		public async Task MakePaymentAsync()
		{
			if (IsBusy)
				return;
			try
			{
				LoadingView.Invoke(true);
				IsBusy = true;
				PaymentService paymentService = new PaymentService();

				int eY = int.Parse(ExpYear);
				int eM = int.Parse(ExpMonth);
				string email;
				var info = await new UserService().getUserDetails();
				if (info != null)
				{
					email = info.email;
				}
				else
				{
					email = "a@gmail.com";
				}
				var result = await paymentService.MakePayment(CardNumber, eY, eM, Cvc,Name,email,(int)CurrentOrder.price);
				if (result == true)
				{
					var RestCall = await new OrdersService().CompletePayment(CurrentOrder.id);
					if (RestCall == true)
					{
						CurrentOrder.paid = true;
						await Task.WhenAll(
							Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync(),
							Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync()
							);
						
						await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Success", "Payment Succesfull!", "OK");
					}
					else
					{
						await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Failed!", "OK");
					}
				}
			}
			catch (Exception ex)
			{
				await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
			finally
			{
				LoadingView.Invoke(false);
				IsBusy = false;
			}
		}

	}
}
