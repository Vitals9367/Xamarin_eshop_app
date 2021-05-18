using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamarin_login_app.Models;
using xamarin_login_app.ViewModels;

namespace xamarin_login_app.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CartView : ContentPage
	{
		CartViewModel VM => (CartViewModel)this.BindingContext;

		public CartView()
		{
			InitializeComponent();
			VM.changeUi += changeOverlay;
			VM.LoadingView += loadingUi;
			Title ="Cart";
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();

			Overlay.IsVisible = true;
			MainPage.IsVisible = false;
			EmptyCart.IsVisible = false;

			await VM.GetProducts();
			changeOverlay();

		}
		
		public void changeOverlay()
		{
			if (VM.SearchList.Count == 0)
			{
				OrderBtn.IsEnabled = false;
				Overlay.IsVisible = false;
				MainPage.IsVisible = false;
				EmptyCart.IsVisible = true;
			}
			else
			{
				OrderBtn.IsEnabled = true;
				Overlay.IsVisible = false;
				MainPage.IsVisible = true;
				EmptyCart.IsVisible = false;
			}
		}

		public void loadingUi(bool loading)
		{
			if (loading)
			{
				OrderBtn.IsEnabled = false;
				Overlay.IsVisible = true;
				MainPage.IsVisible = false;
				EmptyCart.IsVisible = false;
			}
			else
			{
				OrderBtn.IsEnabled = true;
				Overlay.IsVisible = false;
				MainPage.IsVisible = true;
				EmptyCart.IsVisible = false;
			}
		}

		private async void btn_Clicked(object sender, EventArgs e)
		{
			Cart_Items item = (Cart_Items)((StackLayout)sender).BindingContext;
			if (item != null)
			{
				await VM.DeleteItem(item.id);
			}
		}
		private void col1_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{

			//if (e.SelectedItem == null)
			//return;

			VM.ViewItem.Execute(null);
			col1.SelectedItem = null;
		}
		private void col1_Refreshing(object sender, EventArgs e)
		{
			Overlay.IsVisible = true;
			MainPage.IsVisible = false;
			EmptyCart.IsVisible = false;
			VM.GetProducts().GetAwaiter();
			col1.EndRefresh();
		}
	}
}