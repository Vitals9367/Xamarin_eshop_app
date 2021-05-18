using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Windows.Input;
using xamarin_login_app.Models;
using xamarin_login_app.Services;
using xamarin_login_app.Views;

namespace xamarin_login_app.ViewModels
{
	class CartViewModel : BaseViewModel
	{

		public delegate void loadingUi(bool loading);
		public event loadingUi LoadingView;

		public delegate void UpdateCart();
		public event UpdateCart changeUi;

		private ObservableCollection<Cart_Items> _SearchList;
		public ObservableCollection<Cart_Items> SearchList
		{
			set
			{
				this._SearchList = value;
				OnPropertyChanged();
			}
			get
			{
				return this._SearchList;
			}
		}

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

		private float _totalPrice;
		public float totalPrice
		{
			set
			{
				this._totalPrice = value;
				OnPropertyChanged();
			}
			get
			{
				return this._totalPrice;
			}
		}

		public Cart_Items SelectedItem { get; set; }

		public Command OrderCommand { get; set; }
		public Command ViewItem { get; set; }

		public CartViewModel()
		{
			SearchList = new ObservableCollection<Cart_Items>();
			OrderCommand = new Command(async () => await OrderCommandAsync());
			ViewItem = new Command(async () => await ViewItemAsync());
		}

		public async Task GetProducts()
		{
			if (IsBusy)
				return;
			try
			{
				IsBusy = true;
				SearchList.Clear();
				List<Cart_Items> data = await new CartItemService().GetAllProductsAsync();
				foreach (Cart_Items item in data)
					SearchList.Add(item);
				updateInfo();
				changeUi.Invoke();
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("View Model Error", ex.Message, "OK");
			}
			finally
			{
				IsBusy = false;
			}
		}

		public async Task DeleteItem(int id)
		{
			if (IsBusy)
				return;
			try
			{
				IsBusy = true;

				var response = await new CartItemService().DeleteItemAsync(id);
				foreach (var n in SearchList.Where(item => item.id == id).ToArray()) SearchList.Remove(n);
				updateInfo();
				await Application.Current.MainPage.DisplayAlert("Message", response, "OK");
				changeUi.Invoke();
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
		private async Task OrderCommandAsync()
		{
			if (IsBusy)
				return;
			try
			{
				if (SearchList.Count == 0)
				{
					await Application.Current.MainPage.DisplayAlert("Error", "The cart is empty!", "OK");
					return;
				}
				else
				{
					IsBusy = true;
					LoadingView.Invoke(true);
					Orders order = await new OrdersService().CreateOrder();
					if (order != null)
					{
						await Application.Current.MainPage.Navigation.PushAsync(new OrderItemsView(order));
						await Application.Current.MainPage.DisplayAlert("Message", "Order has been created!", "OK");
					}
					else
					{
						await Application.Current.MainPage.DisplayAlert("Message", "Error!", "OK");
					}
					LoadingView.Invoke(false);
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

		private void updateInfo()
		{
			totalPrice = 0;

			foreach (Cart_Items items in SearchList)
			{
				totalPrice += items.defined_item.item.price * items.defined_item.amount;
			}

		}
	}
}

