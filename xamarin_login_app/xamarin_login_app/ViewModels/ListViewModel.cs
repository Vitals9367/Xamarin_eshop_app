using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using xamarin_login_app.Models;
using xamarin_login_app.Services;
using xamarin_login_app.Views;

namespace xamarin_login_app.ViewModels
{
	class ListViewModel:BaseViewModel
	{

		public delegate void UpdateCart();
		public event UpdateCart changeUi;

		public ObservableCollection<Item> SearchList{ get; set; }

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

		private ObservableCollection<string> _CategoryList;
		public ObservableCollection<string> CategoryList
		{
			set
			{
				this._CategoryList = value;
				OnPropertyChanged();
			}
			get
			{
				return this._CategoryList;
			}
		}

		public ObservableCollection<string> AmountList { get; set; }

		public Item SelectedItem { get; set; }

		public Command ViewItem { get; set; }

		public ListViewModel()
		{
			SearchList = new ObservableCollection<Item>();
			CategoryList = new ObservableCollection<string>();
			ViewItem = new Command(async () => await ViewItemAsync());
		}

		private async Task ViewItemAsync()
		{
			if (IsBusy)
				return;
			try
			{
				IsBusy = true;
				if (SelectedItem != null)
				{
					new ImageService().getImage(SelectedItem);
					await Application.Current.MainPage.Navigation.PushAsync(new ProductView(SelectedItem));
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("Error", "No item selected!", "OK");
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

		public async Task GetProducts()
		{
			if (IsBusy)
				return;
			try
			{
				CategoryList.Clear();
				SearchList.Clear();
				var data = await new ProductService().GetAllProductsAsync();

				foreach (var item in data)
				{
					SearchList.Add(item);
					if (!CategoryList.Contains(item.item_type.name))
					{
						CategoryList.Add(item.item_type.name);
					}
				}
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

		public ObservableCollection<Item> UpdateByName(string text)
		{
			if (IsBusy)
				return null;
			try
			{
				IsBusy = true;

				ObservableCollection<Item> itemList = new ObservableCollection<Item>();

					var list = SearchList.Where(c => c.name.ToLower().Contains(text.Trim().ToLower()));

					foreach (var item in list)
						itemList.Add(item);
					return itemList;

			}
			catch (Exception ex)
			{
				Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
			finally
			{
				IsBusy = false;
			}
			return null;
		}

	}
}
