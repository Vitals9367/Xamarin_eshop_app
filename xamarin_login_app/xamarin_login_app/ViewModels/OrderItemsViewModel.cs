using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using xamarin_login_app.Models;
using xamarin_login_app.Services;
using xamarin_login_app.Views;

namespace xamarin_login_app.ViewModels
{
	class OrderItemsViewModel : BaseViewModel
	{

		public delegate void updateUi(bool loading);
		public event updateUi LoadingView;

		private Orders _SelectedOrder;
		public Orders SelectedOrder
		{
			set
			{
				this._SelectedOrder = value;
				OnPropertyChanged();
			}
			get
			{
				return this._SelectedOrder;
			}
		}

		public ObservableCollection<Order_Items> order_items { get; set; }

		public Order_Items SelectedItem { get; set; }

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

		public Command PaymentCommand { get; set; }
		public Command CancelCommand { get; set; }
		public Command ViewItem { get; set; }

		public OrderItemsViewModel()
		{
			order_items = new ObservableCollection<Order_Items>();

			PaymentCommand = new Command(async () => await PaymentCommandAsync());
			CancelCommand = new Command(async () => await CancelCommandAsync());
			ViewItem = new Command(async () => await ViewItemAsync());
		}

		private async Task PaymentCommandAsync()
		{
			if (IsBusy)
				return;
			try
			{
				IsBusy = true;
				LoadingView.Invoke(true);
				UserInfoService infoService = new UserInfoService();
				User_Info info =  await infoService.GetInfoAsync();
				if (info.address == null || info.first_name == null || info.last_name == null || info.phone_number == null)
				{
					await Application.Current.MainPage.Navigation.PushAsync(new ProfileView());
					await Application.Current.MainPage.DisplayAlert("Oops", "Please fill your information", "OK");
				}
				else
				{
					await Application.Current.MainPage.Navigation.PushAsync(new PaymentView(SelectedOrder));
				}
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
			finally
			{
				LoadingView.Invoke(false);
				IsBusy = false;
			}
		}

		private async Task CancelCommandAsync()
		{
			if (IsBusy)
				return;
			try
			{
				IsBusy = true;

				bool answer = await Application.Current.MainPage.DisplayAlert("Reminder", "Do you really want to delete order?", "Yes", "No");

				if (answer) {
					LoadingView.Invoke(true);
					var ordersService = new OrdersService();
					var result = await ordersService.DeleteItemAsync(SelectedOrder.id);

					await Application.Current.MainPage.Navigation.PopAsync();
					await Application.Current.MainPage.DisplayAlert("Success", result, "OK");
					LoadingView.Invoke(false);
				}
				else
				{
					return;
				}
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

		public void updateItem(Orders Order)
		{
			SelectedOrder = Order;
			ImageService imageService = new ImageService();
			foreach (Order_Items ord_item in SelectedOrder.order_items)
			{
				imageService.getImage(ord_item.defined_item.item);
				order_items.Add(ord_item);
			}
		}
		public async Task updateOrder()
		{
			var ordersService = new OrdersService();
			var result = await ordersService.GetProductAsync(SelectedOrder.id);
			if (result!=null)
			{
				updateItem(result);
			}
		}
		private async Task ViewItemAsync()
		{
			if (IsBusy)
				return;
			try
			{
				IsBusy = true;
				if (SelectedItem != null)
					await Application.Current.MainPage.Navigation.PushAsync(new ProductView(SelectedItem.defined_item.item));
				else
					await Application.Current.MainPage.DisplayAlert("Error", "No item selected!", "OK");
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

	}
}
