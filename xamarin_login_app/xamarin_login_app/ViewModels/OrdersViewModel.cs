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
	class OrdersViewModel : BaseViewModel
	{

		public delegate void UpdateCart();
		public event UpdateCart changeUi;

		private ObservableCollection<Orders> _SearchList;
		public ObservableCollection<Orders> SearchList
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

		private Orders _SelectedItem;
		public Orders SelectedItem
		{
			set
			{
				this._SelectedItem = value;
				OnPropertyChanged();
			}
			get
			{
				return this._SelectedItem;
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

		public Command ViewItem { get; set; }
		//public Command RemoveCommand { get; set; }
		//public Command OrderCommand { get; set; }

		public OrdersViewModel()
		{
			SearchList = new ObservableCollection<Orders>();
			//RemoveCommand = new Command(async () => await RemoveCommandAsync());
			//OrderCommand = new Command(async () => await OrderCommandAsync());
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
				var data = await new OrdersService().GetAllProductsAsync();

				foreach (var item in data)
					SearchList.Add(item);
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
					await Application.Current.MainPage.Navigation.PushAsync(new OrderItemsView(SelectedItem));
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
