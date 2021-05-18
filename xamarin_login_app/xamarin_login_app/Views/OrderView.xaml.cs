using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamarin_login_app.ViewModels;

namespace xamarin_login_app.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OrderView : ContentPage
	{

		OrdersViewModel VM => (OrdersViewModel)this.BindingContext;

		public OrderView()
		{
			InitializeComponent();
			VM.changeUi += changeOverlay;
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();

			Overlay.IsVisible = true;
			MainPage.IsVisible = false;
			EmptyCart.IsVisible = false;

			await VM.GetProducts();
		}
		public void changeOverlay()
		{
			if (VM.SearchList.Count == 0)
			{
				Overlay.IsVisible = false;
				MainPage.IsVisible = false;
				EmptyCart.IsVisible = true;
			}
			else
			{
				Overlay.IsVisible = false;
				MainPage.IsVisible = true;
				EmptyCart.IsVisible = false;
			}
		}
		private void col1_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
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